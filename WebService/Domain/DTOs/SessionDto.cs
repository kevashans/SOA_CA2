using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs;

public class SessionDto
{
	public class StartSessionRequest 
	{
		[Required]
		public string ChatRoomId { get; set; }
	}

	public class EndSessionRequest
	{
		[Required]
		public string ChatRoomId { get; set; }
	}

	public class StartSessionResponse
	{
		public Guid SessionId { get; set; }

		public DateTime StartTime { get; set; }

		public string? Context { get; set; }
	}


}
