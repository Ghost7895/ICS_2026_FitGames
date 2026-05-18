using FitGames.app.ViewModels.Profile;

namespace FitGames.app.Views.Profile;

public partial class ProfilePage : ContentPageBase
{
    public ProfilePage(ProfileViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
