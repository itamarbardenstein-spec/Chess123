using Chess.ModelsLogic;

namespace Chess.Models
{
    public abstract class GameGridModel:Grid
    {
        public Piece[,]? BoardPieces;
        public EventHandler<Piece>? ButtonClicked;
        protected Dictionary<(int row, int col), Piece> BoardUIMap = [];
        public abstract void InitGrid(Grid board,bool IsHostUser);
        protected abstract void OnButtonClicked(object? sender, EventArgs e);
        protected abstract void UpdateCellUI(int row, int col);
        public abstract Piece CreatePiece(Piece original, int row, int col);
        public abstract void UpdateDisplay(DisplayMoveArgs e);
        public abstract void Castling(bool right,bool isHostUser,bool MyMove);
        public abstract void Promotion(bool IsHostUser, int Row, int column);
        public abstract void ShowLegalMoves(List<int[]> legalMoves);
        public abstract void ClearDots();
        public abstract void HighlightSquare(int row, int column);
        public abstract void HighlightMove(int fromRow, int fromColumn, int toRow, int toColumn);
        public abstract void ClearBoardHighLights();
        public abstract void ClearSquareHighlight(int row, int col);
    }
}
