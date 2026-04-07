using CommunityToolkit.Mvvm.ComponentModel;

namespace FitGames.BL.Models;

public partial class DeveloperListModel : ModelBase
{
    [ObservableProperty]
    public required partial string Name { get; set; }

    public static DeveloperListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Name = string.Empty
    };
}
