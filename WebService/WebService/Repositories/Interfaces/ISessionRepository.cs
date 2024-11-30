using WebService.Models.Entities;

namespace WebService.Repositories.Interface;

public interface ISessionRepository
{
	Task<IEnumerable<SessionEntity>> GetAllSessionAsync();
	Task<SessionEntity> GetSessionByIdAsync(int id);
	Task AddSessionAsync(SessionEntity session);
	void UpdateSession(SessionEntity session);
	Task DeleteSessionAsync(int id);
	Task SaveChangesAsync();
}
