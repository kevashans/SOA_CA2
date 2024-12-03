using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Auth;

namespace Data.Entities;

/// <summary>
/// Represent a user created chatroom
/// contains message related metadata
/// </summary>
[Table("ChatRoom")]
public class ChatRoomEntity
{
	public ChatRoomEntity(Guid chatRoomId, string userId, string name, string chatRoomType)
	{
		ChatRoomId = chatRoomId;
		UserId = userId;
		Name = name;
		ChatRoomType = chatRoomType;
	}

	/// <summary>
	/// The ID for the chat room (Primary Key)
	/// </summary>
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("ChatRoomId")]
	public Guid ChatRoomId { get; set; }

	/// <summary>
	/// The ID for the user who owns the chatroom (Foreign Key)
	/// </summary>
	[ForeignKey("User")]
	[Column("UserId")] 
	public string UserId { get; set; } = null!;

	/// <summary>
	/// Links the chat room to a user in the AspNetUsers table 
	/// </summary>
	public virtual User User { get; set; } = null!;

	/// <summary>
	/// The name of the chat room
	/// </summary>
	[Column("Name")] 
	[Required]
	[MaxLength(255)]
	public string Name { get; set; } = null!;

	/// <summary>
	/// The context of the chatroom discussion
	/// </summary>
	[Column("ChatRoomType")]
	[Required] 
	[MaxLength(100)]  
	public string ChatRoomType { get; set; } = null!;
}
