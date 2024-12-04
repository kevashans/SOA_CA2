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
}
