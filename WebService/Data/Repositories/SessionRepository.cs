using Domain.Entities;
using Domain.Interfaces;

namespace Data.Repositories;

public class SessionRepository : ISessionRepository
{
	public Task AddSessionAsync(Session session)
	{
		throw new NotImplementedException();
	}

	public Task DeleteSessionAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<Session>> GetAllSessionAsync()
	{
		throw new NotImplementedException();
	}

	public Task<Session> GetSessionByIdAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task SaveChangesAsync()
	{
		throw new NotImplementedException();
	}

	public void UpdateSession(Session session)
	{
		throw new NotImplementedException();
	}
}
