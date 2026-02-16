using Chess.Models;

namespace Chess.ModelsLogic
{
    public abstract class Piece:PieceModel
    {
        public Piece(int row, int column, bool isWhite, string? image) : base(row, column, isWhite, image) { }
        public Piece() { }
        protected override bool PathClear(Piece[,] board, int startRow, int startColumn, int endRow, int endColumn, int rowDirection, int columnDirection)
        {
            bool result = true;
            int rowCheck = startRow + rowDirection;
            int columnCheck = startColumn + columnDirection;
            while (rowCheck != endRow || columnCheck != endColumn)
            {
                if (board[rowCheck, columnCheck].StringImageSource != null)
                    result = false;
                rowCheck += rowDirection;
                columnCheck += columnDirection;
            }
            if (result)
            {
                Piece start = board[startRow, startColumn];
                Piece end = board[endRow, endColumn];
                if (start is King && (endColumn == 0 || endColumn == 7))
                {
                    result = end is Rook && end.IsWhite == start.IsWhite;
                }
                else
                    result = end.StringImageSource == null || end.IsWhite != start.IsWhite;
            }  
            return result;
        }
    }
}
