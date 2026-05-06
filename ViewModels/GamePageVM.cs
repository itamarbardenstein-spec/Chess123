using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace Chess.ViewModels
{
    /// ViewModel for the main game page, coordinating between the game logic and the UI board
    public partial class GamePageVM : ObservableObject
    {
        #region Fields
        private readonly GameGrid grdBoard = [];
        private readonly Game game;
        #endregion
        #region Commands
        /// Command to allow the current player to forfeit the game
        public ICommand ResignCommand { get; set; }
        #endregion
        #region Properties
        public string MyName => game.MyName;
        public string StatusMessage => game.StatusMessage;
        public List<CapturedPieceGroup>? OpponentCapturedPieces => game.GetGroupedCapturedPieces(false);
        public List<CapturedPieceGroup>? MyCapturedPieces => game.GetGroupedCapturedPieces(true);
        // Returns formatted time strings based on whether the user is the host or guest
        public string MyTime => game.IsHostUser ? FormatTime(game.BlackTimeLeft) : FormatTime(game.WhiteTimeLeft);
        public string OpponentTime => game.IsHostUser ? FormatTime(game.WhiteTimeLeft) : FormatTime(game.BlackTimeLeft);
        public string OpponentName => game.OpponentName;
        #endregion
        #region Constructor
        /// Initializes the ViewModel, hooks into game events, and prepares the board grid
        public GamePageVM(Game game, Grid board)
        {
            this.game = game;
            ResignCommand = new Command(ResignGame);
            // Wire up logic events to UI update methods
            game.OnGameChanged += OnGameChanged;
            game.OnGameDeleted += OnGameDeleted;
            game.LegalMoves += ShowLegalMoves;
            game.OnPromotion += Promotion;
            game.ClearBoardHighLights += ClearBoardHighlights;
            game.DisplayChanged += OnDisplayChanged;
            game.ClearLegalMovesDots += ClearDots;
            game.OnCastling += Castling;
            game.HighlightSquare += HighlightSquare;
            game.ClearSquareHighLight += ClearSquareHighlight;
            game.TimeLeftChanged += OnTimeLeftChanged;
            game.GameOver += OnGameOver;
            grdBoard.InitGrid(board, game.IsHostUser);
            grdBoard.ButtonClicked += OnButtonClicked;
            // Sync guest user status with the remote game document
            if (!game.IsHostUser)
                game.UpdateGuestUser(OnComplete);
        }
        #endregion
        #region Public Methods
        /// Enables real-time synchronization with the Firebase database
        public void AddSnapshotListener()
        {
            game.AddSnapshotListener();
        }
        /// Disables database synchronization to save resources when the page is inactive
        public void RemoveSnapshotListener()
        {
            game.RemoveSnapshotListener();
        }
        #endregion
        #region Private Methods
        /// Converts millisecond values into a standard MM:SS clock format
        private static string FormatTime(long millis)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(millis);
            return $"{t.Minutes:D2}:{t.Seconds:D2}";
        }
        /// Resets all board cell backgrounds to their default state
        private void ClearBoardHighlights(object? sender, EventArgs e)
        {
            grdBoard.ClearBoardHighLights();
        }
        /// Triggers the resignation logic within the game model
        private void ResignGame(object obj)
        {
            game.ResignGame();
        }
        /// Removes the highlight from a specific square
        private void ClearSquareHighlight(object? sender, HighlightSquareArgs e)
        {
            grdBoard.ClearSquareHighlight(e.Row, e.Column);
        }
        /// Applies a visual highlight to a specific square, such as for a check or hint
        private void HighlightSquare(object? sender, HighlightSquareArgs e)
        {
            grdBoard.HighlightSquare(e.Row, e.Column);
        }
        /// Clears the dots indicating legal move options
        private void ClearDots(object? sender, EventArgs e)
        {
            grdBoard.ClearDots();
        }
        /// Displays visual dots on all squares where the selected piece can legally move
        private void ShowLegalMoves(object? sender, List<int[]> e)
        {
            grdBoard.ShowLegalMoves(e);
        }
        /// Initiates the pawn promotion UI sequence
        private void Promotion(object? sender, OnPromotionArgs e)
        {
            grdBoard.Promotion(e.IsHostUser, e.Row, e.Column);
        }
        /// Handles the specialized visual movement of pieces during a castling move
        private void Castling(object? sender, CastlingArgs e)
        {
            grdBoard?.Castling(e.Right, e.IsHostUser, e.MyMove);
        }
        /// Displays the final result popup when the game ends
        private void OnGameOver(object? sender, GameOverArgs e)
        {
            string reason = game.GameOverMessageReason(e.IWon, e.Reason);
            string title = game.GameOverMessageTitle(e.IWon, e.Reason);
            Shell.Current.ShowPopup(new GameResultPopup(title, reason));
        }
        /// Updates the UI timer display when the time values change
        private void OnTimeLeftChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(MyTime));
        }
        /// Notifies the user via toast if the game document has been removed from the server
        private void OnGameDeleted(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.GameDeleted, ToastDuration.Long).Show();
            });
        }
        /// Updates the piece positions on the UI board
        private void OnDisplayChanged(object? sender, DisplayMoveArgs e)
        {
            grdBoard.UpdateDisplay(e);
        }
        /// Forwards board click events to the game logic for processing
        private void OnButtonClicked(object? sender, Piece e)
        {
            game.CheckMove(e);
        }
        /// Refreshes all relevant UI properties when the game state changes
        private void OnGameChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(OpponentName));
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(OpponentTime));
            OnPropertyChanged(nameof(MyTime));
            OnPropertyChanged(nameof(MyCapturedPieces));
            OnPropertyChanged(nameof(OpponentCapturedPieces));
        }
        /// Handles error reporting for guest users failing to join a game
        private void OnComplete(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameErr, ToastDuration.Long).Show();
        }
        #endregion
    }
}