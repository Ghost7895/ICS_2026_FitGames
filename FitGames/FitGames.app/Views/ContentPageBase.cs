using FitGames.app.ViewModels;

namespace FitGames.app.Views;

public abstract class ContentPageBase : ContentPage
{
    protected ViewModelBase ViewModel { get; }

    protected ContentPageBase(ViewModelBase viewModel)
    {
        //InitializeComponent();
        BindingContext = ViewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.OnAppearingAsync();
    }
}