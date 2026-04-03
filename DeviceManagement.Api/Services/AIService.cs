namespace DeviceManagement.Api.Services;

using DeviceManagement.Api.Models;
using System.Net.Http.Json;

public class AIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public AIService(IConfiguration config, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiKey = config["AI:ApiKey"] ?? "";

        if (!string.IsNullOrEmpty(_apiKey))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
        }
    }

    public async Task<string> GenerateDescriptionAsync(Device device)
    {
        if (string.IsNullOrEmpty(_apiKey)) return "Groq API Key missing.";

        var url = "https://api.groq.com/openai/v1/chat/completions";

        var prompt = $"Generate a concise, one-sentence professional description for this device: " +
                     $"Name: {device.name}, Manufacturer: {device.manufacturer}, " +
                     $"Type: {device.type}, RAM: {device.ramAmount}GB. " +
                     $"Focus on its use case and performance.";

        var requestBody = new
        {
            model = "llama-3.3-70b-versatile", 
            messages = new[] { new { role = "user", content = prompt } },
            max_tokens = 60
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(url, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GroqResponse>();
                return result?.Choices?[0]?.Message?.Content?.Trim() ?? "Description unavailable.";
            }

            var errorBody = await response.Content.ReadAsStringAsync();
            return $"Groq Error ({response.StatusCode}): {errorBody}";
        }
        catch (Exception ex)
        {
            return $"Exception: {ex.Message}";
        }
    }

    private class GroqResponse
    {
        public Choice[]? Choices { get; set; }
    }
    private class Choice
    {
        public Message? Message { get; set; }
    }
    private class Message
    {
        public string? Content { get; set; }
    }
}