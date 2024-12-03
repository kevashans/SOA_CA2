using Data;
using Data.Entities;
using Domain.Entities;
using Domain.Interfaces;

namespace WebService.Repositories;

public class ChatRoomRepository : IChatRoomRepository
{
	private readonly ApplicationDbContext _context;

	public ChatRoomRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task AddChatRoomAsync(ChatRoom chatRoom)
	{
		await _context.AddAsync(MapToDataEntity(chatRoom));
	}

	public async Task DeleteChatRoomAsync(int id)
	{
		var chatRoom = await _context.ChatRooms.FindAsync(id);

		if (chatRoom != null)
			_context.ChatRooms.Remove(chatRoom);
	}

	public async Task<IEnumerable<ChatRoom>> GetAllChatRoomAsync()
	{
		throw new NotImplementedException();

	}

	public async Task<ChatRoom?> GetChatRoomByIdAsync(int id)
	{
		throw new NotImplementedException();
	}

	public async Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync();
	}

	public void UpdateChatRoom(ChatRoom chatRoom)
	{
		throw new NotImplementedException();
	}

	private ChatRoomEntity MapToDataEntity(ChatRoom chatRoom)
	{
		return new ChatRoomEntity(chatRoom.UserId, chatRoom.Name, chatRoom.ChatRoomType);
	}

	private ChatRoom MapToDomainEntity(ChatRoom chatRoom)
	{
		throw new NotImplementedException();
	}

}
