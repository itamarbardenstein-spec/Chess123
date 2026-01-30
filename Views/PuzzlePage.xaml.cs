using Chess.ModelsLogic;
using Chess.ViewModel;

namespace Chess.Views;

public partial class PuzzlePage : ContentPage
{
    private readonly PuzzlePageVM ppVM;
    public PuzzlePage()
	{
		InitializeComponent();
        ppVM = new PuzzlePageVM(puzzleBoard);
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