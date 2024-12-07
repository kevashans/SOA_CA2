using Domain.Entities;
using static Domain.DTOs.SessionDto;

namespace Services.Interfaces;

public interface ISessionManagementService
{
	Task<StartSessionResponse> StartSession(string chatRoomId, string userId);

	Task<EndSessionResponse> EndSession(string chatRoomId, string userId);

	Task<Session?> GetSession(string chatRoomId, string userId);

	Task UpdateSessionSummary(Guid sessionId, string updatedSummary);
}
