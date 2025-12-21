using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly HttpClient _http;
    private readonly IConfiguration _cfg;

    public ChatController(
        IHttpClientFactory factory,
        IConfiguration cfg)
    {
        _http = factory.CreateClient("gemini");
        _cfg = cfg;
    }

    public record ChatReq(string question);
    public record ChatResp(string reply);

    [HttpPost("ask")]
    public async Task<IActionResult> Ask(ChatReq req)
    {
        var apiKey = _cfg["Gemini:ApiKey"];
        var model = _cfg["Gemini:Model"];

        var url = $"{model}:generateContent?key={apiKey}";

        var payload = new
        {
            contents = new[]
            {
                new
                {
                    role = "user",
                    parts = new[] { new { text = req.question } }
                }
            }
        };

        var res = await _http.PostAsJsonAsync(url, payload);
        var json = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
            return StatusCode((int)res.StatusCode, json);

        using var doc = JsonDocument.Parse(json);
        var reply = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return Ok(new ChatResp(reply ?? "Không có phản hồi"));
    }
}
