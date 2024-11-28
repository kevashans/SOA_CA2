using Data;
using Data.Entities;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


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
		await _context.ChatRooms.AddAsync(chatRoom);
	}

	public async Task DeleteChatRoomAsync(int id)
	{
		var chatRoom = await _context.ChatRooms.FindAsync(id);

		if (chatRoom != null)
			_context.ChatRooms.Remove(chatRoom);
	}

	public async Task<IEnumerable<ChatRoom>> GetAllChatRoomAsync()
	{
		return await _context.ChatRooms.ToListAsync();
	}

	public async Task<ChatRoom?> GetChatRoomByIdAsync(int id)
	{
		return await _context.ChatRooms.FindAsync(id) ?? null;
	}

	public async Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync();
	}

	public void UpdateChatRoom(ChatRoom chatRoom)
	{
		 _context.ChatRooms.Update(chatRoom);
	}

	private ChatRoomEntity MapToDataEntity(ChatRoom chatRoom)
	{
		throw new NotImplementedException();
	}

	private ChatRoom MapToDomainEntity(ChatRoom chatRoom)
	{
		throw new NotImplementedException();
	}

}
