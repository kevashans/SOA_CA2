using Domain.Strategies.Interfaces;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Services;

public class ResponseGeneratorService : IChatResponseGenerator
{
	private readonly HttpClient _httpClient;
	private readonly string? _apiKey;
	private const string ChatGptApiUrl = "https://api.openai.com/v1/chat/completions";

	public ResponseGeneratorService(HttpClient httpClient, IConfiguration config)
	{
		_httpClient = httpClient;
		_apiKey = config["OpenAI:ApiKey"];
		_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
	}

	public async Task<string> GenerateResponseAsync(string systemPrompt, string userMessage)
	{
		var requestPayload = new
		{
			model = "gpt-3.5-turbo",
			messages = new[]
			{
					new { role = "system", content = systemPrompt },
					new { role = "user", content = userMessage }
				},
			max_tokens = 150
		};

		var requestBody = new StringContent(
			JsonSerializer.Serialize(requestPayload),
			Encoding.UTF8,
			"application/json"
		);

		_httpClient.DefaultRequestHeaders.Clear();
		_httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

		var response = await _httpClient.PostAsync(ChatGptApiUrl, requestBody);

		if (!response.IsSuccessStatusCode)
		{
			throw new Exception($"ChatGPT API call failed: {response.ReasonPhrase}");
		}

		var responseContent = await response.Content.ReadAsStringAsync();
		var responseJson = JsonSerializer.Deserialize<ChatGptResponse>(responseContent);

		return responseJson?.Choices?.FirstOrDefault()?.Message?.Content ?? "Sorry, I couldn't generate a response.";
	}

	private class ChatGptResponse
	{
		public List<Choice> Choices { get; set; }
	}

	private class Choice
	{
		public Message Message { get; set; }
	}

	private class Message
	{
		public string Role { get; set; }
		public string Content { get; set; }
	}
}
