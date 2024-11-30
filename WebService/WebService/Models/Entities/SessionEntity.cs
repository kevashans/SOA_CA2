using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebService.Models.Entities;

public class SessionEntity
{
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
	[Required]
	[MaxLength(100)]
	public DateTime EndTime { get; set; }
}
