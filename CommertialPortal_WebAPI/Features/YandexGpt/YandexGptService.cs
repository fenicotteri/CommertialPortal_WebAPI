namespace CommertialPortal_WebAPI.Features.YandexGpt;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

public interface IYandexGptService
{
    Task<string> AskAsync(string prompt, string question, CancellationToken cancellationToken = default);
}

public class YandexGptService : IYandexGptService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public YandexGptService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> AskAsync(string prompt, string question, CancellationToken cancellationToken = default)
    {
        var apiKey = _config["YandexGpt:ApiKey"];
        var folderId = _config["YandexGpt:FolderId"]; 
        var modelUri = $"gpt://{folderId}/yandexgpt-lite"; 

        var requestBody = new
        {
            modelUri = modelUri,
            completionOptions = new
            {
                stream = false,
                temperature = 0.6,
                maxTokens = 2000
            },
            messages = new[]
            {
                new {role = "system", text = question},
                new { role = "user", text = prompt }
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post,
            "https://llm.api.cloud.yandex.net/foundationModels/v1/completion")
        {
            Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json")
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Api-Key", apiKey);
        request.Headers.Add("x-folder-id", folderId!);

        var response = await _httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        using var doc = JsonDocument.Parse(json);

        return doc.RootElement
            .GetProperty("result")
            .GetProperty("alternatives")[0]
            .GetProperty("message")
            .GetProperty("text")
            .GetString() ?? "Ответ пуст";
    }
}

