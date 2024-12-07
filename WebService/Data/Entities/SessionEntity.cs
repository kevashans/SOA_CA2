using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

/// <summary>
/// represents a user session in a specific chatroom
/// contains session summary
/// </summary>
public class SessionEntity
{
	public SessionEntity(Guid sessionId, Guid chatRoomId, DateTime startTime, DateTime? endTime, string context)
	{
		SessionId = sessionId;
		ChatRoomId = chatRoomId;
		StartTime = startTime;
		EndTime = endTime;
		Context = context;
	}

	/// <summary>
	/// Identifier of the created session (Primary Key)
	/// </summary>
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("SessionId")]
	public Guid SessionId { get; set; }

	/// <summary>
	/// Identifier of the chatroom in which the session started (Foreign Key)
	/// </summary>
	[ForeignKey("ChatRoom")]
	[Column("ChatRoomId")]
	public Guid ChatRoomId { get; set; }
	public virtual ChatRoomEntity ChatRoom { get; set; } = null!;

	/// <summary>
	/// The time of which the session started
	/// </summary>
	[Column("StartTime")]
	[Required]
	[MaxLength(100)]
	public DateTime StartTime { get; set; }

	/// <summary>
	/// the time of which the the session ended
	/// </summary>
	[Column("EndTime")]
	[MaxLength(100)]
	public DateTime? EndTime { get; set; }

	/// <summary>
	/// the summary of user and ai conversation
	/// </summary>
	[Column("Context")]
	[Required]
	[MaxLength(2000)]
	public string Context { get; set; } = null!;
}
