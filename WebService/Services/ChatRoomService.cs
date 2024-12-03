using Domain.DTOs;
using Domain.Entities;
using Domain.Factories.Interfaces;
using Domain.Interfaces;
using Services.Interfaces;

namespace Services;

public class ChatRoomService : IChatRoomService
{
	private readonly IChatRoomFactory _factory;
	private readonly IChatRoomRepository _repository;

	public ChatRoomService(IChatRoomFactory factory, IChatRoomRepository repository)
	{
		_factory = factory;
		_repository = repository;
	}

	public async Task<ChatRoom> CreateChatRoom(CreateChatRoomRequest chatroomRequest, string userId)
	{
		if (chatroomRequest == null)
			throw new ArgumentNullException(nameof(chatroomRequest));

		if (string.IsNullOrWhiteSpace(userId))
			throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

		// Use the factory to create the ChatRoom entity
		var chatRoom = _factory.CreateChatRoom(chatroomRequest, userId);

		// Save the ChatRoom using the repository
		await _repository.AddChatRoomAsync(chatRoom);
		await _repository.SaveChangesAsync();
		return chatRoom;
	}

	public void DeleteChatRoom() 
	{
	}

	public void UpdateChatRoom() { }

}
