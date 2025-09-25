// Services/QuizService.cs
using System.Text.Json;
using PolicyPro360.Models;
using Microsoft.EntityFrameworkCore;

namespace PolicyPro360.Services
{
    public interface IQuizService
    {
        Task<Quiz> GetActiveQuizAsync();
        Task<Dictionary<string, decimal>> ComputeCategoryScoresAsync(IEnumerable<(int questionId, IEnumerable<int> optionIds, string raw)> answers);
        Task<List<Policy>> GetCandidatePoliciesAsync(IEnumerable<string> categories, int limit = 10);
        Task<(List<(Policy policy, string reason, decimal score)> ranked, string aiRaw)> RankWithAiAsync(string userSummary, List<Policy> candidates);
        Task<QuizResult> SaveResultAsync(int? userId, int quizId, Dictionary<string, decimal> scores, List<QuizAnswer > answersList, string aiResponseJson, string suggestedPolicyIdsCsv);
    }

    public class QuizService : IQuizService
    {
        private readonly myContext _db;
        private readonly IOpenAiService _ai;

        public QuizService(myContext db, IOpenAiService ai)
        {
            _db = db;
            _ai = ai;
        }

        public async Task<Quiz> GetActiveQuizAsync()
        {
            return await _db.Tbl_Quiz
                .Include(q => q.Questions.OrderBy(x => x.Order))
                    .ThenInclude(qt => qt.Options)
                .FirstOrDefaultAsync(q => q.IsActive);
        }

        public async Task<Dictionary<string, decimal>> ComputeCategoryScoresAsync(IEnumerable<(int questionId, IEnumerable<int> optionIds, string raw)> answers)
        {
            var scores = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
            foreach (var a in answers)
            {
                var options = await _db.Tbl_QuizOption.Where(o => a.optionIds.Contains(o.Id)).ToListAsync();
                foreach (var opt in options)
                {
                    if (!string.IsNullOrWhiteSpace(opt.CategoryWeightsJson))
                    {
                        try
                        {
                            var map = JsonSerializer.Deserialize<Dictionary<string, decimal>>(opt.CategoryWeightsJson);
                            if (map != null)
                            {
                                foreach (var kv in map)
                                {
                                    if (!scores.ContainsKey(kv.Key)) scores[kv.Key] = 0;
                                    scores[kv.Key] += kv.Value * (decimal)opt.Weight;
                                }
                            }
                        }
                        catch { /* ignore invalid json */ }
                    }
                }
            }

            // normalize (sum to 1)
            var total = scores.Values.Sum();
            if (total <= 0)
            {
                // default small values to avoid empty
                var cats = new[] { "life", "medical", "motor", "home" };
                foreach (var c in cats) scores[c] = 0;
                return scores;
            }

            var normalized = scores.ToDictionary(k => k.Key, v => Math.Round(v.Value / total, 4));
            return normalized;
        }

        public async Task<List<Policy>> GetCandidatePoliciesAsync(IEnumerable<string> categories, int limit = 10)
        {
            var catNames = categories.Select(c => c.Trim().ToLower()).ToList();

            // map category names to Category.Id (Tbl_Category)
            var catEntities = await _db.Tbl_Category
                .Where(c => catNames.Contains(c.Name.ToLower()))
                .Select(c => c.Id)
                .ToListAsync();

            var policies = await _db.Tbl_Policy
                .Where(p => p.Active && catEntities.Contains(p.PolicyTypeId))
                .OrderByDescending(p => p.CreatedAt)
                .Take(limit)
                .ToListAsync();

            return policies;
        }

        public async Task<(List<(Policy policy, string reason, decimal score)> ranked, string aiRaw)> RankWithAiAsync(string userSummary, List<Policy> candidates)
        {
            if (!candidates.Any()) return (new List<(Policy, string, decimal)>(), "{}");

            // Build a small prompt with candidate policies
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("You are an insurance advisor. Input:");
            sb.AppendLine($"UserSummary: {userSummary}");
            sb.AppendLine("Policies:");
            foreach (var p in candidates)
            {
                sb.AppendLine($"Id:{p.Id} Name:{p.Name} Premium:{p.Premium} SumInsured:{p.SumInsured} Desc:{(p.Description ?? "").Replace('\n', ' ')}");
            }
            sb.AppendLine("Task: Rank the top 3 policies for the user. Output strict JSON array: [{\"PolicyId\":123,\"Score\":0.93,\"Reason\":\"one-line reason\"}, ...]. If none match, return []");

            var prompt = sb.ToString();
            var aiRaw = await _ai.RankPoliciesAsync(prompt);

            // Try to extract JSON array from aiRaw - responses endpoint returns a JSON wrapper.
            try
            {
                // Attempt to find JSON array inside the response body
                var doc = JsonDocument.Parse(aiRaw);
                // Look for "output" or "choices" depending on endpoint. We'll search for any JSON array string in text.
                string extracted = aiRaw;
                // Try to find a JSON array in the text:
                var firstArrayIndex = aiRaw.IndexOf('[');
                var lastArrayIndex = aiRaw.LastIndexOf(']');
                if (firstArrayIndex >= 0 && lastArrayIndex > firstArrayIndex)
                {
                    var jsonArrayText = aiRaw.Substring(firstArrayIndex, lastArrayIndex - firstArrayIndex + 1);
                    var items = JsonSerializer.Deserialize<List<JsonElement>>(jsonArrayText);
                    var result = new List<(Policy, string, decimal)>();
                    foreach (var el in items)
                    {
                        var id = el.GetProperty("PolicyId").GetInt32();
                        decimal score = el.TryGetProperty("Score", out var sc) ? sc.GetDecimal() : 0;
                        var reason = el.TryGetProperty("Reason", out var r) ? r.GetString() ?? "" : "";
                        var policy = candidates.FirstOrDefault(x => x.Id == id);
                        if (policy != null) result.Add((policy, reason, score));
                    }
                    return (result, aiRaw);
                }
            }
            catch
            {
                // ignore parse errors
            }

            // fallback: return candidates without AI ranking
            return (candidates.Take(3).Select((p, i) => (p, "Recommended based on category match", 1m - i * 0.1m)).ToList(), aiRaw);
        }

        public async Task SaveQuizAnswersAsync(int resultId, List<QuizAnswer> answers)
        {
            foreach (var answer in answers)
            {
                answer.ResultId = resultId;
                _db.Tbl_QuizAnswer.Add(answer);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<QuizResult> SaveResultAsync(int? userId, int quizId, Dictionary<string, decimal> scores, List<QuizAnswer > answersList, string aiResponseJson, string suggestedPolicyIdsCsv)
        {
            var res = new QuizResult
            {
                UserId = userId,
                QuizId = quizId,
                ScoresJson = JsonSerializer.Serialize(scores),
                AiResponseJson = aiResponseJson,
                SuggestedPolicyIds = suggestedPolicyIdsCsv,
                CreatedAt = DateTime.Now
            };
            _db.Tbl_QuizResult.Add(res);
            await _db.SaveChangesAsync();

            // attach answers
            foreach (var a in answersList)
            {
                a.ResultId = res.Id;
                _db.Tbl_QuizAnswer.Add(a);
            }
            await _db.SaveChangesAsync();
            return res;
        }
    }
}
