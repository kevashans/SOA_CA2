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
		return chatRoomType switch
		{
			"Casual" => new CasualChatTypeStrategy(_responseGenerator),
			"Pirate" => new PirateChatTypeStrategy(_responseGenerator),
			"Professional" => new ProfessionalChatTypeStrategy(_responseGenerator),
			_ => throw new ArgumentException($"Unsupported chat room type: {chatRoomType}")
		};
	}
}