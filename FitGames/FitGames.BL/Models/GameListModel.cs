using CommunityToolkit.Mvvm.ComponentModel;
using FitGames.DAL.Enums;

namespace FitGames.BL.Models;


public partial class GameListModel : ModelBase
{
    [ObservableProperty]
    public required partial string Name { get; set; }

    [ObservableProperty]
    public partial string? ImageUrl { get; set; }

    [ObservableProperty]
    public partial Genre Genre { get; set; }

    public static GameListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty,
        Genre = Genre.Unknown
    };
}