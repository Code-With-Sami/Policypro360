using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ControllerBase
{
    private readonly OpenAiService _openAi;

    public ChatController(OpenAiService openAi)
    {
        _openAi = openAi;
    }

    public class ChatRequest { public string Message { get; set; } }
    public class ChatResponse { public string Reply { get; set; } }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] ChatRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Message))
            return BadRequest(new { error = "Empty message" });

        // Only answer insurance/policy related queries
        var insuranceKeywords = new[] { "insurance", "policy", "premium", "claim", "loan", "coverage", "sum insured" };
        bool related = insuranceKeywords.Any(k => req.Message.ToLower().Contains(k));

        if (!related)
            return Ok(new ChatResponse { Reply = "No result found." });

        // Call OpenAI
        var messages = new List<(string role, string content)>
        {
            ("system", "You are an insurance assistant. Only answer questions related to insurance and policies. If asked anything else, say 'No result found.'"),
            ("user", req.Message)
        };

        var reply = await _openAi.GetChatReplyAsync(messages);
        return Ok(new ChatResponse { Reply = reply });
    }
}
