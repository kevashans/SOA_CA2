using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Services;

namespace WebService.Tests;

public class SessionManagementServiceTests
{
	private readonly Mock<ISessionRepository> _sessionRepository;
	private readonly Mock<IChatRoomRepository> _chatRoomRepository;
	private readonly SessionManagementService _sessionService;

	public SessionManagementServiceTests()
	{
		_sessionRepository = new Mock<ISessionRepository>();
		_chatRoomRepository = new Mock<IChatRoomRepository>();

		_sessionService = new SessionManagementService(
			_sessionRepository.Object,
			_chatRoomRepository.Object
		);
	}

	[Fact]
	public async Task StartSession_ShouldCreateNewSession_WhenNoActiveSessionExists()
	{
		// Arrange
		var chatRoomId = Guid.NewGuid();
		var userId = "user123";
		var chatRoom = new ChatRoom(chatRoomId, userId, "General Chat", "Casual");
		var previousSession = new Session(chatRoomId, DateTime.UtcNow.AddHours(-1), DateTime.UtcNow, "Previous Context");

		_chatRoomRepository
			.Setup(repo => repo.GetChatRoomByIdAsync(chatRoomId))
			.ReturnsAsync(chatRoom);

		_sessionRepository
			.Setup(repo => repo.GetActiveSessionAsync(chatRoomId))
			.ReturnsAsync((Session)null);

		_sessionRepository
			.Setup(repo => repo.GetMostRecentSessionAsync(chatRoomId))
			.ReturnsAsync(previousSession);

		// Act
		var session = await _sessionService.StartSession(chatRoomId.ToString(), userId);

		// Assert
		_sessionRepository.Verify(repo => repo.AddSessionAsync(It.IsAny<Session>()), Times.Once);
		_sessionRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);

		Assert.NotNull(session);
		Assert.Equal("Previous Context", session.Context);
		Assert.True(session.StartTime > DateTime.MinValue);
	}

	[Fact]
	public async Task StartSession_ShouldReturnActiveSession_WhenActiveSessionExists()
	{
		// Arrange
		var chatRoomId = Guid.NewGuid();
		var userId = "user123";
		var activeSession = new Session(chatRoomId, DateTime.UtcNow, null, "Active Context");
		var chatRoom = new ChatRoom(chatRoomId, userId, "General Chat", "Casual");

		_chatRoomRepository
			.Setup(repo => repo.GetChatRoomByIdAsync(chatRoomId))
			.ReturnsAsync(chatRoom);

		_sessionRepository
			.Setup(repo => repo.GetActiveSessionAsync(chatRoomId))
			.ReturnsAsync(activeSession);

		// Act
		var session = await _sessionService.StartSession(chatRoomId.ToString(), userId);

		// Assert
		_sessionRepository.Verify(repo => repo.AddSessionAsync(It.IsAny<Session>()), Times.Never);
		Assert.NotNull(session);
		Assert.True(session.StartTime > DateTime.MinValue);
		Assert.Equal("Active Context", session.Context);
	}

	[Fact]
	public async Task EndSession_ShouldMarkSessionAsEnded()
	{
		var chatRoomId = Guid.NewGuid();
		var userId = "user123";
		var session = new Session(chatRoomId, DateTime.UtcNow.AddHours(-1), null, "Session Context");

		_sessionRepository
			.Setup(repo => repo.GetActiveSessionAsync(chatRoomId))
			.ReturnsAsync(session);

		_chatRoomRepository
			.Setup(repo => repo.GetChatRoomByIdAsync(chatRoomId))
			.ReturnsAsync(new ChatRoom(chatRoomId, userId, "General Chat", "Casual"));

		await _sessionService.EndSession(chatRoomId.ToString(), userId);

		_sessionRepository.Verify(repo => repo.UpdateSession(It.Is<Session>(s => s.EndTime != null)), Times.Once);
		Assert.NotNull(session.EndTime);
	}

	[Fact]
	public async Task UpdateSessionSummary_ShouldUpdateSessionContext()
	{
		var sessionId = Guid.NewGuid();
		var updatedSummary = "Updated Summary";
		var session = new Session(Guid.NewGuid(), DateTime.UtcNow.AddHours(-1), null, "Old Summary");

		_sessionRepository
			.Setup(repo => repo.GetSessionByIdAsync(sessionId))
			.ReturnsAsync(session);

		await _sessionService.UpdateSessionSummary(sessionId, updatedSummary);
		_sessionRepository.Verify(repo => repo.UpdateSession(It.Is<Session>(s => s.Context == updatedSummary)), Times.Once);
		Assert.Equal(updatedSummary, session.Context);
	}

	[Fact]
	public async Task GetSession_ShouldReturnActiveSession_WhenActiveSessionExists()
	{
		// Arrange
		var chatRoomId = Guid.NewGuid();
		var userId = "user123";
		var activeSession = new Session(chatRoomId, DateTime.UtcNow, null, "Active Context");
		var chatRoom = new ChatRoom(chatRoomId, userId, "General Chat", "Casual");

		_chatRoomRepository
			.Setup(repo => repo.GetChatRoomByIdAsync(chatRoomId))
			.ReturnsAsync(chatRoom);

		_sessionRepository
			.Setup(repo => repo.GetActiveSessionAsync(chatRoomId))
			.ReturnsAsync(activeSession);
		var session = await _sessionService.GetSession(chatRoomId.ToString(), userId);

		Assert.NotNull(session);
		Assert.Equal(activeSession, session);
	}

	[Fact]
	public async Task GetSession_ShouldReturnMostRecentSession_WhenNoActiveSessionExists()
	{
		var chatRoomId = Guid.NewGuid();
		var userId = "user123";
		var recentSession = new Session(chatRoomId, DateTime.UtcNow.AddHours(-2), DateTime.UtcNow.AddHours(-1), "Recent Context");
		var chatRoom = new ChatRoom(chatRoomId, userId, "General Chat", "Casual");

		_chatRoomRepository
			.Setup(repo => repo.GetChatRoomByIdAsync(chatRoomId))
			.ReturnsAsync(chatRoom);

		_sessionRepository
			.Setup(repo => repo.GetActiveSessionAsync(chatRoomId))
			.ReturnsAsync((Session)null);

		_sessionRepository
			.Setup(repo => repo.GetMostRecentSessionAsync(chatRoomId))
			.ReturnsAsync(recentSession);

		var session = await _sessionService.GetSession(chatRoomId.ToString(), userId);

		Assert.NotNull(session);
		Assert.Equal(recentSession, session);
	}
}