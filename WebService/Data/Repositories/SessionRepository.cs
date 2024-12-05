using Data.Entities;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class SessionRepository : ISessionRepository
{
	private readonly ApplicationDbContext _context;

	public SessionRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task AddSessionAsync(Session session)
	{
		await _context.AddAsync(MapToDataEntity(session));
	}

	public Task DeleteSessionAsync(int id)
	{
		throw new NotImplementedException();
	}

	public async Task<Session?> GetActiveSessionAsync(Guid chatRoomId)
	{
		var session = await _context.Sessions
		.FirstOrDefaultAsync(s => s.ChatRoomId == chatRoomId && s.EndTime == null);

		return session is not null ? MapToDomainEntity(session) : null;
	}

	public Task<IEnumerable<Session>> GetAllSessionAsync()
	{
		throw new NotImplementedException();
	}

	public async Task<Session?> GetMostRecentSessionAsync(Guid chatRoomId)
	{
		var session = await _context.Sessions
			.Where(s => s.ChatRoomId == chatRoomId && s.EndTime != null)
			.OrderByDescending(s => s.EndTime)
			.FirstOrDefaultAsync();

		return session is not null ? MapToDomainEntity(session) : null;
	}

	public async Task<Session?> GetSessionByIdAsync(Guid id)
	{
		var message = await _context.Sessions.FirstOrDefaultAsync(s => s.SessionId == id);
		return message is not null ? MapToDomainEntity(message) : null;
	}

	public async Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync();
	}

	public async Task UpdateSession(Session session)
	{
		try
		{
			var trackedEntity = _context.Sessions.Local.FirstOrDefault(s => s.SessionId == session.SessionId);

			if (trackedEntity != null)
			{
				// Update tracked entity
				trackedEntity.Context = session.Context;
				trackedEntity.EndTime = session.EndTime;
			}
			else
			{
				// Attach entity for persistence
				var entity = MapToDataEntity(session);
				_context.Sessions.Attach(entity);
				_context.Entry(entity).State = EntityState.Modified;
			}

			await _context.SaveChangesAsync();
		}
		catch (DbUpdateException ex)
		{
			// Log or inspect the inner exception
			throw new Exception("An error occurred while updating the session. See inner exception for details.", ex);
		}
	}

	private SessionEntity MapToDataEntity(Session session)
	{
		return new SessionEntity(session.SessionId, session.ChatRoomId, session.StartTime, session.EndTime, session.Context);
	}

	private Session MapToDomainEntity(SessionEntity session)
	{
		return new Session(session.SessionId, session.ChatRoomId, session.StartTime, session.EndTime, session.Context);
	}
}
