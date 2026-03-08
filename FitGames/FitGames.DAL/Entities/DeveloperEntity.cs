namespace FitGames.DAL.Entities;

public record DeveloperEntity : IEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public ICollection<GameEntity> PublishedGames { get; init; } = new List<GameEntity>();
}