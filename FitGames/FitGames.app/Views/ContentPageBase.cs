using FitGames.app.ViewModels;

namespace FitGames.app.Views;

public abstract class ContentPageBase : ContentPage
{
    protected ViewModelBase ViewModel { get; }

    public ContentPageBase(ViewModelBase viewModel)
    {
        BindingContext = ViewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.OnAppearingAsync();
    }
}