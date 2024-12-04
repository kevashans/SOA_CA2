using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;
/// <summary>
/// Represent an input send by the user or an answer sent by chatgpt
/// </summary>
public class MessageEntity
{
	public MessageEntity(Guid messageId, Guid chatRoomId, string messageType, string content, DateTime createdAt)
	{
		MessageId = messageId;
		ChatRoomId = chatRoomId;
		MessageType = messageType;
		Content = content;
		CreatedAt = createdAt;
	}

	/// <summary>
	/// Id of the generated message (Primary Key)
	/// </summary>
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("MessageId")]
	public Guid MessageId { get; set; }

	/// <summary>
	/// The Id of the chatroom the message belong in (Foreign Key)
	/// </summary>
	[ForeignKey("ChatRoom")]
	[Column("ChatRoomId")]
	public Guid ChatRoomId { get; set; }
	public virtual ChatRoomEntity ChatRoom { get; set; } = null!;

	/// <summary>
	/// MessageType can either be input or output
	/// </summary>
	[Column("MessageType")]
	[Required]
	public string MessageType { get; set; } = null!;

	/// <summary>
	/// Content of the message
	/// </summary>
	[Column("Content")]
	[Required]
	[MaxLength(2000)]
	public string Content { get; set; } = null!;

	/// <summary>
	/// When the message is created
	/// </summary>
	[Column("CreatedAt")]
	[Required]
	public DateTime CreatedAt { get; set; }

}
