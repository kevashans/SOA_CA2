using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;
using static Domain.DTOs.MessageDTOs;

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
	public async Task<IActionResult> AddMessage([FromRoute] string chatRoomId, CreateMessageRequests request)
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var message = await _messageService.AddPrompt(chatRoomId, userId, request.Content);

			return Ok(new { Message = message.Content });
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

	[HttpGet("{chatRoomId}"), Authorize]
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
		catch (UnauthorizedAccessException ex)
		{
			return Forbid();
		}
		catch (Exception ex)
		{
			return BadRequest(new { Error = ex.Message });
		}
	}

	[HttpPut("{messageId}"), Authorize]
	public async Task<IActionResult> EditMessage([FromRoute] string messageId, EditMessageRequest request)
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var updatedMessage = await _messageService.EditMessage(messageId, userId, request);

			return Ok(new { Message = updatedMessage.Content });
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

	[HttpDelete("{messageId}"), Authorize]
	public async Task<IActionResult> DeleteMessage([FromRoute] string messageId, DeleteMessageRequest request)
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			await _messageService.DeleteMessage(messageId, userId ,request);

			return Ok(new { Message = $"Message with ID {messageId} has been deleted" });
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
