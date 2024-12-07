using Domain.Factories.Interfaces;
using Domain.Interfaces;
using Moq;
using Services.Interfaces;
using Services;
using Domain.Entities;
using Domain.Strategies.Interfaces;
using static Domain.DTOs.MessageDTOs;

namespace WebService.Tests;

public class MessageServiceTests
{
	private readonly Mock<IMessageRepository> _messageRepositoryMock;
	private readonly Mock<IChatRoomRepository> _chatRoomRepositoryMock;
	private readonly Mock<IChatStrategyFactory> _chatStrategyFactoryMock;
	private readonly Mock<ISessionManagementService> _sessionManagementServiceMock;
	private readonly MessageService _messageService;


	public MessageServiceTests()
	{
		_messageRepositoryMock = new Mock<IMessageRepository>();
		_chatRoomRepositoryMock = new Mock<IChatRoomRepository>();
		_chatStrategyFactoryMock = new Mock<IChatStrategyFactory>();
		_sessionManagementServiceMock = new Mock<ISessionManagementService>();
		_messageService = new MessageService(
			_chatStrategyFactoryMock.Object,
			_messageRepositoryMock.Object,
			_chatRoomRepositoryMock.Object,
			_sessionManagementServiceMock.Object
		);
	}

	[Fact]
	public async Task AddPrompt_ShouldCreateMessage()
	{
		var chatRoomId = Guid.NewGuid().ToString();
		var userId = "user123";
		var prompt = new CreateMessageRequests { Content = "Prompt " };

		var chatRoom = new ChatRoom(Guid.NewGuid(), userId, "Test Chat", "General");
		var strategy = new Mock<IChatTypeStrategy>();

		strategy.Setup(s => s.Respond(It.IsAny<string>())).ReturnsAsync("Response Content");

		_chatRoomRepositoryMock
			.Setup(r => r.GetChatRoomByIdAsync(It.IsAny<Guid>()))
			.ReturnsAsync(chatRoom);

		_chatStrategyFactoryMock
			.Setup(f => f.GetChatStrategy(It.IsAny<string>()))
			.Returns(strategy.Object);

		var result = await _messageService.AddPrompt(chatRoomId, userId, prompt);

		Assert.NotNull(result);
		Assert.Equal("Response Content", result.Content);
		_messageRepositoryMock.Verify(r => r.AddMessageAsync(It.IsAny<Message>()), Times.Exactly(2));
		_messageRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
	}

	[Fact]
	public async Task GetChatroomMessages_ShouldReturnMessages()
	{
		var chatRoomId = Guid.NewGuid();
		var userId = "user123";
		var chatRoom = new ChatRoom(chatRoomId, userId, "General Chat", "Casual");
		var messages = new List<Message>
		{
			new Message(chatRoomId, "Input", "Hello", DateTime.UtcNow),
			new Message(chatRoomId, "Output", "Hi there!", DateTime.UtcNow)
		};

		_chatRoomRepositoryMock
			.Setup(repo => repo.GetChatRoomByIdAsync(chatRoomId))
			.ReturnsAsync(chatRoom);

		_messageRepositoryMock
			.Setup(repo => repo.GetMessagesByChatroomId(chatRoomId))
			.ReturnsAsync(messages);


		var result = await _messageService.GetChatroomMessages(chatRoomId.ToString(), userId);

		Assert.NotNull(result);
		Assert.Equal(2, result.Count());
	}

	[Fact]
	public async Task EditMessage_ShouldUpdateMessageContent()
	{
		var messageId = Guid.NewGuid();
		var userId = "user123";
		var chatRoomId = Guid.NewGuid();
		var request = new EditMessageRequest
		{
			NewContent = "Updated Content"
		};
		var chatRoom = new ChatRoom(chatRoomId, userId, "General Chat", "Casual");
		var message = new Message(chatRoomId, "Input", "Old Content", DateTime.UtcNow);

		_chatRoomRepositoryMock
			.Setup(repo => repo.GetChatRoomByIdAsync(chatRoomId))
			.ReturnsAsync(chatRoom);

		_messageRepositoryMock
			.Setup(repo => repo.GetMessageByIdAsync(messageId))
			.ReturnsAsync(message);

		var result = await _messageService.EditMessage(chatRoomId.ToString() ,messageId.ToString(), userId, request);

		_messageRepositoryMock.Verify(repo => repo.UpdateMessage(It.Is<Message>(m => m.Content == "Updated Content")), Times.Once);
		Assert.NotNull(result);
		Assert.Equal("Updated Content", result.Content);
	}

	[Fact]
	public async Task DeleteMessage_ShouldDeleteMessageSuccessfully()
	{
		// Arrange
		var messageId = Guid.NewGuid();
		var userId = "user123";
		var chatRoomId = Guid.NewGuid();

		var chatRoom = new ChatRoom(chatRoomId, userId, "General Chat", "Casual");
		var message = new Message(chatRoomId, "Input", "Hello", DateTime.UtcNow);

		_chatRoomRepositoryMock
			.Setup(repo => repo.GetChatRoomByIdAsync(chatRoomId))
			.ReturnsAsync(chatRoom);

		_messageRepositoryMock
			.Setup(repo => repo.GetMessageByIdAsync(messageId))
			.ReturnsAsync(message);
		await _messageService.DeleteMessage(messageId.ToString(), userId, chatRoomId.ToString());

		_messageRepositoryMock.Verify(repo => repo.DeleteMessageAsync(messageId), Times.Once);
	}
}
