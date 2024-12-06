using Domain.DTOs;
using Domain.Entities;
using Domain.Factories.Interfaces;
using Domain.Interfaces;
using Domain.Strategies.Interfaces;
using Services.Interfaces;
using System.Text;
using static Domain.DTOs.MessageDTOs;

namespace Services;

/// <summary>
/// Service that deals with messages related functionality
/// </summary>
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

	/// <summary>
	/// add a user prompt to a chatroom and generate response using the right strategy with respect to session summary
	/// </summary>
	/// <param name="chatRoomId">the identifier of the room where message is being generated</param>
	/// <param name="userId">The identidier of the user sending the message</param>
	/// <param name="prompt">The message send by the user</param>
	/// <returns>The generated response</returns>
	/// <exception cref="KeyNotFoundException"></exception>
	public async Task<CreateMessageResponse> AddPrompt(string chatRoomId, string userId, CreateMessageRequests prompt)
	{
		// Validate ChatRoom
		var chatRoomGuid = GetGuid(chatRoomId);
		var chatRoom = await _chatroomRepository.GetChatRoomByIdAsync(chatRoomGuid) ?? throw new KeyNotFoundException($"ChatRoom with ID {chatRoomGuid} not found");
		chatRoom.ValidateOwnership(userId);

		// Get active Session or most recent
		var session = await _sessionManagementService.GetSession(chatRoomId, userId);

		// Create chatstrategy and response
		var chatStrategy = _factory.GetChatStrategy(chatRoom.ChatRoomType);

		// Provide context using session summary
		chatStrategy.ProvideContext(session?.Context);

		var response = await chatStrategy.Respond(prompt.Content);
		var (input, output) = chatRoom.AddMessage(prompt.Content, response);

		await _messageRepository.AddMessageAsync(input);
		await _messageRepository.AddMessageAsync(output);
		await _messageRepository.SaveChangesAsync();

		// Update session summary
		var lastMessages = await _messageRepository.GetLastMessagesAsync(chatRoomGuid, 10);
		var updatedSummary = await GenerateSummaryFromMessages(chatStrategy, lastMessages, session?.Context);

		if (session != null)
			await _sessionManagementService.UpdateSessionSummary(session.SessionId, updatedSummary);

		return new CreateMessageResponse
		{
			MessageId = output.MessageId,
			ChatRoomId = output.ChatRoomId,
			Content = output.Content,
			MessageType = output.MessageType,
			CreatedAt = output.CreatedAt
		};
	}

	/// <summary>
	/// Retrieves all message inside a specific chatroom
	/// </summary>
	/// <param name="chatRoomId">identifier of the chatroom whosemessages are to be received</param>
	/// <param name="userId">the identifier of the user requesting the message</param>
	/// <returns>list of messages inside the chatroom</returns>
	/// <exception cref="UnauthorizedAccessException">Thrown if user is not the owner of the chatroom.</exception>
	public async Task<IEnumerable<MessageResponse?>> GetChatroomMessages(string chatRoomId, string userId)
	{
		// Valdiate ChatRoom
		var chatRoomGuid = GetGuid(chatRoomId);
		await ValidateChatRoom(chatRoomGuid, userId);

		var messages = await _messageRepository.GetMessagesByChatroomId(chatRoomGuid);

		return messages.Select(message => new MessageResponse
		{
			MessageId = message.MessageId,
			ChatRoomId = message.ChatRoomId,
			Content = message.Content,
			MessageType = message.MessageType,
			CreatedAt = message.CreatedAt
		});
	}

	/// <summary>
	/// edits the content of an existing message
	/// </summary>
	/// <param name="messageId">identifier of the message to be edited</param>
	/// <param name="userId">Identifier of the user attempting to edit the message</param>
	/// <param name="request">Request containing the updated content of the message</param>
	/// <returns>Updated version of the message</returns>
	/// <exception cref="KeyNotFoundException">If the message with the specific identifier is not found</exception>
	public async Task<EditMessageResponse> EditMessage(string chatRoomId, string messageId, string userId, EditMessageRequest request)
	{
		// Validate ChatRoom
		var chatRoomGuid = GetGuid(chatRoomId);
		await ValidateChatRoom(chatRoomGuid, userId);

		// Validate Message
		var messageGuid = GetGuid(messageId);
		var existingMessage = await _messageRepository.GetMessageByIdAsync(messageGuid) ?? throw new KeyNotFoundException($"message with ID {messageId} not found");

		existingMessage.UpdateContent(request.NewContent);
		await _messageRepository.UpdateMessage(existingMessage);

		return new EditMessageResponse
		{
			MessageId = existingMessage.MessageId,
			ChatRoomId = existingMessage.ChatRoomId,
			Content = existingMessage.Content,
		};
	}

	/// <summary>
	/// Deletes message from a chatroom
	/// </summary>
	/// <param name="messageId">unique identifier of the message to be deleted</param>
	/// <param name="userId">unique identifier of the user attempting to delete the message</param>
	/// <param name="request">request containing the chatroom to be deleted</param>
	/// <exception cref="KeyNotFoundException">If chatroom with the specific identifier is not found</exception>
	public async Task DeleteMessage(string messageId, string userId, string chatRoomId)
	{
		// Validate ChatRoom
		var chatRoomGuid = GetGuid(chatRoomId);
		await ValidateChatRoom(chatRoomGuid, userId);

		// Validate Message
		var messageGuid = GetGuid(messageId);
		var message = await _messageRepository.GetMessageByIdAsync(messageGuid) ?? throw new KeyNotFoundException($"message with ID {messageId} not found");

		await _messageRepository.DeleteMessageAsync(messageGuid);
		await _messageRepository.SaveChangesAsync();
	}

	/// <summary>
	/// Generates a summary from the collection of messages
	/// </summary>
	/// <param name="strategy">the chat strategy used to create the summary</param>
	/// <param name="messages">the collection of messages that is to be summarized</param>
	/// <param name="previousSummary">the previous session summary</param>
	/// <returns>the summarized messages</returns>
	private async Task<string> GenerateSummaryFromMessages(IChatTypeStrategy strategy, IEnumerable<Message> messages, string? previousSummary)
	{
		var summaryBuilder = new StringBuilder($"Previous Summary: {previousSummary} ,Session Summary:\n");
		foreach (var message in messages)
		{
			summaryBuilder.AppendLine($"{message.CreatedAt}: {message.Content}");
		}

		var summary = await strategy.Respond("Create a Summary from this chat history: " + summaryBuilder.ToString());
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
