using Domain.DTOs;
using Domain.Entities;
using Domain.Factories;

namespace Services;

public class ChatRoomService
{
	private ChatRoomFactory _factory = new();
	//private IChatRoomRepository _repository

	public ChatRoom CreateChatRoom(ChatRoomDto chatroom) 
	{
		throw new NotImplementedException();
		//factory.CreateChatRoom
		// ICXhatRoomRepo.add(charoom)
	}
}
