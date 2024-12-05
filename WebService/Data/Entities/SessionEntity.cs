using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

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

	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	[Column("SessionId")]
	public Guid SessionId { get; set; }

	[ForeignKey("ChatRoom")]
	[Column("ChatRoomId")]
	public Guid ChatRoomId { get; set; }
	public virtual ChatRoomEntity ChatRoom { get; set; } = null!;

	[Column("StartTime")]
	[Required]
	[MaxLength(100)]
	public DateTime StartTime { get; set; }

	[Column("EndTime")]
	[MaxLength(100)]
	public DateTime? EndTime { get; set; }

	[Column("Context")]
	[Required]
	[MaxLength(2000)]
	public string Context { get; set; } = null!;
}
