using Domain.Strategies.Interfaces;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

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

		if (responseJson?.Choices == null || responseJson.Choices.Count == 0)
		{
			throw new InvalidOperationException("API returned no choices.");
		}

		var content = responseJson.Choices.First().Message.Content;

		if (string.IsNullOrWhiteSpace(content))
		{
			throw new InvalidOperationException("API returned empty response.");
		}

		return content;
	}

	public class ChatGptResponse
	{
		[JsonPropertyName("choices")]
		public List<Choice> Choices { get; set; } = new();
	}

	public class Choice
	{
		[JsonPropertyName("message")]
		public Message Message { get; set; } = new();
	}

	public class Message
	{
		[JsonPropertyName("content")]
		public string Content { get; set; } = string.Empty;
	}
	public class ImageResponse
	{
		[JsonPropertyName("data")]
		public List<ImageData> Data { get; set; } = new();
	}

	public class ImageData
	{
		[JsonPropertyName("url")]
		public string Url { get; set; } = string.Empty;
	}
}
