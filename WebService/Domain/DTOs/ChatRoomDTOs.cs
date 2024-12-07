namespace Domain.DTOs
{
	public class CreateChatRoomRequest
	{
		public string Name { get; set; } = null!;

		public string ChatRoomType { get; set; } = null!;
	}

	public class UpdateChatRoomRequest
	{
		public string? Name { get; set; }

		public string? ChatRoomType { get; set; }
	}

	public class CreateChatRoomResponse
	{
		public Guid ChatRoomId { get; set; }

		public string Name { get; set; } = null!;

		public string ChatRoomType { get; set; } = null!;

		public string UserId { get; set; } = null!;
	}

	public class ChatRoomResponse
	{
		public Guid ChatRoomId { get; set; }

		public string Name { get; set; } = null!;

		public string ChatRoomType { get; set; } = null!;

		public string UserId { get; set; } = null!;
	}

	public class UpdateChatRoomResponse
	{
		public Guid ChatRoomId { get; set; }

		public string? Name { get; set; }

		public string? ChatRoomType { get; set; }
	}
}
