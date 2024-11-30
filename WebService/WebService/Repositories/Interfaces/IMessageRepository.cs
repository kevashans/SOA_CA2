using WebService.Models.Entities;

namespace WebService.Repositories.Interface;

public interface IMessageRepository
{
	Task<IEnumerable<MessageEntity>> GetAllMessageAsync();
	Task<MessageEntity?> GetMessageByIdAsync(int id);
	Task AddMessageAsync(MessageEntity chatRoom);
	void UpdateMessage(MessageEntity chatRoom);
	Task DeleteMessageAsync(int id);
	Task SaveChangesAsync();
}
