using System.Windows.Input;
using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace Chess.ViewModel
{
    public partial class PuzzlePageVM
    {
        private readonly PuzzleGrid puzzleGridBoard = [];
        private readonly Puzzle puzlle= new();
        public ICommand ShowHintCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }
        public ICommand ShowMoveCommand { get; private set; }
        public PuzzlePageVM(Grid puzzleBoard)
        {
            puzzleGridBoard.InitPuzzleGrid(puzzleBoard);
            puzzleGridBoard.ButtonClicked += OnButtonClicked;
            puzlle.LegalMoves += ShowLegalMoves;
            puzlle.ClearLegalMovesDots += ClearDots;
            puzlle.CorrectMove += OnCorrectMove;
            puzlle.IncorrectMove += OnIncorrectMove;
            ShowHintCommand=new Command(ShowHint);
            HomeCommand = new Command(TransferHome);
            ShowMoveCommand=new Command(ShowCorrectMove);
        }

        private void ShowCorrectMove(object obj)
        {
            puzzleGridBoard.ShowCorrectMove();
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
        }

        private void ShowHint(object obj)
        {
            puzzleGridBoard.ShowHint();
        }

        private void OnIncorrectMove(object? sender, EventArgs e)
        {
            string reason = Strings.IncorrectMoveMessage;
            string title = Strings.IncorrectMoveTitle;
            Application.Current?.MainPage?.ShowPopup(new CorrectMovePopup(title, reason));
        }
        private void OnCorrectMove(object? sender, EventArgs e)
        {
            string reason = Strings.CorrectMoveMessage;
            string title = Strings.CorrectMoveTitle;
            Application.Current?.MainPage?.ShowPopup(new CorrectMovePopup(title, reason));
        }
        private void OnButtonClicked(object? sender, Piece e)
        {
            puzlle.OnButtonClicked(e);
        }
        private void ClearDots(object? sender, EventArgs e)
        {
            puzzleGridBoard.ClearDots();
        }
        private void ShowLegalMoves(object? sender, List<int[]> e)
        {
            puzzleGridBoard.ShowLegalMoves(e);
        }
    }
}
