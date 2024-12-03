using Domain.Entities;
using Domain.Factories.Interfaces;
using Domain.Interfaces;
using Services.Interfaces;

namespace Services;

public class MessageService : IMessageService
{
	private readonly IChatStrategyFactory _factory;
	private readonly IMessageRepository _messageRepository;
	private readonly IChatRoomRepository _chatroomRepository;


	public MessageService(IChatStrategyFactory factory, IMessageRepository messageRepository, IChatRoomRepository chatroomRepository)
	{
		_factory = factory;
		_chatroomRepository = chatroomRepository;
		_messageRepository = messageRepository;
	}

	public async Task<Message> AddPrompt(string chatRoomId, string userId, string prompt)
	{
		if (!Guid.TryParse(chatRoomId, out Guid chatRoomGuid))
			throw new ArgumentException("Invalid ChatRoom ID format.");

		var chatRoom = await _chatroomRepository.GetChatRoomByIdAsync(chatRoomGuid);

		if(chatRoom == null)
			throw new KeyNotFoundException($"ChatRoom with ID {chatRoomGuid} not found");

		if (chatRoom.UserId != userId)
			throw new UnauthorizedAccessException("You are not authorized to modify this chat room.");


		throw new NotImplementedException("Not implemented");
	}
}
