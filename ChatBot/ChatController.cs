using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace DATN.ChatBot;

[ApiController]
[Route("api/chat")]
public class ChatController : ControllerBase
{
    private readonly HttpClient _http;
    private readonly IConfiguration _cfg;

    public ChatController(IHttpClientFactory factory, IConfiguration cfg)
    {
        _http = factory.CreateClient("gemini");
        _cfg = cfg;
    }

    public record ChatReq(string question);
    public record ChatResp(string reply);

    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] ChatReq req)
    {
        if (req is null || string.IsNullOrWhiteSpace(req.question))
            return BadRequest(new { message = "question không được để trống" });

        var model = _cfg["Gemini:Model"];
        if (string.IsNullOrWhiteSpace(model))
            return StatusCode(500, "Thiếu cấu hình Gemini:Model");

        var storeName = _cfg["Gemini:StoreName"];
        if (string.IsNullOrWhiteSpace(storeName))
            return StatusCode(500, "Thiếu cấu hình Gemini:StoreName");

        var url = $"{model}:generateContent";

        var payload = new
        {
            contents = new[]
            {
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new
                        {
                            text = $"Chỉ trả lời dựa trên dữ liệu trong kho. Nếu không thấy, trả lời: 'Không tìm thấy trong dữ liệu'. Câu hỏi: {req.question}"
                        }
                    }
                }
            },
            tools = new object[]
            {
                new
                {
                    file_search = new
                    {
                        file_search_store_names = new[] { storeName }
                    }
                }
            },
            generationConfig = new
            {
                temperature = 0.2
            }
        };

        var res = await _http.PostAsJsonAsync(url, payload);
        var json = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
            return StatusCode((int)res.StatusCode, json);

        using var doc = JsonDocument.Parse(json);
        var root = doc.RootElement;

        string? reply = null;

        if (root.TryGetProperty("candidates", out var candidates) && candidates.GetArrayLength() > 0)
        {
            var c0 = candidates[0];
            if (c0.TryGetProperty("content", out var content) &&
                content.TryGetProperty("parts", out var parts))
            {
                reply = string.Join("", parts.EnumerateArray()
                    .Where(p => p.TryGetProperty("text", out _))
                    .Select(p => p.GetProperty("text").GetString()));
            }
        }

        return Ok(new ChatResp(reply ?? "Không có phản hồi"));
    }
}