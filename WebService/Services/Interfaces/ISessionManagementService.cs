using Domain.Entities;

namespace Services.Interfaces;

public interface ISessionManagementService
{
	Task<Session> StartSession(string chatRoomId, string userId);

	Task EndSession(string chatRoomId, string userId);

	Task<Session?> GetSession(string chatRoomId, string userId);

	Task UpdateSessionSummary(Guid sessionId, string updatedSummary);
}
