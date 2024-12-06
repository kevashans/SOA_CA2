using Domain.Entities;
using static Domain.DTOs.MessageDTOs;
namespace Services.Interfaces;

public interface IMessageService
{
	Task<CreateMessageResponse> AddPrompt(string chatRoomId, string userId, CreateMessageRequests prompt);

	Task<IEnumerable<MessageResponse?>> GetChatroomMessages(string chatRoomId, string userId);

	Task<EditMessageResponse> EditMessage(string chatRoomId, string messageId, string userId, EditMessageRequest message);

	Task DeleteMessage(string messageId, string userId ,string chatRoomId);
}
