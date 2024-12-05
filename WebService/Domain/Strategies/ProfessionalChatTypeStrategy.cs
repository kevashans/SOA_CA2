using Domain.Strategies.Interfaces;


namespace Domain.Strategies
{
	internal class ProfessionalChatTypeStrategy : IChatTypeStrategy
	{
		private readonly IChatResponseGenerator _responseGenerator;
		private string _systemPrompt = "You are a classy professional.";

		public ProfessionalChatTypeStrategy(IChatResponseGenerator responseGenerator)
		{
			_responseGenerator = responseGenerator;
		}

		public async Task<string> Respond(string userMessage)
		{
			return await _responseGenerator.GenerateResponseAsync(_systemPrompt, userMessage);
		}

		public void ProvideContext(string? summary)
		{
			_systemPrompt += $" keep in mind the previous chat messages: {summary}";
		}
	}
}
