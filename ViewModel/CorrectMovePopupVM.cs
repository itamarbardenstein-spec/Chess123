using System.Windows.Input;
using Chess.Views;

namespace Chess.ViewModel
{
    public partial class CorrectMovePopupVM
    {
        #region Fields
        private readonly CorrectMovePopup _popup;
        #endregion
        #region Commands
        public ICommand NextPuzzleCommand { get; private set; }
        #endregion
        #region Properties
        public string ResultText { get; }
        public string ResultMessage { get; }
        #endregion
        #region Constructor
        public CorrectMovePopupVM(CorrectMovePopup popup,string title, string message)
        {
            _popup = popup;
            NextPuzzleCommand = new Command(SelectNextPuzzle);
            ResultText = title;
            ResultMessage = message;
        }
        #endregion
        #region Private Methods
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
