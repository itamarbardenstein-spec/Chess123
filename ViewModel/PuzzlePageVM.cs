using System.Windows.Input;
using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace Chess.ViewModel
{
    public partial class PuzzlePageVM
    {
        private readonly PuzzleGrid puzzleGrid = [];
        private readonly Puzzle puzlle;
        public ICommand ShowHintCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }
        public ICommand ShowMoveCommand { get; private set; }
        public PuzzlePageVM(Grid puzzleBoard,string difficulty)
        {
            puzlle = new Puzzle(difficulty);
            if (difficulty=="Easy")
                puzzleGrid.InitEasyPuzzleGrid(puzzleBoard);
            else if(difficulty=="Medium")
                puzzleGrid.InitMediumPuzzleGrid(puzzleBoard);
            else
                puzzleGrid.InitHardPuzzleGrid(puzzleBoard);
            puzzleGrid.ButtonClicked += OnButtonClicked;
            puzlle.LegalMoves += ShowLegalMoves;
            puzlle.DisplayChanged += OnDisplayChanged;
            puzlle.MakeOpponentMove += MakeOpponentMove;
            puzlle.ClearLegalMovesDots += ClearDots;
            puzlle.CorrectMove += OnCorrectMove;
            puzlle.IncorrectMove += OnIncorrectMove;
            puzlle.CorrectSolution += OnCorrectSolution;
            ShowHintCommand =new Command(ShowHint);
            HomeCommand = new Command(TransferHome);
            ShowMoveCommand=new Command(ShowCorrectMove);
        }

        private void MakeOpponentMove(object? sender, string e)
        {
            puzzleGrid.MakeOpponentMove(e);
        }
        private void OnCorrectSolution(object? sender, EventArgs e)
        {
            string reason = Strings.CorrectSolutionMessage;
            string title = Strings.CorrectSolutionTitle;
            Application.Current?.MainPage?.ShowPopup(new CorrectMovePopup(title, reason));
        }

        private void OnDisplayChanged(object? sender, DisplayMoveArgs e)
        {
            puzzleGrid.UpdateDisplay(e);
        }
        private void ShowCorrectMove(object obj)
        {
            puzzleGrid.ShowCorrectMove();
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
            puzzleGrid.ShowHint();
        }

        private void OnIncorrectMove(object? sender, EventArgs e)
        {
            Toast.Make(Strings.IncorrectMoveMessage, ToastDuration.Short).Show();
        }
        private void OnCorrectMove(object? sender, EventArgs e)
        {
            Toast.Make(Strings.CorrectMove, ToastDuration.Short).Show();
        }
        private void OnButtonClicked(object? sender, Piece e)
        {
            puzlle.OnButtonClicked(e);
        }
        private void ClearDots(object? sender, EventArgs e)
        {
            puzzleGrid.ClearDots();
        }
        private void ShowLegalMoves(object? sender, List<int[]> e)
        {
            puzzleGrid.ShowLegalMoves(e);
        }
    }
}
