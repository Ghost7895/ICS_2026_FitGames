using CommunityToolkit.Mvvm.ComponentModel;

namespace FitGames.BL.Models;

public partial class UserDetailModel : ModelBase
{
    [ObservableProperty]
    public required partial string Username { get; set; }

    [ObservableProperty]
    public required partial string Email { get; set; }

    [ObservableProperty]
    public partial string? Name { get; set; }

    [ObservableProperty]
    public partial string? Surname { get; set; }

    [ObservableProperty]
    public partial string? PhoneNumber { get; set; }

    [ObservableProperty]
    public required partial Guid LibraryId { get; set; }

    public static UserDetailModel Empty
        => new()
        {
            Id = Guid.Empty,
            Username = string.Empty,
            Email = string.Empty,
            LibraryId = Guid.Empty
        };

    public static UserDetailModel Copy(UserDetailModel model,
        string? username = null, string? email = null, string? name = null,
        string? surname = null, string? phoneNumber = null, Guid? libraryId = null)
        => new()
        {
            Id = model.Id,
            Username = username ?? model.Username,
            Email = email ?? model.Email,
            Name = name ?? model.Name,
            Surname = surname ?? model.Surname,
            PhoneNumber = phoneNumber ?? model.PhoneNumber,
            LibraryId = libraryId ?? model.LibraryId
        };
}
