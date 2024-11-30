using Domain.Entities;

namespace Domain.Factories;

public class ChatRoomFactory
{
	/// <summary>
	/// Create chat strategy based on chat room type
	/// </summary>
	/// <param name="name"></param>
	/// <param name="chatRoomType"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public ChatRoom CreateChatRoom(string name, string chatRoomType)
	{
		throw new NotImplementedException();
		//return new ChatRoom(name, chatRoomType, strategy);
	}
}

