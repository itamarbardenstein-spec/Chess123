using Chess.Models;
using Chess.ViewModel;
using CommunityToolkit.Maui.Views;

namespace Chess.Views
{
    public partial class GameResultPopup : Popup
    {
        public GameResultPopup(string title, string message)
        {
            InitializeComponent();
            BindingContext = new GameResultPopupVM(this, title, message);
        }
    }
}