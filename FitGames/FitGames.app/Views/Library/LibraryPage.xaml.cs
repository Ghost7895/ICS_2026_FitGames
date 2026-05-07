using FitGames.app.ViewModels.Library;

namespace FitGames.app.Views.Library;

public partial class LibraryPage : ContentPageBase
{
    public LibraryPage(LibraryViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}