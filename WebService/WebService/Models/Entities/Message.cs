using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Models.Entities;
/// <summary>
/// Represent an input send by the user or an answer sent by chatgpt
/// </summary>
public class Message
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("MessageId")]
	public Guid MessageId { get; set; }

	[ForeignKey("ChatRoom")]
	[Column("ChatRoomId")]
	public Guid ChatRoomId { get; set; }
	public virtual ChatRoom ChatRoom { get; set; } = null!;

	[Column("MessageType")]
	[Required]
	[MaxLength(100)]
	public string MessageType { get; set; } = null!;

	[Column("Content")]
	[Required]
	[MaxLength(100)]
	public string Content { get; set; } = null!;

	[Column("CreatedAt")]
	[Required]
	[MaxLength(100)]
	public DateTime CreatedAt { get; set; }

}
