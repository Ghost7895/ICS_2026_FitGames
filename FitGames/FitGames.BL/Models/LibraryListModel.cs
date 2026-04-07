using CommunityToolkit.Mvvm.ComponentModel;

namespace FitGames.BL.Models;

public partial class LibraryListModel : ModelBase
{
    [ObservableProperty]
    public required partial string Name { get; set; }

    public static LibraryListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty
    };
}
