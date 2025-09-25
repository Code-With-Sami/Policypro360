using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PolicyPro360.Services
{
    public interface IOpenAiService
    {
        Task<string> RankPoliciesAsync(string prompt, CancellationToken ct = default);
        Task<string> GetChatReplyAsync(List<(string role, string content)> messages, CancellationToken ct = default);
    }

    public class OpenAiService : IOpenAiService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public OpenAiService(IConfiguration config, IHttpClientFactory httpFactory)
        {
            _apiKey = config["OpenAI:ApiKey"] ?? Environment.GetEnvironmentVariable("OpenAI__ApiKey");
            _http = httpFactory.CreateClient();
        }

        // Method for a simple prompt (File 1)
        public async Task<string> RankPoliciesAsync(string prompt, CancellationToken ct = default)
        {
            var payload = new
            {
                model = "gpt-4o-mini", // Use your desired model
                input = prompt,
                max_output_tokens = 800
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var resp = await _http.PostAsync("https://api.openai.com/v1/responses", content, ct);
            var body = await resp.Content.ReadAsStringAsync(ct);
            resp.EnsureSuccessStatusCode();
            return body;
        }

        // Method for chat-based interactions (File 2)
        public async Task<string> GetChatReplyAsync(List<(string role, string content)> messages, CancellationToken ct = default)
        {
            var payload = new
            {
                model = "gpt-4o-mini", // Use your desired model
                messages = messages.Select(m => new { role = m.role, content = m.content })
            };

            var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions");
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var resp = await _http.SendAsync(req, ct);
            var body = await resp.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(body);
            return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }
    }
}
