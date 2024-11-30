using WebService.Models.Entities;
using WebService.Repositories.Interface;

namespace WebService.Repositories;

public class MessageRepository : IMessageRepository
{
	public Task AddMessageAsync(MessageEntity chatRoom)
	{
		throw new NotImplementedException();
	}

	public Task DeleteMessageAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<MessageEntity>> GetAllMessageAsync()
	{
		throw new NotImplementedException();
	}

	public Task<MessageEntity?> GetMessageByIdAsync(int id)
	{
		throw new NotImplementedException();
	}

	public Task SaveChangesAsync()
	{
		throw new NotImplementedException();
	}

	public void UpdateMessage(MessageEntity chatRoom)
	{
		throw new NotImplementedException();
	}
}
