using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace API.Controllers;

public class ChatRoomController : ControllerBase
{
	private readonly ChatRoomService _chatRoomService;
	public ChatRoomController(ChatRoomService chatRoomService)
	{
		_chatRoomService = chatRoomService;
	}

	/// <summary>
	/// Creates a new chat room
	/// </summary>
	[HttpPost]
	public IActionResult CreateChatRoom(ChatRoomDto dto)
	{
		throw new NotImplementedException();
		// var chatroom = ChatRoomFactory.CreateChatRoom()
		//_context.AddChatRoom(chatroom)
	}

	/// <summary>
	/// Retrieves all chat rooms
	/// </summary>
	[HttpGet]
	public IActionResult GetAllChatRooms()
	{
		throw new NotImplementedException("GetAllChatRooms method is not implemented.");
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
