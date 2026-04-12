using CommunityToolkit.Mvvm.ComponentModel;

namespace FitGames.BL.Models;

public abstract partial class ModelBase : ObservableObject
{
    [ObservableProperty]
    public partial Guid Id { get; set; }
}