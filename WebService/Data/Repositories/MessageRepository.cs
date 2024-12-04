using Domain.Entities;
using Data.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;
public class MessageRepository : IMessageRepository
{
	private readonly ApplicationDbContext _context;

	public MessageRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task AddMessageAsync(Message message)
	{
		await _context.AddAsync(MapToDataEntity(message));
	}

	public async Task DeleteMessageAsync(Guid id)
	{
		var message = await _context.Messages.FindAsync(id);

		if (message != null)
			_context.Messages.Remove(message);
	}

	public async Task<Message?> GetMessageByIdAsync(Guid id)
	{
		var message = await _context.Messages.FirstOrDefaultAsync(m => m.MessageId == id);
		return message is not null ? MapToDomainEntity(message) : null;
	}

	public async Task<IEnumerable<Message>> GetLastMessagesAsync(Guid chatRoomId, int count)
	{
		var messages = await _context.Messages
			.Where(m => m.ChatRoomId == chatRoomId)
			.OrderByDescending(m => m.CreatedAt)
			.Take(count)
			.ToListAsync();

		return messages.Select(MapToDomainEntity);
	}

	public async Task<IEnumerable<Message?>> GetMessagesByChatroomId(Guid id)
	{
		var messages = await _context.Messages
			.Where(cr => cr.ChatRoomId == id)
			.ToListAsync();

		return messages.Select(MapToDomainEntity);
	}

	public async Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync();
	}

	public async Task UpdateMessage(Message message)
	{
		var trackedEntity = _context.Messages.Local.FirstOrDefault(cr => cr.MessageId == message.MessageId);

		if (trackedEntity != null)
		{
			// update tracked entity
			trackedEntity.Content = message.Content;
		}
		else
		{
			// attach entity for persistence
			var entity = MapToDataEntity(message);
			_context.Messages.Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
		}

		await _context.SaveChangesAsync();
	}

	private MessageEntity MapToDataEntity(Message message)
	{
		return new MessageEntity(message.MessageId, message.ChatRoomId, message.MessageType, message.Content, message.CreatedAt);
	}

	private Message MapToDomainEntity(MessageEntity message)
	{
		return new Message(message.MessageId, message.ChatRoomId, message.MessageType, message.Content, message.CreatedAt);
	}
}
