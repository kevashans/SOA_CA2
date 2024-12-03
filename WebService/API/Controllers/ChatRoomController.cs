using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;

namespace API.Controllers;

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
	[HttpPut("update/{chatRoomId}"), Authorize]
	public async Task<IActionResult> UpdateChatRoom(string chatRoomId, UpdateChatRoomRequest dto)
	{
		string? userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		if (userId == null)
			return Unauthorized();

		try
		{
			var updatedChatRoom = await _chatRoomService.UpdateChatRoom(chatRoomId, dto, userId);

			return Ok(new { Message = $"Chatroom {chatRoomId} updated.", ChatRoom = updatedChatRoom });
		}
		catch (Exception ex)
		{
			return BadRequest(new { Error = ex.Message });
		}
	}


	/// <summary>
	/// Retrieves chat rooms by a specific user
	/// </summary>
	[HttpGet("{userId}")]
	public IActionResult GetChatRoomByUserId(string userId)
	{
		throw new NotImplementedException("GetChatRoomByUserId method is not implemented.");
	}
}
