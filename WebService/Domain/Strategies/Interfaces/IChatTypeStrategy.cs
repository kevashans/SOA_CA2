namespace Domain.Strategies.Interfaces;

public interface IChatTypeStrategy
{
    Task<string> Respond(string userMessage);

    void ProvideContext(string? summary);
}
