using System.Windows.Input;
using Chess.Views;

namespace Chess.ViewModels
{
    /// ViewModel for the puzzle completion popup, managing results and navigation
    public partial class CorrectMovePopupVM
    {
        #region Fields
        /// Reference to the popup UI element for controlling its lifecycle
        private readonly CorrectMovePopup _popup;
        #endregion
        #region Commands
        /// Command to navigate the user back to the puzzle selection screen
        public ICommand NextPuzzleCommand { get; private set; }
        #endregion
        #region Properties
        /// The main header text indicating the result of the puzzle
        public string ResultText { get; }
        /// Detailed feedback or success message for the user
        public string ResultMessage { get; }
        #endregion
        #region Constructor
        /// Initializes the ViewModel with the popup instance and the text to display
        public CorrectMovePopupVM(CorrectMovePopup popup, string title, string message)
        {
            _popup = popup;
            NextPuzzleCommand = new Command(SelectNextPuzzle);
            ResultText = title;
            ResultMessage = message;
        }
        #endregion
        #region Private Methods
        /// Redirects the user to the difficulty selection page and closes the current popup
        private void SelectNextPuzzle(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new PuzzleDifficultyPage();
            });
            _popup.Close();
        }
        #endregion
    }
}