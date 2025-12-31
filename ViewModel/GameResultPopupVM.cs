using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public partial class GameResultPopupVM
    {
        private readonly GameResultPopup _popup;
        public ICommand HomeCommand { get; private set; }
        public string ResultText { get; }
        public string ResultMessage { get; }
        public GameResultPopupVM(GameResultPopup popup, string title, string message)
        {
            _popup = popup;
            ResultText = title;
            ResultMessage = message;
            HomeCommand = new Command(TransferHome);
        }
        private void TransferHome(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new HomePage();
                }
            });
            _popup.Close();
        }

    }
}
