namespace WebService.DTOs;

/// <summary>
/// Data transfer object for ChatRoom entity
/// </summary>
public class ChatRoomDto
{
	/// <summary>
	/// The ID of the chatroom
	/// </summary>
	public Guid ChatRoomId { get; set; }

	/// <summary>
	/// The name of the chat room
	/// </summary>
	public string Name { get; set; } = null!;

	/// <summary>
	/// The type or context of the chat room discussion
	/// </summary>
	public string ChatRoomType { get; set; } = null!;
}
