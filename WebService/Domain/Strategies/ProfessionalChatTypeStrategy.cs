using Domain.Strategies.Interfaces;


namespace Domain.Strategies
{
	internal class ProfessionalChatTypeStrategy : IChatTypeStrategy
	{
		private readonly IChatResponseGenerator _responseGenerator;

		public ProfessionalChatTypeStrategy(IChatResponseGenerator responseGenerator)
		{
			_responseGenerator = responseGenerator;
		}

		public Task<string> Respond(string userMessage)
		{
			throw new NotImplementedException();
		}
	}
}
