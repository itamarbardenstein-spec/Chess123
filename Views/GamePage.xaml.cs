using Chess.ModelsLogic;
using Chess.ViewModel;

namespace Chess.Views;

public partial class GamePage : ContentPage
{
	public GamePage(Game game)
	{
		InitializeComponent();
		BindingContext = new GamePageVM(game);
    }
}