using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
	private readonly IMessageService _messageService;

	public MessagesController(IMessageService messageService)
	{
		_messageService = messageService;
	}

	/// <summary>
	/// Creates a new chat room
	/// </summary>
	[HttpPost("{chatRoomId}"), Authorize]
	public async Task<IActionResult> AddMessage([FromRoute] string chatRoomId, string prompt)
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
				return Unauthorized();

			var message = await _messageService.AddPrompt(chatRoomId, userId, prompt);

			return Ok(new { Message = message.Content });
		}
		catch (Exception ex)
		{
			return BadRequest(new { Error = ex.Message });
		}
	}
}
