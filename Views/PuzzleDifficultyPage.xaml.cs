using Chess.ViewModel;

namespace Chess.Views;

public partial class PuzzleDifficultyPage : ContentPage
{
	public PuzzleDifficultyPage()
	{
		InitializeComponent();
		BindingContext = new PuzzleDifficåtyVM();
    }
}