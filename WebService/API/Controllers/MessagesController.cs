using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;
using static Domain.DTOs.MessageDTOs;

namespace API.Controllers;

[ApiController]
[Route("api/ChatRooms/{chatRoomId}/Messages")]
public class MessagesController : ControllerBase
{
	private readonly IMessageService _messageService;

	public MessagesController(IMessageService messageService)
	{
		_messageService = messageService;
	}

	[HttpPost, Authorize]
	public async Task<IActionResult> AddMessage([FromRoute] string chatRoomId, CreateMessageRequests request)
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var message = await _messageService.AddPrompt(chatRoomId, userId, request);

			return Ok(new { Message = message.Content });
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

	[HttpGet, Authorize]
	public async Task<IActionResult> GetMessages([FromRoute] string chatRoomId)
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var messages = await _messageService.GetChatroomMessages(chatRoomId, userId);

			return Ok(new { Message = messages });
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

	[HttpPut("{messageId}"), Authorize]
	public async Task<IActionResult> EditMessage([FromRoute] string chatRoomId, [FromRoute] string messageId, EditMessageRequest request)
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var updatedMessage = await _messageService.EditMessage(chatRoomId, messageId, userId, request);

			return Ok(new { Message = updatedMessage.Content });
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

	[HttpDelete("{messageId}"), Authorize]
	public async Task<IActionResult> DeleteMessage([FromRoute] string chatRoomId, [FromRoute] string messageId)
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();


			await _messageService.DeleteMessage(messageId, userId, chatRoomId);

			return Ok(new { Message = $"Message with ID {messageId} has been deleted" });
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
