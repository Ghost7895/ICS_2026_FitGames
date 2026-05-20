using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitGames.app.Services.Interfaces;

namespace FitGames.app.ViewModels;

public abstract partial class ViewModelBase : ObservableRecipient
{
    private bool _forceDataRefresh = true;

    protected readonly IMessengerService MessengerService;

    protected ViewModelBase(IMessengerService messengerService)
        : base(messengerService.Messenger)
    {
        MessengerService = messengerService;

        IsActive = true;
    }

    public async Task OnAppearingAsync()
    {
        if (_forceDataRefresh)
        {
            await LoadDataAsync();

            _forceDataRefresh = false;
        }
    }

    protected void ForceDataRefreshOnNextAppearing()
    {
        _forceDataRefresh = true;
    }

    [RelayCommand]
    public async Task RefreshAsync()
    {
        await LoadDataAsync();
    }

    protected virtual Task LoadDataAsync()
        => Task.CompletedTask;
}
