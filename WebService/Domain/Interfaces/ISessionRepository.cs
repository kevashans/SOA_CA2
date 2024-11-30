using Domain.Entities;

namespace Domain.Interfaces;

public interface ISessionRepository
{
	Task<IEnumerable<Session>> GetAllSessionAsync();
	Task<Session> GetSessionByIdAsync(int id);
	Task AddSessionAsync(Session session);
	void UpdateSession(Session session);
	Task DeleteSessionAsync(int id);
	Task SaveChangesAsync();
}
