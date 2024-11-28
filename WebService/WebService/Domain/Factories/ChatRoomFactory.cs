namespace WebService.Domain.Factories;

public static class ChatRoomFactory
{
	/// <summary>
	/// Create chat strategy based on chat room type
	/// </summary>
	/// <param name="name"></param>
	/// <param name="chatRoomType"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public static ChatRoom CreateChatRoom(string name, string chatRoomType)
	{
		throw new NotImplementedException();
		//return new ChatRoom(name, chatRoomType, strategy);
	}
}

