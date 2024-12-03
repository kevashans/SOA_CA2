using Domain.Entities;
using Data.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Data.Auth;

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

	public Task DeleteMessageAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<Message>> GetAllMessageAsync()
	{
		throw new NotImplementedException();
	}

	public Task<Message?> GetMessageByIdAsync(string id)
	{
		throw new NotImplementedException();
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

	public void UpdateMessage(Message chatRoom)
	{
		throw new NotImplementedException();
	}

	private MessageEntity MapToDataEntity(Message message)
	{
		return new MessageEntity(message.MessageId, message.ChatRoomId, message.MessageType, message.Content, message.CreatedAt);
	}

	private Message MapToDomainEntity(MessageEntity message)
	{
		return new Message(message.ChatRoomId, message.MessageType, message.Content, message.CreatedAt);
	}

}
