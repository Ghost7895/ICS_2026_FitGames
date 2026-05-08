namespace FitGames.app.Messages;

public record GameEditMessage
{
    public required Guid GameId { get; init; }
}
