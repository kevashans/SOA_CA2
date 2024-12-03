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

		if (chatRoom == null)
			throw new KeyNotFoundException($"ChatRoom with ID {chatRoomGuid} not found");

		chatRoom.ValidateOwnership(userId);

		var chatStrategy = _factory.GetChatStrategy(chatRoom.ChatRoomType);

		var response = await chatStrategy.Respond(prompt);

		var (input, output) = chatRoom.AddMessage(prompt, response);

		await _messageRepository.AddMessageAsync(input);
		await _messageRepository.AddMessageAsync(output);
		await _messageRepository.SaveChangesAsync();

		return output;
	}

	public async Task<IEnumerable<Message?>> GetChatroomMessages(string chatRoomId, string userId)
	{
		if (!Guid.TryParse(chatRoomId, out Guid chatRoomGuid))
			throw new ArgumentException("Invalid ChatRoom ID format.");

		var chatRoom = await _chatroomRepository.GetChatRoomByIdAsync(chatRoomGuid);

		if (chatRoom == null)
			throw new KeyNotFoundException($"ChatRoom with ID {chatRoomGuid} not found");

		chatRoom.ValidateOwnership(userId);

		return await _messageRepository.GetMessagesByChatroomId(chatRoomGuid);
	}

}
