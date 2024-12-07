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


	public async Task<Session?> GetActiveSessionAsync(Guid chatRoomId)
	{
		var session = await _context.Sessions
		.FirstOrDefaultAsync(s => s.ChatRoomId == chatRoomId && s.EndTime == null);

		return session is not null ? MapToDomainEntity(session) : null;
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
		var trackedEntity = _context.Sessions.Local.FirstOrDefault(s => s.SessionId == session.SessionId);

		if (trackedEntity != null)
		{
			trackedEntity.Context = session.Context;
			trackedEntity.EndTime = session.EndTime;
		}

		await _context.SaveChangesAsync();
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
