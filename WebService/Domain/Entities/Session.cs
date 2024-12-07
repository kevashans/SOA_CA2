namespace Domain.Entities;

public class Session
{
	public Guid SessionId { get; private set; }

	public Guid ChatRoomId { get; private set; }

	public DateTime StartTime { get; private set; }

	public DateTime? EndTime { get; private set; }

	public string Context { get; private set; } = null!;

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

	public void End()
	{
		if (EndTime != null)
			throw new InvalidOperationException("Session ended already.");

		EndTime = DateTime.UtcNow;
		Context += "\n[Session Ended]";
	}

	public void UpdateSummary(string updatedSummary)
	{
		Context = updatedSummary;
	}
}
