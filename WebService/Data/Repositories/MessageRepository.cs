using Domain.Entities;
using Data.Entities;
using Domain.Interfaces;

namespace WebService.Repositories;

public class MessageRepository : IMessageRepository
{
	public Task AddMessageAsync(Message chatRoom)
	{
		throw new NotImplementedException();
	}

	public Task DeleteMessageAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<Message>> GetAllMessageAsync()
	{
		throw new NotImplementedException();
	}

	public Task<Message?> GetMessageByIdAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task SaveChangesAsync()
	{
		throw new NotImplementedException();
	}

	public void UpdateMessage(Message chatRoom)
	{
		throw new NotImplementedException();
	}

	private ChatRoomEntity MapToDataEntity(ChatRoom chatRoom)
	{
		throw new NotImplementedException();
	}
}
