using Chess.ModelsLogic;

namespace Chess.Models
{
    /// Base model for the game grid UI, managing the visual representation of the chessboard
    public abstract class GameGridModel : Grid
    {
        #region Fields
        /// Internal logic representation of pieces in a 2D array
        protected Piece[,]? BoardPieces;
        /// Maps board coordinates (row, col) to their corresponding UI Piece objects
        protected Dictionary<(int row, int col), Piece> BoardUIMap = [];
        #endregion
        #region Events
        /// Triggered when a board square is interacted with by the user
        public EventHandler<Piece>? ButtonClicked;
        #endregion
        #region Public Methods
        public abstract void InitGrid(Grid board, bool IsHostUser);
        public abstract Piece CreatePiece(Piece original, int row, int col);
        public abstract void UpdateDisplay(DisplayMoveArgs e);
        public abstract void Castling(bool right, bool isHostUser, bool MyMove);
        public abstract void Promotion(bool IsHostUser, int Row, int column);
        public abstract void ShowLegalMoves(List<int[]> legalMoves);
        public abstract void ClearDots();
        public abstract void HighlightSquare(int row, int column);
        public abstract void HighlightMove(int fromRow, int fromColumn, int toRow, int toColumn);
        public abstract void ClearBoardHighLights();
        public abstract void ClearSquareHighlight(int row, int col);
        #endregion
        #region Private Methods
        protected abstract void OnButtonClicked(object? sender, EventArgs e);
        protected abstract void UpdateCellUI(int row, int col);
        #endregion
    }
}