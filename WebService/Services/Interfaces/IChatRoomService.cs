using Domain.DTOs;
using Domain.Entities;

namespace Services.Interfaces;
public interface IChatRoomService
{
	Task<CreateChatRoomResponse> CreateChatRoom(CreateChatRoomRequest chatroomRequest, string userId);

	Task<UpdateChatRoomResponse> UpdateChatRoom(string chatRoomId, UpdateChatRoomRequest updateChatRoomRequest, string userId);

	Task<IEnumerable<ChatRoomResponse>> GetChatRoomByUserId(string userId);

	Task DeleteChatRoomById(string chatRoomId, string userId);
}
