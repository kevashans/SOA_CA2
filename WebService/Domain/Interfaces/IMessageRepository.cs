using Domain.Entities;

namespace Domain.Interfaces;

public interface IMessageRepository
{
	Task<IEnumerable<Message>> GetAllMessageAsync();

	Task<Message?> GetMessageByIdAsync(string id);

	Task<IEnumerable<Message?>> GetMessagesByChatroomId(Guid id);

	Task AddMessageAsync(Message chatRoom);

	void UpdateMessage(Message chatRoom);

	Task DeleteMessageAsync(int id);

	Task SaveChangesAsync();
}
