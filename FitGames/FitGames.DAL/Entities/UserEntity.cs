namespace FitGames.DAL.Entities;

public record UserEntity : IEntity
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? PhoneNumber { get; set; }

    public required LibraryEntity Library { get; init; }


}