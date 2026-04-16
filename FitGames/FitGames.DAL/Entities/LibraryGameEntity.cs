namespace FitGames.DAL.Entities;

public record LibraryGameEntity
{
    public Guid LibraryId { get; set; }
    public Guid GameId { get; set; }

    public required LibraryEntity Library { get; init; }
    public required GameEntity Game { get; init; }
}
