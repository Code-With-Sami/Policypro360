using Microsoft.AspNetCore.Mvc;
using PolicyPro360.Services;
using PolicyPro360.Models;

namespace PolicyPro360.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<IActionResult> GetActive()
        {
            var quiz = await _quizService.GetActiveQuizAsync();
            if (quiz == null) return NotFound();

            var dto = new
            {
                id = quiz.Id,
                title = quiz.Title,
                Questions = quiz.Questions.Select(q => new {
                    q.Id,
                    q.Text,
                    q.QuestionType,
                    q.Order,
                    Options = q.Options.Select(o => new { o.Id, o.Text })
                }).OrderBy(q => q.Order)
            };

            return Json(dto);
        }

        public class SubmitRequest
        {
            public int QuizId { get; set; }
            public List<AnswerDto> Answers { get; set; }
        }

        public class AnswerDto
        {
            public int QuestionId { get; set; }
            public List<int> OptionIds { get; set; } = new();
            public string RawAnswer { get; set; }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit([FromBody] SubmitRequest request)
        {
            if (request == null || request.Answers == null) return BadRequest();

            int? userId = HttpContext.Session.GetInt32("userId");

            var answersForCompute = request.Answers
    .Select(a => (a.QuestionId, (a.OptionIds ?? new List<int>()).AsEnumerable(), a.RawAnswer));

            var scores = await _quizService.ComputeCategoryScoresAsync(answersForCompute);

            var orderedCats = scores.OrderByDescending(kv => kv.Value)
                                    .Select(kv => kv.Key).Take(2).ToList();

            var candidates = await _quizService.GetCandidatePoliciesAsync(orderedCats, 12);

            var userSummary = $"Anonymous user; answers: {string.Join(", ", request.Answers.Select(a => a.RawAnswer ?? string.Join("/", a.OptionIds)))}";

            List<(Policy policy, string reason, decimal score)> ranked;
            string aiRaw;

            try
            {
                (ranked, aiRaw) = await _quizService.RankWithAiAsync(userSummary, candidates);
            }
            catch
            {
                ranked = candidates.Take(3)
                    .Select((p, i) => (p, "Fallback suggestion", 1m - i * 0.1m))
                    .ToList();
                aiRaw = "{}";
            }


            var suggestedCsv = string.Join(",", ranked.Select(r => r.policy.Id));

            var answersToSave = request.Answers.Select(a => new QuizAnswer
            {
                QuestionId = a.QuestionId,
                OptionIdsCsv = a.OptionIds != null ? string.Join(",", a.OptionIds) : "",
                RawAnswer = a.RawAnswer ?? ""
            }).ToList();

            var saved = await _quizService.SaveResultAsync(userId, request.QuizId, scores, answersToSave, aiRaw, suggestedCsv);

            var response = new
            {
                id = saved.Id,
                scores,
                recommended = ranked.Select(r => new
                {
                    id = r.policy.Id,
                    name = r.policy.Name,
                    premium = r.policy.Premium,
                    sumInsured = r.policy.SumInsured,
                    reason = r.reason,
                    score = r.score
                })
            };

            return Json(response);
        }
    }
}
