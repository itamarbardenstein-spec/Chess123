using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using System.Windows.Input;

namespace Chess.ViewModel
{
    public class HomePageVM
    {
        #region Fields
        private readonly User user = new();
        #endregion
        #region Commands
        public ICommand PlayCommand { get; }
        public ICommand PuzzleCommand { get; }        
        #endregion
        #region Properties
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
        public HomePageVM()
        {
            PlayCommand = new Command(Play);
            PuzzleCommand = new Command(ShowPuzzle);       
            
        }
        #endregion
        #region Private Methods
        private void ShowPuzzle(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new PuzzleDifficultyPage();
            });
        }       
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
