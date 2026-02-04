
using System.Windows.Input;
using Chess.Views;

namespace Chess.ViewModel
{
    public partial class CorrectMovePopupVM
    {
        private readonly CorrectMovePopup _popup;
        public ICommand NextPuzzleCommand { get; private set; }
        public string ResultText { get; }
        public string ResultMessage { get; }
        public CorrectMovePopupVM(CorrectMovePopup popup,string title, string message)
        {
            _popup = popup;
            NextPuzzleCommand = new Command(SelectNextPuzzle);
            ResultText = title;
            ResultMessage = message;
        }
        private void SelectNextPuzzle(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new PuzzleDifficultyPage();
                }
            });
            _popup.Close();
        }

    }
}
