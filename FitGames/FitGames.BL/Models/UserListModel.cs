using CommunityToolkit.Mvvm.ComponentModel;

namespace FitGames.BL.Models;

public partial class UserListModel : ModelBase
{
    [ObservableProperty]
    public required partial string Username { get; set; }

    [ObservableProperty]
    public required partial string Email { get; set; }

    [ObservableProperty]
    public partial string? Name { get; set; }

    [ObservableProperty]
    public partial string? Surname { get; set; }

    public static UserListModel Empty => new()
    {
        Id = Guid.NewGuid(),
        Username = string.Empty,
        Email = string.Empty
    };
}
