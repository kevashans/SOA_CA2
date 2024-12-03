using Domain.Entities;
namespace Services.Interfaces;

public interface IMessageService
{
	Task<Message> AddPrompt(string chatRoomId, string userId, string prompt);
}
