using Chess.Views;
using System.Windows.Input;

namespace Chess.ViewModel
{
    /// ViewModel for the final game result popup, displaying win/loss details and navigation
    public partial class GameResultPopupVM
    {
        #region Fields
        /// Reference to the result popup UI for lifecycle management
        private readonly GameResultPopup _popup;
        #endregion
        #region Commands
        /// Command to redirect the user back to the home screen after a game
        public ICommand HomeCommand { get; private set; }
        #endregion
        #region Properties
        /// The main heading text for the result (e.g., "Victory" or "Defeat")
        public string ResultText { get; }
        /// Detailed information regarding why the game ended (e.g., "Checkmate" or "Resignation")
        public string ResultMessage { get; }
        #endregion
        #region Constructor
        /// Initializes the ViewModel with the popup instance and the final game outcomes
        public GameResultPopupVM(GameResultPopup popup, string title, string message)
        {
            _popup = popup;
            ResultText = title;
            ResultMessage = message;
            HomeCommand = new Command(TransferHome);
        }
        #endregion
        #region Private Methods
        /// Navigates the application back to the HomePage on the UI thread and closes the popup
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