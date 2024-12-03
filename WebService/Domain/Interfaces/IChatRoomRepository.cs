using Domain.Entities;

namespace Domain.Interfaces;

public interface IChatRoomRepository
{
	Task<IEnumerable<ChatRoom>> GetAllChatRoomAsync();
	Task<ChatRoom?> GetChatRoomByIdAsync(Guid id);
	Task AddChatRoomAsync(ChatRoom chatRoom);
	void UpdateChatRoom(ChatRoom chatRoom);
	Task DeleteChatRoomAsync(Guid id);
	Task SaveChangesAsync();
	Task SaveAsync(ChatRoom chatRoom);
	Task<IEnumerable<ChatRoom>> GetChatRoomsByUserIdAsync(string userId);
}
