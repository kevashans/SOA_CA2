using Microsoft.EntityFrameworkCore;
using WebService.Data;
using WebService.Models.Entities;
using WebService.Repositories.Interface;

namespace WebService.Repositories;

public class ChatRoomRepository : IChatRoomRepository
{
	private readonly ApplicationDbContext _context;

	public ChatRoomRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task AddChatRoomAsync(ChatRoomEntity chatRoom)
	{
		await _context.ChatRooms.AddAsync(chatRoom);
	}

	public async Task DeleteChatRoomAsync(int id)
	{
		var chatRoom = await _context.ChatRooms.FindAsync(id);

		if (chatRoom != null)
			_context.ChatRooms.Remove(chatRoom);
	}

	public async Task<IEnumerable<ChatRoomEntity>> GetAllChatRoomAsync()
	{
		return await _context.ChatRooms.ToListAsync();
	}

	public async Task<ChatRoomEntity?> GetChatRoomByIdAsync(int id)
	{
		return await _context.ChatRooms.FindAsync(id) ?? null;
	}

	public async Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync();
	}

	public void UpdateChatRoom(ChatRoomEntity chatRoom)
	{
		 _context.ChatRooms.Update(chatRoom);
	}

}
