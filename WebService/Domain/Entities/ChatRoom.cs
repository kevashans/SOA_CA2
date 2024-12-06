using Domain.Common.Enums;

namespace Domain.Entities;

public class ChatRoom
{
	public Guid ChatRoomId { get; private set; }

	public string UserId { get; private set; } = null!;

	public string Name { get; private set; } = null!;

	public string ChatRoomType { get; private set; } = null!;

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

	public void UpdateDetails(string? name, string? chatRoomType)
	{
		if (!string.IsNullOrWhiteSpace(name))
			Name = name;

		if (!string.IsNullOrWhiteSpace(chatRoomType))
		{
			if (Enum.TryParse(typeof(ChatRoomType), chatRoomType, true, out var parsedEnumValue))
			{
				ChatRoomType = parsedEnumValue.ToString()!;
			}
			else
			{
				throw new ArgumentException($"Invalid value for ChatRoom type: {chatRoomType} ");
			}
		}
	}
}
