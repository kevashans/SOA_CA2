using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatRoomController : ControllerBase
{
	private readonly IChatRoomService _chatRoomService;
	public ChatRoomController(IChatRoomService chatRoomService)
	{
		_chatRoomService = chatRoomService;
	}

	/// <summary>
	/// Creates a new chat room
	/// </summary>
	[HttpPost("create"), Authorize]
	public async Task<IActionResult> CreateChatRoom(CreateChatRoomRequest dto)
	{
		string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		if (userId == null)
			return Unauthorized();

		try
		{
			await _chatRoomService.CreateChatRoom(dto, userId);

			return Ok(new { Message = $"Chatroom {dto.Name} created." });
		}
		catch (Exception ex)
		{
			return BadRequest(new { Error = ex.Message });
		}
	}

	/// <summary>
	/// Updates an existing chat room
	/// </summary>
	[HttpPut("update"), Authorize]
	public async Task<IActionResult> UpdateChatRoom(UpdateChatRoomRequest dto)
	{
		string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		if (userId == null)
			return Unauthorized();

		try
		{
			var updatedChatRoom = await _chatRoomService.UpdateChatRoom(dto, userId);

			return Ok(new { Message = $"Chatroom {dto.ChatRoomId} updated.", ChatRoom = updatedChatRoom });
		}
		catch (Exception ex)
		{
			return BadRequest(new { Error = ex.Message });
		}
	}


	/// <summary>
	/// Retrieves chat rooms which is created by a specific user
	/// </summary>
	[HttpGet("me"), Authorize]
	public async Task<IActionResult> GetChatRoomByUserId()
	{
		string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		if (userId == null)
			return Unauthorized();

		try
		{
			var updatedChatRoom = await _chatRoomService.GetChatRoomByUserId(userId);

			return Ok(new { ChatRooms = updatedChatRoom });
		}
		catch (Exception ex)
		{
			return BadRequest(new { Error = ex.Message });
		}
	}

	[HttpDelete("{id}"), Authorize]
	public async Task<IActionResult> DeleteChatRoomById(string id)
	{
		string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		if (userId == null)
			return Unauthorized();

		try
		{
			await _chatRoomService.DeleteChatRoomById(id, userId);

			return Ok(new { Message = $"ChatRoom with ID {id} has been deleted" });
		}
		catch (Exception ex)
		{
			return BadRequest(new { Error = ex.Message });
		}
	}
}
