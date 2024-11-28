using Microsoft.AspNetCore.Mvc;
using WebService.DTOs;
using WebService.Repositories.Interface;
using WebService.Repositories;

namespace WebService.Controllers;

public class MessageController
{
	private readonly IMessageRepository _context;
	public MessageController(MessageRepository context)
	{
		_context = context;
	}

	[HttpPost("{chatRoomId}/messages")]
	public IActionResult AddMessage(Guid chatRoomId, MessageDto dto)
	{
		throw new NotImplementedException("AddMessage method is not implemented.");
		//var chat strategy = ChatStrategyFactory.GetChatStrategy()
		//var content =  strategy.respond()
		// _context.AddMessageAsync(content, chatroom, ....);
	}

	/// <summary>
	/// Retrieves messages for a specific chat room
	/// </summary>
	[HttpGet("{id}/messages")]
	public IActionResult GetMessages(Guid id)
	{
		throw new NotImplementedException("GetMessages method is not implemented.");
	}
}
