using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class OpenAiService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public OpenAiService(IConfiguration config, IHttpClientFactory httpFactory)
    {
        _apiKey = config["OpenAI:ApiKey"] ?? Environment.GetEnvironmentVariable("OpenAI__ApiKey");
        _http = httpFactory.CreateClient();
    }

    public async Task<string> GetChatReplyAsync(List<(string role, string content)> messages)
    {
        var payload = new
        {
            model = "gpt-4o-mini",
            messages = messages.Select(m => new { role = m.role, content = m.content })
        };

        var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var resp = await _http.SendAsync(req);
        var body = await resp.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(body);
        return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
    }
}
