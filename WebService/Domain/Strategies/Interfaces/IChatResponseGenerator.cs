
namespace Domain.Strategies.Interfaces;

public interface IChatResponseGenerator
{
	Task<string> GenerateResponseAsync(string systemPrompt, string userMessage);
}
