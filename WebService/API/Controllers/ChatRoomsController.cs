using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatRoomsController : ControllerBase
{
	private readonly IChatRoomService _chatRoomService;

	public ChatRoomsController(IChatRoomService chatRoomService)
	{
		_chatRoomService = chatRoomService;
	}

	[HttpPost, Authorize]
	public async Task<IActionResult> CreateChatRoom(CreateChatRoomRequest dto)
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var createdChatRoom = await _chatRoomService.CreateChatRoom(dto, userId);

			return Ok(createdChatRoom);
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

	[HttpPut("{chatRoomId}"), Authorize]
	public async Task<IActionResult> UpdateChatRoom([FromRoute] string chatRoomId, UpdateChatRoomRequest dto)
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var updatedChatRoom = await _chatRoomService.UpdateChatRoom(chatRoomId, dto, userId);

			return Ok(updatedChatRoom);
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
	public async Task<IActionResult> GetChatRoomByUserId()
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			var chatRooms = await _chatRoomService.GetChatRoomByUserId(userId);

			var response = chatRooms.Select(chatRoom => new ChatRoomResponse
			{
				ChatRoomId = chatRoom.ChatRoomId,
				Name = chatRoom.Name,
				ChatRoomType = chatRoom.ChatRoomType,
				UserId = chatRoom.UserId
			});

			return Ok(new { ChatRooms = chatRooms });
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

	[HttpDelete("{chatRoomId}"), Authorize]
	public async Task<IActionResult> DeleteChatRoomById(string chatRoomId)
	{
		try
		{
			string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
			if (userId == null)
				return Unauthorized();

			await _chatRoomService.DeleteChatRoomById(chatRoomId, userId);

			return Ok(new { Message = $"ChatRoom with ID {chatRoomId} has been deleted" });
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
