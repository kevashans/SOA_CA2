namespace Domain.Entities;

public class ChatRoom
{
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
	public void BusinessRule()
	{
		throw new NotImplementedException();
	}
}
