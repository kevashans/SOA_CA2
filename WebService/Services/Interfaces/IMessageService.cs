using Domain.Entities;
using static Domain.DTOs.MessageDTOs;
namespace Services.Interfaces;

public interface IMessageService
{
	Task<Message> AddPrompt(string chatRoomId, string userId, string prompt);

	Task<IEnumerable<Message?>> GetChatroomMessages(string chatRoomId, string userId);

	Task<Message> EditMessage(string chatRoomId, string userId, EditMessageRequest message);

	Task DeleteMessage(string messageId, string userId ,DeleteMessageRequest request);
}
