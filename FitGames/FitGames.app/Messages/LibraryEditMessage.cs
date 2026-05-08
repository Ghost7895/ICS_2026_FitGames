namespace FitGames.app.Messages;

public record LibraryEditMessage
{
    public required Guid LibraryId { get; init; }
}
