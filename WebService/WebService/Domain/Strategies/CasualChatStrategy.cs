using WebService.Domain.Strategies.Interfaces;

namespace WebService.Domain.Strategies
{
    public class CasualChatStrategy : IChatStrategy
	{
		/// <summary>
		/// Return ChatGPT response in a casual context
		/// </summary>
		/// <param name="userMessage"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public string Respond(string userMessage)
		{
			throw new NotImplementedException();
		}
	}
}
