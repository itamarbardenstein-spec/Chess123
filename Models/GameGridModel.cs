using Chess.ModelsLogic;

namespace Chess.Models
{
    public abstract class GameGridModel:Grid
    {
        protected Grid? GameBoard;
        public Piece[,]? BoardPieces;
        public EventHandler<Piece>? ButtonClicked;
        protected Dictionary<(int row, int col), PieceModel> BoardUIMap = [];
        public abstract void InitGrid(Grid board,bool IsHostUser);
        protected abstract void OnButtonClicked(object? sender, EventArgs e);
        protected abstract void UpdateCellUI(int row, int col);
        public abstract Piece CreatePiece(Piece original, int row, int col);
        public abstract void UpdateDisplay(DisplayMoveArgs e);
        public abstract void Castling(bool right,bool isHostUser,bool MyMove);

    }
}
