using FitGames.app.Services.Interfaces;

namespace FitGames.app.ViewModels.Library;

public class LibraryViewModel : ViewModelBase
{
    public LibraryViewModel(IMessengerService messengerService) : base(messengerService)
    {
    }
}