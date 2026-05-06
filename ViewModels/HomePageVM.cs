using System.Windows.Input;
using Chess.Views;
using Chess.ModelsLogic;

namespace Chess.ViewModels
{
    /// ViewModel for the main landing page, handling navigation to game modes and puzzles
    public class HomePageVM
    {
        #region Fields
        /// Instance of the current user profile, loaded from local storage
        private readonly User user = new();
        #endregion
        #region Commands
        /// Command to initiate the standard multiplayer game flow via the AppShell
        public ICommand PlayCommand { get; }
        /// Command to navigate the user to the puzzle difficulty selection screen
        public ICommand PuzzleCommand { get; }
        #endregion
        #region Properties
        /// Gets or sets the display name of the authenticated user
        public string UserName
        {
            get => user.UserName;
            set
            {
                user.UserName = value;
            }
        }
        #endregion
        #region Constructor
        /// Initializes navigation commands for the home screen
        public HomePageVM()
        {
            PlayCommand = new Command(Play);
            PuzzleCommand = new Command(ShowPuzzle);
        }
        #endregion
        #region Private Methods
        /// Transitions the UI to the puzzle mode selection page
        private void ShowPuzzle(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new PuzzleDifficultyPage();
            });
        }
        /// Switches the main application shell to begin the online play sequence
        private void Play(object? sender)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new AppShell();
            });
        }
        #endregion
    }
}