using Chess.ModelsLogic;
using Chess.ViewModel;

namespace Chess.Views;

public partial class PuzzlePage : ContentPage
{
    private readonly PuzzlePageVM ppVM;
    public PuzzlePage(string difficulty)
	{
		InitializeComponent();
        ppVM = new PuzzlePageVM(puzzleBoard, difficulty);
        BindingContext = ppVM;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}