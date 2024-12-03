using Domain.Strategies.Interfaces;


namespace Domain.Strategies;

internal class PirateChatTypeStrategy : IChatTypeStrategy
{
	private readonly IChatResponseGenerator _responseGenerator;

	public PirateChatTypeStrategy(IChatResponseGenerator responseGenerator)
	{
		_responseGenerator = responseGenerator;
	}

	public Task<string> Respond(string userMessage)
	{
		throw new NotImplementedException();
	}
}
