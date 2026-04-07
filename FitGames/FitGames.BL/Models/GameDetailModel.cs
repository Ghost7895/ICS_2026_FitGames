using CommunityToolkit.Mvvm.ComponentModel;
using FitGames.DAL.Enums;

namespace FitGames.BL.Models;

public partial class GameDetailModel : ModelBase
{
    [ObservableProperty]
    public required partial string Name { get; set; }

    [ObservableProperty]
    public required partial string Description { get; set; }

    [ObservableProperty]
    public partial Pegi Pegi { get; set; }

    [ObservableProperty]
    public partial Genre Genre { get; set; }

    [ObservableProperty]
    public partial string? ImageUrl { get; set; }

    [ObservableProperty]
    public partial string? DeveloperName { get; set; }

    public static GameDetailModel Empty
        => new()
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Description = string.Empty,
            DeveloperName = "Unknown"
        };

    public static GameDetailModel Copy(GameDetailModel model,
        string? name = null, string? description = null, Pegi? pegi = null,
        Genre? genre = null, string? imageUrl = null, string? developerName = null)
        => new()
        {
            Id = model.Id,
            Name = name ?? model.Name,
            Description = description ?? model.Description,
            Pegi = pegi ?? model.Pegi,
            Genre = genre ?? model.Genre,
            ImageUrl = imageUrl ?? model.ImageUrl,
            DeveloperName = developerName ?? model.DeveloperName
        };
}