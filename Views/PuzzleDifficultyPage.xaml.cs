using Chess.ViewModels;

namespace Chess.Views;

public partial class PuzzleDifficultyPage : ContentPage
{
	public PuzzleDifficultyPage()
	{
		InitializeComponent();
		BindingContext = new PuzzleDifficåtyPageVM();
    }
}