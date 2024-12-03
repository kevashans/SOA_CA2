using Domain.Strategies.Interfaces;


namespace Domain.Strategies
{
	internal class ProfessionalChatTypeStrategy : IChatTypeStrategy
	{
		public Task<string> Respond(string userMessage)
		{
			throw new NotImplementedException();
		}
	}
}
