using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/chatRooms/{chatRoomId}/sessions")]
[Authorize]
public class SessionsController : ControllerBase
{
	private readonly ISessionManagementService _sessionService;

	public SessionsController(ISessionManagementService sessionService)
	{
		_sessionService = sessionService;
	}

	[HttpPost("start")]
	public async Task<IActionResult> StartSession([FromRoute] string chatRoomId)
	{
		try
		{
			// Retrieve the user ID from the token
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
				return Unauthorized();

			// Start a session using the service
			var response = await _sessionService.StartSession(chatRoomId, userId);

			return Ok(response);
		}
		catch (UnauthorizedAccessException)
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
	public async Task<IActionResult> EndSession([FromRoute] string chatRoomId)
	{
		try
		{
			// Retrieve the user ID from the token
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
				return Unauthorized();

			// End the session using the service
			var response = await _sessionService.EndSession(chatRoomId, userId);

			return Ok(response);
		}
		catch (UnauthorizedAccessException)
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
	[HttpGet]
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

			return Ok(session);
		}
		catch (UnauthorizedAccessException)
		{
			return Forbid();
		}
		catch (Exception ex)
		{
			return BadRequest(new { Error = ex.Message });
		}
	}

}
