// Controllers/QuizController.cs
using Microsoft.AspNetCore.Mvc;
using PolicyPro360.Services;
using PolicyPro360.Models;
using System.Text.Json;

namespace PolicyPro360.Controllers
{
    public class QuizController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly myContext _db;

        public QuizController(IQuizService quizService, myContext db)
        {
            _quizService = quizService;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetActive()
        {
            var quiz = await _quizService.GetActiveQuizAsync();
            if (quiz == null) return NotFound();

            // sanitize - do not include CategoryWeightsJson in options
            var dto = new
            {
                quiz.Id,
                quiz.Title,
                Questions = quiz.Questions.Select(q => new
                {
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit([FromBody] SubmitRequest request)
        {
            if (request == null || request.Answers == null) return BadRequest();

            int? userId = HttpContext.Session.GetInt32("userId");
            var answersForCompute = request.Answers.Select(a => (a.QuestionId, a.OptionIds.AsEnumerable(), a.RawAnswer));
            var scores = await _quizService.ComputeCategoryScoresAsync(answersForCompute);

            // determine top categories
            var orderedCats = scores.OrderByDescending(kv => kv.Value).Select(kv => kv.Key).Take(2).ToList();

            // get candidate policies
            var candidates = await _quizService.GetCandidatePoliciesAsync(orderedCats, 12);

            // friendly user summary (no PII)
            var userSummary = $"Anonymous user; answers: {string.Join(", ", request.Answers.Select(a => a.RawAnswer ?? string.Join("/", a.OptionIds)))}";

            var (ranked, aiRaw) = await _quizService.RankWithAiAsync(userSummary, candidates);

            // suggested policy ids csv
            var suggestedCsv = string.Join(",", ranked.Select(r => r.policy.Id));

            // save results
            var answersToSave = new List<QuizAnswer>();
            foreach (var a in request.Answers)
            {
                answersToSave.Add(new QuizAnswer
                {
                    QuestionId = a.QuestionId,
                    OptionIdsCsv = a.OptionIds != null ? string.Join(",", a.OptionIds) : "",
                    RawAnswer = a.RawAnswer ?? ""
                });
            }

            var saved = await _quizService.SaveResultAsync(userId, request.QuizId, scores, answersToSave, aiRaw, suggestedCsv);

            // prepare response to client
            var response = new
            {
                saved.Id,
                Scores = scores,
                Recommended = ranked.Select(r => new { r.policy.Id, r.policy.Name, r.policy.Premium, r.policy.SumInsured, r.reason, r.score }),
            };

            return Json(response);
        }
    }
}
