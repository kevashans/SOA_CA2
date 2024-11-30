using WebService.Models.Entities;

namespace WebService.Repositories.Interface;

public interface IChatRoomRepository
{
	Task<IEnumerable<ChatRoomEntity>> GetAllChatRoomAsync();
	Task<ChatRoomEntity?> GetChatRoomByIdAsync(int id);
	Task AddChatRoomAsync(ChatRoomEntity chatRoom);
	void UpdateChatRoom(ChatRoomEntity chatRoom);
	Task DeleteChatRoomAsync(int id);
	Task SaveChangesAsync();
}
