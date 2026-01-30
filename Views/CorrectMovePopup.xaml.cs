
using Chess.ViewModel;
using CommunityToolkit.Maui.Views;
namespace Chess.Views;
public partial class CorrectMovePopup : Popup
{
	public CorrectMovePopup(string title, string message)
	{
		InitializeComponent();
        BindingContext = new CorrectMovePopupVM(this, title, message);
    }
}