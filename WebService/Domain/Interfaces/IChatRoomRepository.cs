using Domain.Entities;

namespace Domain.Interfaces;

public interface IChatRoomRepository
{
	Task<IEnumerable<ChatRoom>> GetAllChatRoomAsync();
	Task<ChatRoom?> GetChatRoomByIdAsync(int id);
	Task AddChatRoomAsync(ChatRoom chatRoom);
	void UpdateChatRoom(ChatRoom chatRoom);
	Task DeleteChatRoomAsync(int id);
	Task SaveChangesAsync();
}
