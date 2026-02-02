using System.Windows.Input;
using Chess.Models;
using Chess.Views;

namespace Chess.ViewModel
{
    public class PuzzleDifficוtyVM
    {
        public ICommand EasyPuzzleCommand { get; }
        public ICommand MediumPuzzleCommand { get; }
        public ICommand HardPuzzleCommand { get; }
        public ICommand HomeCommand { get; }
        public PuzzleDifficוtyVM()
        {
            EasyPuzzleCommand = new Command(SelectEasyPuzzle);
            MediumPuzzleCommand = new Command(SelectMediumPuzzle);
            HardPuzzleCommand = new Command(SelectHardPuzzle);
            HomeCommand= new Command(TrasnferHome);
        }

        private void TrasnferHome(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new HomePage();
                }
            });
        }
        private void SelectHardPuzzle(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new PuzzlePage(Strings.Hard);
                }
            });
        }

        private void SelectMediumPuzzle(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new PuzzlePage(Strings.Medium);
                }
            });
        }

        private void SelectEasyPuzzle(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                {
                    Application.Current.MainPage = new PuzzlePage(Strings.Easy);
                }
            });
        }
    }
}
