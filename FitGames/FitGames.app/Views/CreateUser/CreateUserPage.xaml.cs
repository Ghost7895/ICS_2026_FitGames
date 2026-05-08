using FitGames.app.ViewModels.CreateUser;

namespace FitGames.app.Views.CreateUser;

public partial class CreateUserPage : ContentPageBase
{
    public CreateUserPage(CreateUserViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}
