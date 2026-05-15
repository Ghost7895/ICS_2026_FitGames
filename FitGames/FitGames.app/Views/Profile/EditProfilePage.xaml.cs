using FitGames.app.ViewModels.Profile;

namespace FitGames.app.Views.Profile;

public partial class EditProfilePage : ContentPageBase
{
    public EditProfilePage(EditProfileViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
