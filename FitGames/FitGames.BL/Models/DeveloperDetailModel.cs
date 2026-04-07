using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace FitGames.BL.Models;

public partial class DeveloperDetailModel : ModelBase
{
    [ObservableProperty]
    public required partial string Name { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<GameListModel> PublishedGames { get; set; } = [];

    public static DeveloperDetailModel Empty
        => new()
        {
            Id = Guid.Empty,
            Name = string.Empty,
            PublishedGames = []
        };

    public static DeveloperDetailModel Copy(DeveloperDetailModel model,
        string? name = null, ObservableCollection<GameListModel>? publishedGames = null)
        => new()
        {
            Id = model.Id,
            Name = name ?? model.Name,
            PublishedGames = publishedGames ?? model.PublishedGames
        };
}
