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
		if (string.IsNullOrWhiteSpace(userId))
			throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

		// Use the factory to create the ChatRoom entity
		var chatRoom = _factory.CreateChatRoom(chatroomRequest, userId);

		// Save the ChatRoom using the repository
		await _repository.AddChatRoomAsync(chatRoom);
		await _repository.SaveChangesAsync();
		return chatRoom;
	}

	public async Task<IEnumerable<ChatRoom>> GetChatRoomByUserId(string userId)
	{
		if (string.IsNullOrWhiteSpace(userId))
			throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

		var chatRooms = await _repository.GetChatRoomsByUserIdAsync(userId);

		return chatRooms;
	}

	public async Task<ChatRoom> UpdateChatRoom(UpdateChatRoomRequest updateChatRoomRequest, string userId)
	{
		if (!Guid.TryParse(updateChatRoomRequest.ChatRoomId, out Guid chatRoomGuid))
			throw new ArgumentException("Invalid ChatRoom ID format.");

		var existingChatRoom = await _repository.GetChatRoomByIdAsync(chatRoomGuid);

		if (existingChatRoom == null)
			throw new KeyNotFoundException($"ChatRoom with ID {updateChatRoomRequest.ChatRoomId} not found.");

		// Apply business rules (e.g., only allow updates to certain fields)
		existingChatRoom.Name = updateChatRoomRequest.Name ?? existingChatRoom.Name;
		existingChatRoom.ChatRoomType = updateChatRoomRequest.ChatRoomType ?? existingChatRoom.ChatRoomType;

		await _repository.SaveAsync(existingChatRoom);
		return existingChatRoom;
	}

}
