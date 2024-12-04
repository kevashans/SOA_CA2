namespace Domain.Entities;

public class Session
{
	public Session(Guid sessionId, Guid chatRoomId, DateTime startTime, DateTime? endTime, string context)
	{
		SessionId = sessionId;
		ChatRoomId = chatRoomId;
		StartTime = startTime;
		EndTime = endTime;
		Context = context;
	}

	public Session(Guid chatRoomId, DateTime startTime, DateTime? endTime, string context)
	{
		ChatRoomId = chatRoomId;
		StartTime = startTime;
		EndTime = endTime;
		Context = context;
	}

	public Guid SessionId { get; set; }

	public Guid ChatRoomId { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime? EndTime { get; set; }

	public string Context { get; set; } = null!;
}
