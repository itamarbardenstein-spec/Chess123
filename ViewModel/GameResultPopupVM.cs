using Chess.Views;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public partial class GameResultPopupVM
    {
        #region Fields
        private readonly GameResultPopup _popup;
        #endregion
        #region Commands
        public ICommand HomeCommand { get; private set; }
        #endregion
        #region Properties
        public string ResultText { get; }
        public string ResultMessage { get; }
        #endregion
        #region Constructor
        public GameResultPopupVM(GameResultPopup popup, string title, string message)
        {
            _popup = popup;
            ResultText = title;
            ResultMessage = message;
            HomeCommand = new Command(TransferHome);
        }
        #endregion
        #region Private Methods
        private void TransferHome(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new HomePage();
            });
            _popup.Close();
        }
        #endregion
    }
}
