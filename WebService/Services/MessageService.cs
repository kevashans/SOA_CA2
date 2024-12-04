using Domain.Entities;
using Domain.Factories.Interfaces;
using Domain.Interfaces;
using Domain.Strategies.Interfaces;
using Services.Interfaces;
using System.Text;
using static Domain.DTOs.MessageDTOs;

namespace Services;

public class MessageService : IMessageService
{
	private readonly IChatStrategyFactory _factory;

	private readonly IMessageRepository _messageRepository;
	private readonly IChatRoomRepository _chatroomRepository;

	private readonly ISessionManagementService _sessionManagementService;

	public MessageService(IChatStrategyFactory factory, IMessageRepository messageRepository, IChatRoomRepository chatroomRepository, ISessionManagementService sessionManagementService)
	{
		_factory = factory;
		_chatroomRepository = chatroomRepository;
		_messageRepository = messageRepository;
		_sessionManagementService = sessionManagementService;
	}

	public async Task<Message> AddPrompt(string chatRoomId, string userId, string prompt)
	{
		// Validate ChatRoom
		var chatRoomGuid = GetGuid(chatRoomId);
		var chatRoom = await _chatroomRepository.GetChatRoomByIdAsync(chatRoomGuid) ?? throw new KeyNotFoundException($"ChatRoom with ID {chatRoomGuid} not found");
		chatRoom.ValidateOwnership(userId);

		// Get active Session or most recent
		var session = await _sessionManagementService.GetSession(chatRoomId, userId);

		// Create chatstrategy and response
		var chatStrategy = _factory.GetChatStrategy(chatRoom.ChatRoomType);
		chatStrategy.ProvideContext(session?.Context);

		var response = await chatStrategy.Respond(prompt);
		var (input, output) = chatRoom.AddMessage(prompt, response);

		//if(session != null)
		//_sessionManagementService.UpdateSession(session.SessionId, userId, //last 10 messages summary);

		await _messageRepository.AddMessageAsync(input);
		await _messageRepository.AddMessageAsync(output);
		await _messageRepository.SaveChangesAsync();

		var lastMessages = await _messageRepository.GetLastMessagesAsync(chatRoomGuid, 10);
		var updatedSummary = await GenerateSummaryFromMessages(chatStrategy,lastMessages, session?.Context); 

		if (session != null)
		await _sessionManagementService.UpdateSessionSummary(session.SessionId, updatedSummary);

		return output;
	}

	public async Task<IEnumerable<Message?>> GetChatroomMessages(string chatRoomId, string userId)
	{
		// Valdiate ChatRoom
		var chatRoomGuid = GetGuid(chatRoomId);
		await ValidateChatRoom(chatRoomGuid, userId);

		return await _messageRepository.GetMessagesByChatroomId(chatRoomGuid);
	}

	public async Task<Message> EditMessage(string messageId, string userId, EditMessageRequest request)
	{
		// Validate ChatRoom
		var chatRoomGuid = GetGuid(request.ChatRoomId);
		await ValidateChatRoom(chatRoomGuid, userId);

		// Validate Message
		var messageGuid = GetGuid(messageId);
		var existingMessage = await _messageRepository.GetMessageByIdAsync(messageGuid) ?? throw new KeyNotFoundException($"message with ID {messageId} not found");
		existingMessage.Content = request.NewContent ?? existingMessage.Content;

		await _messageRepository.UpdateMessage(existingMessage);

		return existingMessage;
	}

	public async Task DeleteMessage(string messageId, string userId, DeleteMessageRequest request)
	{
		// Validate ChatRoom
		var chatRoomGuid = GetGuid(request.ChatRoomId);
		await ValidateChatRoom(chatRoomGuid, userId);

		// Validate Message
		var messageGuid = GetGuid(messageId);
		var message = await _messageRepository.GetMessageByIdAsync(messageGuid) ?? throw new KeyNotFoundException($"message with ID {messageId} not found");

		await _messageRepository.DeleteMessageAsync(messageGuid);
		await _messageRepository.SaveChangesAsync();
	}

	private async Task<string> GenerateSummaryFromMessages(IChatTypeStrategy strategy, IEnumerable<Message> messages, string? previousSummary)
	{
		var summaryBuilder = new StringBuilder($"Previous Summary: {previousSummary} ,Session Summary:\n");
		foreach (var message in messages)
		{
			summaryBuilder.AppendLine($"{message.CreatedAt}: {message.Content}");
		}

		var summary = await strategy.Respond("Create a Summary from this chat history " + summaryBuilder.ToString());
		return summary;
	}

	private Guid GetGuid(string id)
	{
		if (!Guid.TryParse(id, out Guid guid))
			throw new ArgumentException("Invalid ID format.");

		return guid;
	}

	private async Task ValidateChatRoom(Guid chatRoomGuid, string userId) 
	{
		var chatRoom = await _chatroomRepository.GetChatRoomByIdAsync(chatRoomGuid) ?? throw new KeyNotFoundException($"ChatRoom with ID {chatRoomGuid} not found");
		chatRoom.ValidateOwnership(userId);
	}
}
