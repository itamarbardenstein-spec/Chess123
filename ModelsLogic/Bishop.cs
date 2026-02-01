using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class Bishop(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        public override bool IsMoveValid(Piece[,] board, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            bool result = false;
            if (Math.Abs(toRow - fromRow) == Math.Abs(toColumn - fromColumn))
            {
                int rowDirection = Math.Sign(toRow - fromRow);
                int columnDirection = Math.Sign(toColumn - fromColumn);
                if(PathClear(board, fromRow, fromColumn, toRow, toColumn, rowDirection, columnDirection))
                    result=true;
            }
            return result;
        }
    }
}
