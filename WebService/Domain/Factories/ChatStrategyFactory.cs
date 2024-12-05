using Domain.Factories.Interfaces;
using Domain.Strategies;
using Domain.Strategies.Interfaces;

namespace Domain.Factories;

public class ChatStrategyFactory : IChatStrategyFactory
{
	private readonly IChatResponseGenerator _responseGenerator;

	public ChatStrategyFactory(IChatResponseGenerator responseGenerator)
	{
		_responseGenerator = responseGenerator;
	}
	public IChatTypeStrategy GetChatStrategy(string chatRoomType)
	{
		return chatRoomType.ToLower() switch
		{
			"casual" => new CasualChatTypeStrategy(_responseGenerator),
			"pirate" => new PirateChatTypeStrategy(_responseGenerator),
			"professional" => new ProfessionalChatTypeStrategy(_responseGenerator),
			_ => throw new ArgumentException($"Unsupported chat room type: {chatRoomType}")
		};
	}
}