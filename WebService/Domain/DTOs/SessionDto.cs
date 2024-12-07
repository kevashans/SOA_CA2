using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class SessionDto
{
	public class StartSessionResponse
	{
		public Guid SessionId { get; set; }

		public DateTime StartTime { get; set; }

		public string? Context { get; set; }
	}

	public class EndSessionResponse
	{
		public Guid SessionId { get; set; }

		public DateTime? EndTime { get; set; }

		public string? FinalContext { get; set; }
	}

	public class SessionResponse
	{
		public Guid SessionId { get; set; }


		public Guid ChatRoomId { get; set; }

		public DateTime StartTime { get; set; }


		public DateTime? EndTime { get; set; }


		public string? Context { get; set; }
	}
}