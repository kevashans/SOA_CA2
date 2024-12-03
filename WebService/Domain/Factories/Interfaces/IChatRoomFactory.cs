using Domain.DTOs;
using Domain.Entities;

namespace Domain.Factories.Interfaces;

public interface IChatRoomFactory
{
	ChatRoom CreateChatRoom(CreateChatRoomRequest chatroomRequest, string userId);
}
