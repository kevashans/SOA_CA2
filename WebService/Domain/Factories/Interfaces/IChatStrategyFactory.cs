using Domain.Strategies.Interfaces;

namespace Domain.Factories.Interfaces;

public interface IChatStrategyFactory
{
	public IChatTypeStrategy GetChatStrategy(string chatRoomType);
}
