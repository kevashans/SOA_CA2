using Domain.Entities;
using Domain.Interfaces;
using Services.Interfaces;

namespace Services;
/// <summary>
/// Service that deals with session related functionality
/// </summary>
public class SessionManagementService : ISessionManagementService
{
	private readonly ISessionRepository _sessionRepository;
	private readonly IChatRoomRepository _chatRoomRepository;

	public SessionManagementService(ISessionRepository sessionRepository, IChatRoomRepository chatRoomRepository)
	{
		_sessionRepository = sessionRepository;
		_chatRoomRepository = chatRoomRepository;
	}

	/// <summary>
	/// Start a new session for the specified chatroom
	/// if a sesion already exists it returns the existing one instead of creating a new one
	/// </summary>
	/// <param name="chatRoomId">The identifier of the chatroom whhere the session is being started</param>
	/// <param name="userId">The identifier of the user starting the session</param>
	/// <returns>the newly created session or the laready active session</returns>
	/// <exception cref="KeyNotFoundException">if the chatroom with the unique identifier is not found</exception>
	public async Task<Session> StartSession(string chatRoomId, string userId)
	{
		var chatRoomGuid = GetGuid(chatRoomId);

		// Validate ChatRoom
		var chatRoom = await _chatRoomRepository.GetChatRoomByIdAsync(chatRoomGuid)
			?? throw new KeyNotFoundException($"ChatRoom with ID {chatRoomGuid} not found");

		chatRoom.ValidateOwnership(userId);

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

	/// <summary>
	/// Ends the active session for a specific chatroom
	/// if no active session exists the method does no adjustment
	/// </summary>
	/// <param name="chatRoomId">the unique identifier of the chatroom where the session is supposed to be ended</param>
	/// <param name="userId">the unique identifier of the user ending the session</param>
	/// <returns></returns>
	public async Task EndSession(string chatRoomId, string userId)
	{
		var session = await GetSession(chatRoomId, userId);
		if (session == null || session.EndTime is not null)
			return;

		session.End();

		await _sessionRepository.UpdateSession(session);
	}

	public async Task UpdateSessionSummary(Guid sessionId, string updatedSummary)
	{
		var session = await _sessionRepository.GetSessionByIdAsync(sessionId)
			?? throw new KeyNotFoundException($"Session with ID {sessionId} not found");

		session.UpdateSummary(updatedSummary);

		await _sessionRepository.UpdateSession(session);
	}

	/// <summary>
	/// Get Active Session if exists otherwise returns the most recent
	/// </summary>
	/// <param name="chatRoomId">The unique identifier of the chatroom we are getting the session from</param>
	/// <param name="userId">the unique identifier of the user requesting the session</param>
	/// <returns>active or most recent session in the chatroom</returns>
	/// <exception cref="ArgumentException">if chatroom id is not a valid GUID</exception>
	/// <exception cref="UnauthorizedAccessException">if user is not authorized to access the chat room</exception>
	public async Task<Session?> GetSession(string chatRoomId, string userId)
	{
		var chatRoomGuid = GetGuid(chatRoomId);

		// Validate that the user owns the chatroom
		var chatRoom = await _chatRoomRepository.GetChatRoomByIdAsync(chatRoomGuid)
			?? throw new KeyNotFoundException($"ChatRoom with ID {chatRoomGuid} not found");

		chatRoom.ValidateOwnership(userId);

		// Fetch the active session or most recent ended session
		var activeSession = await _sessionRepository.GetActiveSessionAsync(chatRoomGuid);
		if (activeSession != null)
			return activeSession;

		return await _sessionRepository.GetMostRecentSessionAsync(chatRoomGuid);
	}

	private Guid GetGuid(string id)
	{
		if (!Guid.TryParse(id, out Guid guid))
			throw new ArgumentException("Invalid ID format.");
		return guid;
	}
}
