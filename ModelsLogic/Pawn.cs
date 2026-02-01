using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class Pawn(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        public override bool IsMoveValid(Piece[,] board, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            Piece pawn = board[fromRow, fromColumn];
            bool white = pawn.IsWhite;
            Piece target = board[toRow, toColumn];
            bool result = false;
            if (fromColumn == toColumn && target.StringImageSource== null && toRow == fromRow - 1)
                result= true;
            else if (fromRow == 6&& fromColumn == toColumn && target.StringImageSource == null && board[fromRow - 1, fromColumn].StringImageSource == null && toRow == fromRow - 2)
                result= true;
            else if (Math.Abs(toColumn - fromColumn) == 1 && toRow == fromRow - 1 && target?.StringImageSource != null && target.IsWhite != white)
                result= true;
            return result;
        }
    }
}
