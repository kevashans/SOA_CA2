using Domain.DTOs;
using Domain.Entities;
using Domain.Factories.Interfaces;
using Domain.Interfaces;
using Services.Interfaces;

namespace Services;

/// <summary>
/// Service that deals with chatroom related functionalities
/// </summary>
public class ChatRoomService : IChatRoomService
{
	private readonly IChatRoomFactory _factory;
	private readonly IChatRoomRepository _repository;

	public ChatRoomService(IChatRoomFactory factory, IChatRoomRepository repository)
	{
		_factory = factory;
		_repository = repository;
	}

	/// <summary>
	/// Creates a new ChatRoom for a specified user
	/// </summary>
	/// <param name="chatroomRequest">details of the chatroom to create</param>
	/// <param name="userId">the identifier of the user creating the chatroom</param>
	/// <returns>return the created chatroom</returns>
	public async Task<CreateChatRoomResponse> CreateChatRoom(CreateChatRoomRequest chatroomRequest, string userId)
	{
		// Use the factory to create the ChatRoom entity
		var chatRoom = _factory.CreateChatRoom(chatroomRequest, userId);

		// Save the ChatRoom using the repository
		await _repository.AddChatRoomAsync(chatRoom);
		await _repository.SaveChangesAsync();
		return new CreateChatRoomResponse
		{
			ChatRoomId = chatRoom.ChatRoomId,
			Name = chatRoom.Name,
			ChatRoomType = chatRoom.ChatRoomType,
			UserId = chatRoom.UserId
		};
	}

	/// <summary>
	/// Retrieves all chatroom owned by a specified user
	/// </summary>
	/// <param name="userId">The identifier of the user whose chatrooms is supposed to be retrieved</param>
	/// <returns>a collection of chatrooms</returns>
	public async Task<IEnumerable<ChatRoomResponse>> GetChatRoomByUserId(string userId)
	{
		var chatRooms = await _repository.GetChatRoomsByUserIdAsync(userId);

		return chatRooms.Select(chatRoom => new ChatRoomResponse
		{
			ChatRoomId = chatRoom.ChatRoomId,
			Name = chatRoom.Name,
			ChatRoomType = chatRoom.ChatRoomType,
			UserId = chatRoom.UserId
		});
	}

	/// <summary>
	/// Update details of an existing chatroom
	/// </summary>
	/// <param name="chatRoomId">Identifier of the chatroom to be updated</param>
	/// <param name="updateChatRoomRequest">Request containing the new details of the chatroom</param>
	/// <param name="userId">Identifier of the user trying to update the chatroom</param>
	/// <returns>Updated chatroom entity</returns>
	/// <exception cref="ArgumentException">thrown if chatrrom id is not a valid uuid format</exception>
	/// <exception cref="KeyNotFoundException">If chatroom with specified ID doesnt exist</exception>
	/// <exception cref="UnauthorizedAccessException">Thrown if user is not the owner of the chatroom</exception>
	public async Task<UpdateChatRoomResponse> UpdateChatRoom(string chatRoomId, UpdateChatRoomRequest updateChatRoomRequest, string userId)
	{
		if (!Guid.TryParse(chatRoomId, out Guid chatRoomGuid))
			throw new ArgumentException("Invalid ChatRoom ID format.");

		var existingChatRoom = await _repository.GetChatRoomByIdAsync(chatRoomGuid);

		if (existingChatRoom == null)
			throw new KeyNotFoundException($"ChatRoom with ID {chatRoomId} not found.");

		existingChatRoom.ValidateOwnership(userId);

		existingChatRoom.UpdateDetails(updateChatRoomRequest.Name, updateChatRoomRequest.ChatRoomType);

		await _repository.SaveAsync(existingChatRoom);
		return new UpdateChatRoomResponse
		{
			ChatRoomId = existingChatRoom.ChatRoomId,
			Name = existingChatRoom.Name,
			ChatRoomType = existingChatRoom.ChatRoomType,
		};
	}

	/// <summary>
	/// Delete chatroom by unique id
	/// </summary>
	/// <param name="chatRoomId"> The unique identifier of the chatroom to be deleted</param>
	/// <param name="userId">The unique identifier of the user trying to delete the chatroom</param>
	/// <exception cref="ArgumentException">thrown if chatRoomId is not a valid GUID format</exception>
	/// <exception cref="KeyNotFoundException">thrown if the chat room with specified id does not exist</exception>
	/// <exception cref="UnauthorizedAccessException">Thrown if user is not the owner of the chatroom</exception>

	public async Task DeleteChatRoomById(string chatRoomId, string userId)
	{
		if (!Guid.TryParse(chatRoomId, out Guid chatRoomGuid))
			throw new ArgumentException("Invalid ChatRoom ID format.");

		var chatRoom = await _repository.GetChatRoomByIdAsync(chatRoomGuid) ?? throw new KeyNotFoundException($"ChatRoom with ID {chatRoomGuid} not found");

		chatRoom.ValidateOwnership(userId);

		await _repository.DeleteChatRoomAsync(chatRoomGuid);
		await _repository.SaveChangesAsync();
	}

}
