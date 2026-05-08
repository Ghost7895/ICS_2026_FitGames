 using FitGames.app.ViewModels.Game;

namespace FitGames.app.Views.Game;

public partial class GameDetailPage : ContentPageBase
{
    public GameDetailPage(GameDetailViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}