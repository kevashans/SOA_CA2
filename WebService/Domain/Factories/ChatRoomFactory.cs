using Domain.Common.Enums;
using Domain.DTOs;
using Domain.Entities;
using Domain.Factories.Interfaces;

namespace Domain.Factories;

public class ChatRoomFactory : IChatRoomFactory
{
	/// <summary>
	/// Create chat strategy based on chat room type
	/// </summary>
	/// <param name="name"></param>
	/// <param name="chatRoomType"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public ChatRoom CreateChatRoom(CreateChatRoomRequest chatroomRequest, string userId)
	{
		if (!Enum.TryParse(typeof(ChatRoomType), chatroomRequest.ChatRoomType, true, out var result) ||!Enum.IsDefined(typeof(ChatRoomType), result))
		{
			throw new ArgumentException($"Invalid ChatRoomType: {chatroomRequest.ChatRoomType}");
		}

		var chatroom = new ChatRoom(userId, chatroomRequest.Name, chatroomRequest.ChatRoomType);
		return chatroom;
	}
}

