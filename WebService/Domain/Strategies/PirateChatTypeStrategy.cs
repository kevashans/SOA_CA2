using Domain.Strategies.Interfaces;


namespace Domain.Strategies;

internal class PirateChatTypeStrategy : IChatTypeStrategy
{
	private readonly IChatResponseGenerator _responseGenerator;
	private readonly string _systemPrompt = "You are a pirate.";

	public PirateChatTypeStrategy(IChatResponseGenerator responseGenerator)
	{
		_responseGenerator = responseGenerator;
	}

	public async Task<string> Respond(string userMessage)
	{
		return await _responseGenerator.GenerateResponseAsync(_systemPrompt, userMessage);
	}
}
