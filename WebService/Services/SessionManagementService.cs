using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces;

namespace Services;

public class SessionManagementService : ISessionManagementService
{
	private readonly ISessionRepository _sessionRepository;
	private readonly IChatRoomRepository _chatRoomRepository;

	public SessionManagementService(ISessionRepository sessionRepository, IChatRoomRepository chatRoomRepository)
	{
		_sessionRepository = sessionRepository;
		_chatRoomRepository = chatRoomRepository;
	}

	public async Task<Session> StartSession(string chatRoomId, string userId)
	{
		if (!Guid.TryParse(chatRoomId, out Guid chatRoomGuid))
			throw new ArgumentException("Invalid ChatRoom ID format.");

		// Validate ChatRoom
		var chatRoom = await _chatRoomRepository.GetChatRoomByIdAsync(chatRoomGuid);
		if (chatRoom == null || chatRoom.UserId != userId)
			throw new UnauthorizedAccessException("You are not authorized to access this chat room.");

		// Get active session if available
		var activeSession = await _sessionRepository.GetActiveSessionAsync(chatRoomGuid);
		if (activeSession != null)
			return activeSession;

		// Get the most recent session for the chatroom
		var lastSession = await _sessionRepository.GetMostRecentSessionAsync(chatRoomGuid);

		// Create new session
		var newSession = new Session(chatRoomGuid, DateTime.UtcNow, null, lastSession?.Context ?? string.Empty);

		await _sessionRepository.AddSessionAsync(newSession);
		await _sessionRepository.SaveChangesAsync();

		return newSession;
	}

	public async Task EndSession(string chatRoomId, string userId)
	{
		if (!Guid.TryParse(chatRoomId, out Guid chatRoomGuid))
			throw new ArgumentException("Invalid  ID format.");

		var session = await GetSession(chatRoomId, userId);
		if (session == null || session.EndTime is not null)
			return;

		// Mark end time
		session.EndTime = DateTime.UtcNow;

		// generate summary for the session
		session.Context += "\n[Session Ended]";
		await _sessionRepository.UpdateSession(session);
	}

	public async Task UpdateSessionSummary(Guid sessionId, string updatedSummary)
	{
		var session = await _sessionRepository.GetSessionByIdAsync(sessionId)
			?? throw new KeyNotFoundException($"Session with ID {sessionId} not found");

		session.Context = updatedSummary;
		await _sessionRepository.UpdateSession(session);
	}

	/// <summary>
	/// Get Active Session if exists otherwise return most recent
	/// </summary>
	/// <param name="chatRoomId"></param>
	/// <param name="userId"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	/// <exception cref="UnauthorizedAccessException"></exception>
	public async Task<Session?> GetSession(string chatRoomId, string userId)
	{
		if (!Guid.TryParse(chatRoomId, out Guid chatRoomGuid))
			throw new ArgumentException("Invalid ChatRoom ID format.");

		// Validate that the user owns the chatroom
		var chatRoom = await _chatRoomRepository.GetChatRoomByIdAsync(chatRoomGuid);
		if (chatRoom == null || chatRoom.UserId != userId)
			throw new UnauthorizedAccessException("You are not authorized to access this chat room.");

		// Fetch the active session or most recent ended session
		var activeSession = await _sessionRepository.GetActiveSessionAsync(chatRoomGuid);
		if (activeSession != null)
			return activeSession;

		return await _sessionRepository.GetMostRecentSessionAsync(chatRoomGuid);
	}
}
