namespace Domain.Entities;

public class ChatRoom
{
	public ChatRoom(string userId, string name, string chatRoomType)
	{
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
	public void ChangeType()
	{
		throw new NotImplementedException();
	}

	public void ChangeName(string newName)
	{
		if (string.IsNullOrWhiteSpace(newName))
			throw new ArgumentException("Chat room name cannot be null or empty.");

		if (newName.Length > 255)
			throw new ArgumentException("Chat room name cannot exceed 255 characters.");

		Name = newName;
	}
}
