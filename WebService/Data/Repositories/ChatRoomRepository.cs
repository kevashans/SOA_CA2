using Data.Entities;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

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

	public async Task DeleteChatRoomAsync(Guid id)
	{
		var chatRoom = await _context.ChatRooms.FindAsync(id);

		if (chatRoom != null)
			_context.ChatRooms.Remove(chatRoom);
	}

	public async Task<ChatRoom?> GetChatRoomByIdAsync(Guid id)
	{
		var chatroom = await _context.ChatRooms.FirstOrDefaultAsync(cr => cr.ChatRoomId == id);
		return chatroom is not null ? MapToDomainEntity(chatroom) : null;
	}

	public async Task SaveChangesAsync()
	{
		await _context.SaveChangesAsync();
	}

	public async Task<IEnumerable<ChatRoom>> GetChatRoomsByUserIdAsync(string userId)
	{
		var chatRoomEntities = await _context.ChatRooms
			.Where(cr => cr.UserId == userId)
			.ToListAsync();

		return chatRoomEntities.Select(MapToDomainEntity);
	}

	public async Task SaveAsync(ChatRoom chatRoom)
	{
		var trackedEntity = _context.ChatRooms.Local.FirstOrDefault(cr => cr.ChatRoomId == chatRoom.ChatRoomId);

		if (trackedEntity != null)
		{
			trackedEntity.Name = chatRoom.Name;
			trackedEntity.ChatRoomType = chatRoom.ChatRoomType;
		}

		await _context.SaveChangesAsync();
	}

	private ChatRoomEntity MapToDataEntity(ChatRoom chatRoom)
	{
		return new ChatRoomEntity(chatRoom.ChatRoomId,chatRoom.UserId, chatRoom.Name, chatRoom.ChatRoomType);
	}

	private ChatRoom MapToDomainEntity(ChatRoomEntity chatRoom)
	{
		return new ChatRoom(chatRoom.ChatRoomId,chatRoom.UserId, chatRoom.Name, chatRoom.ChatRoomType);
	}
}
