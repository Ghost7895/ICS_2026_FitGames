using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace FitGames.BL.Models;

public partial class LibraryDetailModel : ModelBase
{
    [ObservableProperty]
    public required partial string Name { get; set; }

    [ObservableProperty]
    public partial ObservableCollection<GameListModel> Games { get; set; } = [];

    public static LibraryDetailModel Empty
        => new()
        {
            Id = Guid.Empty,
            Name = string.Empty,
            Games = []
        };

    public static LibraryDetailModel Copy(LibraryDetailModel model,
        string? name = null, ObservableCollection<GameListModel>? games = null)
        => new()
        {
            Id = model.Id,
            Name = name ?? model.Name,
            Games = games ?? model.Games
        };
}
