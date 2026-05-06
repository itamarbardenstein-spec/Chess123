using System.Windows.Input;
using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace Chess.ViewModels
{
    /// ViewModel for the puzzle challenge page, managing single-player logic and board interactions
    public partial class PuzzlePageVM
    {
        #region Fields
        /// Handles the visual representation of the board specifically for puzzle scenarios
        private readonly PuzzleGrid puzzleGrid = [];
        /// Core logic for puzzle state, validation, and solution tracking
        private readonly Puzzle puzzle;
        #endregion
        #region Commands
        /// Highlights the correct piece that needs to be moved
        public ICommand ShowHintCommand { get; private set; }
        /// Returns the user to the application's home screen
        public ICommand HomeCommand { get; private set; }
        /// Reveals the complete correct move (from and to squares)
        public ICommand ShowMoveCommand { get; private set; }
        #endregion
        #region Constructor
        /// Initializes the puzzle based on difficulty and connects logic events to UI updates
        public PuzzlePageVM(Grid puzzleBoard, string difficulty)
        {
            puzzle = new Puzzle(difficulty);
            // Initialize board grid based on selected difficulty level
            if (difficulty == Strings.Easy)
                puzzleGrid.InitEasyPuzzleGrid(puzzleBoard);
            else if (difficulty == Strings.Medium)
                puzzleGrid.InitMediumPuzzleGrid(puzzleBoard);
            else
                puzzleGrid.InitHardPuzzleGrid(puzzleBoard);
            // Wire up puzzle logic events
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
        /// Clears all visual highlights from the board squares
        private void ClearHighlights(object? sender, EventArgs e)
        {
            puzzleGrid.ClearBoardHighLights();
        }
        /// Highlights a specific square to nudge the user toward the solution
        private void HighlightHintSquare(object? sender, HighlightSquareArgs e)
        {
            puzzleGrid.ShowHint(e.Row, e.Column);
        }
        /// Visually demonstrates the correct move coordinates on the grid
        private void HighlightCorrectMove(object? sender, DisplayMoveArgs e)
        {
            puzzleGrid.ShowCorrectMove(e.FromRow, e.FromColomn, e.ToRow, e.ToColumn);
        }
        /// Executes the predefined counter-move from the puzzle's "opponent"
        private void MakeOpponentMove(object? sender, string e)
        {
            puzzleGrid.MakeOpponentMove(e);
        }
        /// Shows a success popup when the entire puzzle sequence is solved correctly
        private void OnCorrectSolution(object? sender, EventArgs e)
        {
            string reason = Strings.CorrectSolutionMessage;
            string title = Strings.CorrectSolutionTitle;
            Application.Current?.MainPage?.ShowPopup(new CorrectMovePopup(title, reason));
        }
        /// Refreshes the piece positions on the board after a move
        private void OnDisplayChanged(object? sender, DisplayMoveArgs e)
        {
            puzzleGrid.UpdateDisplay(e);
        }
        /// Requests the correct move data from the puzzle logic
        private void ShowCorrectMove(object obj)
        {
            puzzle.CorrectMoveSquares();
        }
        /// Navigates back to the main home screen
        private void TransferHome(object obj)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                if (Application.Current != null)
                    Application.Current.MainPage = new HomePage();
            });
        }
        /// Requests a hint for the starting square from the puzzle logic
        private void ShowHint(object obj)
        {
            puzzle.HintSquare();
        }
        /// Alerts the user that the attempted move is not part of the solution
        private void OnIncorrectMove(object? sender, EventArgs e)
        {
            Toast.Make(Strings.IncorrectMoveMessage, ToastDuration.Short).Show();
        }
        /// Confirms to the user that they made the right move in the sequence
        private void OnCorrectMove(object? sender, EventArgs e)
        {
            Toast.Make(Strings.CorrectMove, ToastDuration.Short).Show();
        }
        /// Passes board interactions to the puzzle engine for validation
        private void OnButtonClicked(object? sender, Piece e)
        {
            puzzle.OnButtonClicked(e);
        }
        /// Removes dots indicating legal moves from the board
        private void ClearDots(object? sender, EventArgs e)
        {
            puzzleGrid.ClearDots();
        }
        /// Displays visual indicators for all possible legal moves for the selected piece
        private void ShowLegalMoves(object? sender, List<int[]> e)
        {
            puzzleGrid.ShowLegalMoves(e);
        }
        #endregion
    }
}