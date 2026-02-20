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
        #region Fields
        private readonly PuzzleGrid puzzleGrid = [];
        private readonly Puzzle puzzle;
        #endregion
        #region Commands
        public ICommand ShowHintCommand { get; private set; }
        public ICommand HomeCommand { get; private set; }
        public ICommand ShowMoveCommand { get; private set; }
        #endregion
        #region Constructor
        public PuzzlePageVM(Grid puzzleBoard,string difficulty)
        {
            puzzle = new Puzzle(difficulty);
            if (difficulty==Strings.Easy)
                puzzleGrid.InitEasyPuzzleGrid(puzzleBoard);
            else if(difficulty==Strings.Medium)
                puzzleGrid.InitMediumPuzzleGrid(puzzleBoard);
            else
                puzzleGrid.InitHardPuzzleGrid(puzzleBoard);
            puzzleGrid.ButtonClicked += OnButtonClicked;
            puzzle.LegalMoves += ShowLegalMoves;
            puzzle.DisplayChanged += OnDisplayChanged;
            puzzle.MakeOpponentMove += MakeOpponentMove;
            puzzle.ClearLegalMovesDots += ClearDots;
            puzzle.HighlightCorrectMoveHint += HighlightCorrectMove;
            puzzle.HighlightHintSquare += HighlightHintSquare;
            puzzle.CorrectMove += OnCorrectMove;
            puzzle.IncorrectMove += OnIncorrectMove;
            puzzle.RemoveHighlight += ClearHighlights;
            puzzle.CorrectSolution += OnCorrectSolution;
            ShowHintCommand = new Command(ShowHint);
            HomeCommand = new Command(TransferHome);
            ShowMoveCommand = new Command(ShowCorrectMove);
        }
        #endregion
        #region Private Methods
        private void ClearHighlights(object? sender, EventArgs e)
        {
            puzzleGrid.ClearBoardHighLights();
        }
        private void HighlightHintSquare(object? sender, HighlightSquareArgs e)
        {
            puzzleGrid.ShowHint(e.Row,e.Column);
        }
        private void HighlightCorrectMove(object? sender, DisplayMoveArgs e)
        {
            puzzleGrid.ShowCorrectMove(e.FromRow,e.FromColomn,e.ToRow,e.ToColumn);
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
            puzzle.CorrectMoveSquares();
        }
        private void TransferHome(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new HomePage();
            });
        }
        private void ShowHint(object obj)
        {
            puzzle.HintSquare();
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
            puzzle.OnButtonClicked(e);
        }
        private void ClearDots(object? sender, EventArgs e)
        {
            puzzleGrid.ClearDots();
        }
        private void ShowLegalMoves(object? sender, List<int[]> e)
        {
            puzzleGrid.ShowLegalMoves(e);
        }
        #endregion
    }
}
