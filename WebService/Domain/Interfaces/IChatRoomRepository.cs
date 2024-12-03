using Domain.Entities;

namespace Domain.Interfaces;

public interface IChatRoomRepository
{
	Task<ChatRoom?> GetChatRoomByIdAsync(Guid id);
	Task AddChatRoomAsync(ChatRoom chatRoom);
	Task DeleteChatRoomAsync(Guid id);
	Task SaveChangesAsync();
	Task SaveAsync(ChatRoom chatRoom);
	Task<IEnumerable<ChatRoom>> GetChatRoomsByUserIdAsync(string userId);
}
