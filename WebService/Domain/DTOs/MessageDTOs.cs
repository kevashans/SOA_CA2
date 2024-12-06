using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class MessageDTOs
{
	public class CreateMessageRequests 
	{
		[Required]
		[StringLength(1000, ErrorMessage = "Message content cannot exceed 1000 characters.")]
		public string Content { get; set; } = null!;
	}
	public class EditMessageRequest
	{
		[Required]
		public string ChatRoomId { get; set; }

		[Required]
		[StringLength(1000, ErrorMessage = "Message content cannot exceed 1000 characters.")]
		public string NewContent { get; set; }
	}
	public class DeleteMessageRequest
	{
		[Required]
		public string ChatRoomId { get; set; }
	}

	public class MessageResponse
	{
		public Guid MessageId { get; set; }

		public Guid ChatRoomId { get; set; }

		public string Content { get; set; } = null!;

		public string MessageType { get; set; } = null!;

		public DateTime CreatedAt { get; set; }
	}

	public class CreateMessageResponse
	{
		public Guid MessageId { get; set; }

		public Guid ChatRoomId { get; set; }

		public string Content { get; set; } = null!;

		public string MessageType { get; set; } = null!;

		public DateTime CreatedAt { get; set; }
	}

	public class EditMessageResponse
	{
		public Guid MessageId { get; set; }

		public Guid ChatRoomId { get; set; }

		public string Content { get; set; } = null!;

		public DateTime UpdatedAt { get; set; }
	}
}
