

using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs
{
	public class CreateChatRoomRequest
	{
		/// <summary>
		/// The name of the chat room
		/// </summary>
		public string Name { get; set; } = null!;

		/// <summary>
		/// The type or context of the chat room discussion
		/// </summary>
		public string ChatRoomType { get; set; } = null!;
	}

	public class UpdateChatRoomRequest
	{
		/// <summary>
		/// ChatRoom identifier
		/// </summary>
		[Required]
		public string ChatRoomId { get; set; } = null!;

		/// <summary>
		/// The name of the chat room
		/// </summary>
		public string Name { get; set; } = null!;

		/// <summary>
		/// The type or context of the chat room discussion
		/// </summary>
		public string ChatRoomType { get; set; } = null!;
	}
}
