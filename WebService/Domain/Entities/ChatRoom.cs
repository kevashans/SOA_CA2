using Domain.Common.Enums;

namespace Domain.Entities;

public class ChatRoom
{
	public ChatRoom(string userId, string name, string chatRoomType)
	{
		UserId = userId;
		Name = name;
		ChatRoomType = chatRoomType;
	}

	public ChatRoom(Guid chatRoomId, string userId, string name, string chatRoomType)
	{
		ChatRoomId = chatRoomId;
		UserId = userId;
		Name = name;
		ChatRoomType = chatRoomType;
	}

	/// <summary>
	/// The unique identifier for the chat room.
	/// </summary>
	public Guid ChatRoomId { get; set; }

	/// <summary>
	/// The unique identifier for the user who owns the chat room.
	/// </summary>
	public string UserId { get; set; } = null!;

	/// <summary>
	/// The name of the chat room.
	/// </summary>
	public string Name { get; set; } = null!;

	/// <summary>
	/// The type or category of the chat room (e.g., General, Topic-specific, etc.).
	/// </summary>
	public string ChatRoomType { get; set; } = null!;


	/// <summary>
	/// Business rule method
	/// </summary>
	public void ValidateOwnership(string userId)
	{
		if (UserId != userId)
			throw new UnauthorizedAccessException("You are not authorized to modify this chat room.");
	}

	public (Message input, Message output) AddMessage(string prompt, string response)
	{
		var input = new Message(ChatRoomId, nameof(MessageType.Input), prompt, DateTime.UtcNow);
		var output = new Message(ChatRoomId, nameof(MessageType.Output), response, DateTime.UtcNow);

		return (input, output);
	}
}
