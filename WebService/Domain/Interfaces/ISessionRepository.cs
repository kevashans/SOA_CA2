using Domain.Entities;

namespace Domain.Interfaces;

public interface ISessionRepository
{
	Task<IEnumerable<Session>> GetAllSessionAsync();

	Task<Session?> GetSessionByIdAsync(Guid id);

	Task AddSessionAsync(Session session);

	Task UpdateSession(Session session);

	Task DeleteSessionAsync(int id);

	Task SaveChangesAsync();

	Task<Session?> GetMostRecentSessionAsync(Guid chatRoomId);

	Task<Session?> GetActiveSessionAsync(Guid chatRoomId);

}
