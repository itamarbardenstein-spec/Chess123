using Chess.ModelsLogic;

namespace Chess.Models
{
    /// Base model for puzzle-specific game grids, supporting different difficulty levels and hint systems
    public abstract class PuzzleGridModel : Grid
    {
        #region Fields
        /// Logical state of pieces on the puzzle board
        protected Piece[,]? BoardPieces;
        /// Tracks the number of moves made by the user in the current puzzle session
        protected int moveCount = 0;
        /// Maps physical coordinates to the UI Piece objects for direct manipulation
        protected Dictionary<(int row, int col), Piece> BoardUIMap = [];
        #endregion

        #region Events
        /// Event fired when a user interacts with a piece or square during a puzzle
        public EventHandler<Piece>? ButtonClicked;
        #endregion

        #region Public Methods
        /// Set up the board with a predefined 'Easy' difficulty chess puzzle
        public abstract void InitEasyPuzzleGrid(Grid board);
        /// Set up the board with a predefined 'Medium' difficulty chess puzzle
        public abstract void InitMediumPuzzleGrid(Grid board);
        /// Set up the board with a predefined 'Hard' difficulty chess puzzle
        public abstract void InitHardPuzzleGrid(Grid board);

        public abstract Piece CreatePiece(Piece original, int row, int col);
        public abstract void UpdateDisplay(DisplayMoveArgs e);

        /// Executes the automated response move for the opponent based on puzzle logic
        public abstract void MakeOpponentMove(string difficulty);
        public abstract void ShowLegalMoves(List<int[]> legalMoves);
        public abstract void ClearDots();

        /// Highlights a specific piece or square to provide a hint to the user
        public abstract void ShowHint(int row, int column);
        /// Visually demonstrates the correct sequence for the puzzle solution
        public abstract void ShowCorrectMove(int fromRow, int fromColumn, int toRow, int toColumn);
        public abstract void ClearBoardHighLights();
        #endregion

        #region Private Methods
        protected abstract void OnButtonClicked(object? sender, EventArgs e);
        protected abstract void UpdateCellUI(int row, int col);
        #endregion
    }
}