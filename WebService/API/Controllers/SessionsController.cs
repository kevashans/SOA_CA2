using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;
using static Domain.DTOs.SessionDto;

namespace API.Controllers;

[ApiController]
[Route("api/sessions")]
[Authorize]
public class SessionsController : ControllerBase
{
	private readonly ISessionManagementService _sessionService;

	public SessionsController(ISessionManagementService sessionService)
	{
		_sessionService = sessionService;
	}

	[HttpPost("start")]
	public async Task<IActionResult> StartSession([FromBody] StartSessionRequest request)
	{
		try
		{
			// Retrieve the user ID from the token
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
				return Unauthorized();

			// Start a session using the service
			var session = await _sessionService.StartSession(request.ChatRoomId, userId);

			return Ok(new
			{
				SessionId = session.SessionId,
				StartTime = session.StartTime,
				Context = session.Context
			});
		}
		catch (UnauthorizedAccessException ex)
		{
			return Forbid();
		}
		catch (Exception ex)
		{
			return BadRequest(new { Error = ex.Message });
		}
	}

	/// <summary>
	/// Ends an active session.
	/// </summary>
	[HttpPost("end")]
	public async Task<IActionResult> EndSession([FromBody] EndSessionRequest request)
	{
		try
		{
			// Retrieve the user ID from the token
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
				return Unauthorized();

			// End the session using the service
			await _sessionService.EndSession(request.ChatRoomId, userId);

			return Ok(new { Message = "Session ended successfully." });
		}
		catch (UnauthorizedAccessException ex)
		{
			return Forbid();
		}
		catch (Exception ex)
		{
			return BadRequest(new { Error = ex.Message });
		}
	}


	/// <summary>
	/// Fetches details of an active session or the most recent session.
	/// </summary>
	[HttpGet("{chatRoomId}")]
	public async Task<IActionResult> GetSession([FromRoute] string chatRoomId)
	{
		try
		{
			// Retrieve the user ID from the token
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
				return Unauthorized();

			// Fetch the session details
			var session = await _sessionService.GetSession(chatRoomId, userId);

			if (session == null)
				return NotFound(new { Error = "No session found." });

			return Ok(new
			{
				SessionId = session.SessionId,
				StartTime = session.StartTime,
				EndTime = session.EndTime,
				Summary = session.Context
			});
		}
		catch (UnauthorizedAccessException ex)
		{
			return Forbid();
		}
		catch (Exception ex)
		{
			return BadRequest(new { Error = ex.Message });
		}
	}

}
