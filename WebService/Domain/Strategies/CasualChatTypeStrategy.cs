using Domain.Strategies.Interfaces;

namespace Domain.Strategies;

public class CasualChatTypeStrategy : IChatTypeStrategy
{

	private readonly IChatResponseGenerator _responseGenerator;
	private string _systemPrompt = "Please answer in a casual format";

	public CasualChatTypeStrategy(IChatResponseGenerator responseGenerator)
	{
		_responseGenerator = responseGenerator;
	}

	public async Task<string> Respond(string userMessage)
	{
		return await _responseGenerator.GenerateResponseAsync(_systemPrompt,userMessage);
	}

	public void ProvideContext(string? summary)
	{
		_systemPrompt += $" keep in mind the previous chat messages: {summary}";
	}
}
