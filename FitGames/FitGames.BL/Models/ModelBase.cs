using CommunityToolkit.Mvvm.ComponentModel;

namespace FitGames.BL.Models;

public abstract class ModelBase : ObservableObject
{
    public Guid Id { get; set; }
}
