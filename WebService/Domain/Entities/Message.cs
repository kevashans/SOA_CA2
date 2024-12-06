namespace Domain.Entities;

public class Message
{
	public Guid MessageId { get; private set; }

	public Guid ChatRoomId { get; private set; }

	public string MessageType { get; private set; } = null!;

	public string Content { get; private set; } = null!;

	public DateTime CreatedAt { get; private set; }

	public Message(Guid chatRoomId, string messageType, string content, DateTime createdAt)
	{
		ChatRoomId = chatRoomId;
		MessageType = messageType;
		Content = content;
		CreatedAt = createdAt;
	}

	public Message(Guid messageId, Guid chatRoomId, string messageType, string content, DateTime createdAt)
	{
		MessageId = messageId;
		ChatRoomId = chatRoomId;
		MessageType = messageType;
		Content = content;
		CreatedAt = createdAt;
	}

	public void UpdateContent(string? newContent)
	{
		if (!string.IsNullOrWhiteSpace(newContent))
		{
			Content = newContent;
		}
	}
}