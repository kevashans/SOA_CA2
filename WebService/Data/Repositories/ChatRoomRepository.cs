using Data;
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

	public async Task<IEnumerable<ChatRoom>> GetAllChatRoomAsync()
	{
		throw new NotImplementedException();

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

	public void UpdateChatRoom(ChatRoom chatRoom)
	{
		//var existingEntity = _context.ChatRooms.FirstOrDefault(cr => cr.ChatRoomId == chatRoom.ChatRoomId);

		//if (existingEntity == null)
		//	throw new KeyNotFoundException($"ChatRoom with ID {chatRoom.ChatRoomId} not found.");

		//existingEntity.Name = chatRoom.Name ?? existingEntity.Name;
		//existingEntity.ChatRoomType = chatRoom.ChatRoomType ?? existingEntity.ChatRoomType;
		//existingEntity.UserId = chatRoom.UserId;
		// Attach the detached entity
		var chatRoomEntity = MapToDataEntity(chatRoom);

		// Update the entire entity
		_context.ChatRooms.Update(chatRoomEntity);
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
			// update tracked entity
			trackedEntity.Name = chatRoom.Name;
			trackedEntity.ChatRoomType = chatRoom.ChatRoomType;
		}
		else
		{
			// attach entity for persistence
			var entity = MapToDataEntity(chatRoom);
			_context.ChatRooms.Attach(entity);
			_context.Entry(entity).State = EntityState.Modified;
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
