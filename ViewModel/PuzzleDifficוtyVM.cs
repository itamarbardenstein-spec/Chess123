using System.Windows.Input;
using Chess.Models;
using Chess.Views;

namespace Chess.ViewModel
{
    /// ViewModel for selecting the difficulty level of chess puzzles
    public class PuzzleDifficוtyVM
    {
        #region Commands
        /// Command to start a puzzle session with low difficulty
        public ICommand EasyPuzzleCommand { get; }
        /// Command to start a puzzle session with moderate difficulty
        public ICommand MediumPuzzleCommand { get; }
        /// Command to start a puzzle session with high difficulty
        public ICommand HardPuzzleCommand { get; }
        /// Command to return to the home screen
        public ICommand HomeCommand { get; }
        #endregion
        #region Constructor
        /// Initializes navigation commands for difficulty selection
        public PuzzleDifficוtyVM()
        {
            EasyPuzzleCommand = new Command(SelectEasyPuzzle);
            MediumPuzzleCommand = new Command(SelectMediumPuzzle);
            HardPuzzleCommand = new Command(SelectHardPuzzle);
            HomeCommand = new Command(TrasnferHome);
        }
        #endregion
        #region Private Methods
        /// Navigates the user back to the HomePage
        private void TrasnferHome(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new HomePage();
            });
        }
        /// Switches the view to a new PuzzlePage configured with Hard difficulty
        private void SelectHardPuzzle(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new PuzzlePage(Strings.Hard);
            });
        }
        /// Switches the view to a new PuzzlePage configured with Medium difficulty
        private void SelectMediumPuzzle(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new PuzzlePage(Strings.Medium);
            });
        }
        /// Switches the view to a new PuzzlePage configured with Easy difficulty
        private void SelectEasyPuzzle(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new PuzzlePage(Strings.Easy);
            });
        }
        #endregion
    }
}