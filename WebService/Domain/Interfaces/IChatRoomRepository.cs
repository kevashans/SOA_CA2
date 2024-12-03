using Domain.Entities;

namespace Domain.Interfaces;

public interface IChatRoomRepository
{
	Task<IEnumerable<ChatRoom>> GetAllChatRoomAsync();
	Task<ChatRoom?> GetChatRoomByIdAsync(Guid id);
	Task AddChatRoomAsync(ChatRoom chatRoom);
	void UpdateChatRoom(ChatRoom chatRoom);
	Task DeleteChatRoomAsync(int id);
	Task SaveChangesAsync();
	Task SaveAsync(ChatRoom chatRoom);
}
