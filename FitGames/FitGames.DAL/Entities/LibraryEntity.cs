namespace FitGames.DAL.Entities;

public record LibraryEntity : IEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<GameEntity> Games { get; init; } = new List<GameEntity>();

}