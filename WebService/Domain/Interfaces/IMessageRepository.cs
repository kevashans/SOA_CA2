using Domain.Entities;

namespace Domain.Interfaces;

public interface IMessageRepository
{
	Task<Message?> GetMessageByIdAsync(Guid id);

	Task<IEnumerable<Message?>> GetMessagesByChatroomId(Guid id);

	Task AddMessageAsync(Message chatRoom);

	Task DeleteMessageAsync(Guid id);

	Task SaveChangesAsync();

	Task UpdateMessage(Message message);

	Task<IEnumerable<Message>> GetLastMessagesAsync(Guid chatRoomId, int count);
}
