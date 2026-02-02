using Chess.Models;

namespace Chess.ModelsLogic
{
    public abstract class Piece:PieceModel
    {
        public Piece(int row, int column, bool isWhite, string? image) : base(row, column, isWhite, image) { }
        public Piece() { }
        protected override bool PathClear(Piece[,] board, int startRow, int startColumn, int endRow, int endColumn, int rowDirection, int columnDirection)
        {
            int rowCheck = startRow + rowDirection;
            int columnCheck = startColumn + columnDirection;
            while (rowCheck != endRow || columnCheck != endColumn)
            {
                if (board[rowCheck, columnCheck].StringImageSource != null)
                    return false;
                rowCheck += rowDirection;
                columnCheck += columnDirection;
            }
            Piece start = board[startRow, startColumn];
            Piece end = board[endRow, endColumn];
            if (start is King && (endColumn == 0 || endColumn == 7))
            {
                return end is Rook && end.IsWhite == start.IsWhite;
            }
            return end.StringImageSource == null || end.IsWhite != start.IsWhite;
        }
    }
}
