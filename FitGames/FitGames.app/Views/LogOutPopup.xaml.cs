using System;

namespace FitGames.app.Views;

public partial class LogOutPopup : ContentPage
{
    public LogOutPopup()
    {
        InitializeComponent();
    }

    private async void OnLogOutClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
        await Shell.Current.GoToAsync("//signin");
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}