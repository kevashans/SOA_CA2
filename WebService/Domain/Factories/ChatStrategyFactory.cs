using Domain.Strategies.Enums;
using Domain.Strategies;
using Domain.Strategies.Interfaces;

namespace WebService.Domain.Factories;

public class ChatStrategyFactory
{
	public static IChatTypeStrategy GetChatStrategy(string chatRoomType)
	{
		return chatRoomType switch
		{
			"Casual" => new CasualChatTypeStrategy(),
			"Pirate" => new PirateChatTypeStrategy(),
			"Professional" => new ProfessionalChatTypeStrategy(),
			_ => throw new ArgumentException($"Unsupported chat room type: {chatRoomType}")
		};
	}
}