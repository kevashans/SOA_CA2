using WebService.Models.Entities;
using WebService.Repositories.Interface;

namespace WebService.Repositories;

public class SessionRepository : ISessionRepository
{
	public Task AddAsync(SessionEntity session)
	{
		throw new NotImplementedException();
	}

	public Task DeleteSessionAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<SessionEntity>> GetAllAsync()
	{
		throw new NotImplementedException();
	}

	public Task<SessionEntity> GetByIdAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task SaveChangesAsync()
	{
		throw new NotImplementedException();
	}

	public Task UpdateAsync(SessionEntity session)
	{
		throw new NotImplementedException();
	}
}
