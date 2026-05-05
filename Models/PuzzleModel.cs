using Chess.ModelsLogic;

namespace Chess.Models
{
    /// Base logic for chess puzzles, managing validation, move tracking, and hint systems
    public abstract class PuzzleModel
    {
        #region Fields
        /// Logical 2D array representing the board state for the current puzzle
        protected Piece[,]? gameBoard;
        /// Tracks user clicks (e.g., 0 for selection, 1 for moving)
        protected int ClickCount = 0;
        /// Stores the row coordinate of the piece being moved
        protected int MoveFromRow = 0;
        /// Stores the column coordinate of the piece being moved
        protected int MoveFromColumn = 0;
        /// Stores the row coordinate of the target destination
        protected int MoveToRow = 0;
        /// Stores the column coordinate of the target destination
        protected int MoveToColumn = 0;
        /// Flag indicating if the user has successfully finished the puzzle
        protected bool solved = false;
        /// The current difficulty setting (Easy, Medium, Hard)
        protected string currentDifficulty = String.Empty;
        /// Tracks the current step number in a multi-move puzzle
        protected int moveNumber = 0;
        /// Expected target row for the correct move in the current step
        protected int CorrectMoveRow = 0;
        /// Expected target column for the correct move in the current step
        protected int CorrectMoveColumn = 0;
        /// The row coordinate of the piece that MUST be moved for the correct solution
        protected int CorrectPieceRow = 0;
        /// The column coordinate of the piece that MUST be moved for the correct solution
        protected int CorrectPieceColumn = 0;
        #endregion
        #region Events
        /// Signals the UI to remove legal move indicators
        public EventHandler? ClearLegalMovesDots;
        /// Triggered when the user performs the correct move for the current step
        public EventHandler? CorrectMove;
        /// Triggered when the final move of the puzzle is correctly completed
        public EventHandler? CorrectSolution;
        /// Signals the UI to highlight a specific square as a hint
        public EventHandler<HighlightSquareArgs>? HighlightHintSquare;
        /// Signals the UI to show both the 'from' and 'to' squares for a solution hint
        public EventHandler<DisplayMoveArgs>? HighlightCorrectMoveHint;
        /// Signals the UI to remove all active highlights
        public EventHandler? RemoveHighlight;
        /// Triggered when the user makes a move that doesn't match the puzzle solution
        public EventHandler? IncorrectMove;
        /// Requests the automated opponent to perform its programmed response move
        public EventHandler<string>? MakeOpponentMove;
        /// Passes a list of valid move coordinates to the UI for display
        public EventHandler<List<int[]>>? LegalMoves;
        /// Requests a visual update to move a piece on the board
        public EventHandler<DisplayMoveArgs>? DisplayChanged;
        #endregion
        #region Public Methods
        /// Handles the logic when a piece or square is clicked
        public abstract void OnButtonClicked(Piece p);
        /// Factory method to instantiate specific piece types at specific coordinates
        public abstract Piece CreatePiece(Piece original, int row, int col);
        /// Loads the board configuration for an Easy difficulty puzzle
        public abstract void InitEasyPuzzleBoard();
        /// Loads the board configuration for a Medium difficulty puzzle
        public abstract void InitMediumPuzzleBoard();
        /// Loads the board configuration for a Hard difficulty puzzle
        public abstract void InitHardPuzzleBoard();
        /// Executes move logic for a given coordinate
        public abstract void Play(int rowIndex, int columnIndex);
        /// Validates if the user's current move matches the puzzle's required move
        public abstract bool CheckMove();
        /// Evaluates if the total sequence of moves has completed the puzzle
        public abstract void CheckSolution();
        /// Identifies and signals which piece should be moved next
        public abstract void HintSquare();
        /// Identifies and signals both the source and destination for the next correct move
        public abstract void CorrectMoveSquares();
        #endregion
        #region Private Methods
        /// Calculates all valid moves for a piece according to chess rules
        protected abstract List<int[]> GetLegalMoveList(Piece p);
        #endregion
    }
}