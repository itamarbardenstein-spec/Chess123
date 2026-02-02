using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class Rook(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {     
        public bool HasRightRookMoved { get; set; } = false;
        public bool HasLeftRookMoved { get; set; } = false;
        public override bool IsMoveValid(Piece[,] board, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            bool result = false;
            if (fromRow == toRow || fromColumn == toColumn)
            {
                int rowDirection = Math.Sign(toRow - fromRow);
                int columnDirection = Math.Sign(toColumn - fromColumn);
                if(PathClear(board, fromRow, fromColumn, toRow, toColumn, rowDirection, columnDirection))
                    result= true;
            }
            return result;
        }
    }
}
