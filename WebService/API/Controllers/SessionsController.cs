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
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
				return Unauthorized();

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

	[HttpPost("end")]
	public async Task<IActionResult> EndSession([FromRoute] string chatRoomId)
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
				return Unauthorized();

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

	[HttpGet]
	public async Task<IActionResult> GetSession([FromRoute] string chatRoomId)
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrWhiteSpace(userId))
				return Unauthorized();

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
