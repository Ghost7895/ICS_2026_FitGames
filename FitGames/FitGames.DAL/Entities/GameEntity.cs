using FitGames.DAL.Enums;

namespace FitGames.DAL.Entities
{
    public record GameEntity : IEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string DeveloperName { get; set; }
        public required Pegi Pegi { get; set; }
        public required Genre Genre { get; set; }
        public string? ImageUrl { get; set; }

    }
}
