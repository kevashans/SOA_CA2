using Domain.Strategies.Interfaces;

namespace Domain.Strategies;

public class CasualChatTypeStrategy : IChatTypeStrategy
{

	private readonly IChatResponseGenerator _responseGenerator;

	public CasualChatTypeStrategy(IChatResponseGenerator responseGenerator)
	{
		_responseGenerator = responseGenerator;
	}
	/// <summary>
	/// Return ChatGPT response in a casual context
	/// </summary>
	/// <param name="userMessage"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public Task<string> Respond(string userMessage)
	{
		// responseGenerote.generate
		throw new NotImplementedException();
	}
}
