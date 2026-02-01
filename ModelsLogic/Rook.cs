using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class Rook(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        bool result = false;
        public bool HasRightRookMoved { get; set; } = false;
        public bool HasLeftRookMoved { get; set; } = false;
        public override bool IsMoveValid(Piece[,] board, int fromRow, int fromColumn, int toRow, int toColumn)
        {          
            if (fromRow == toRow || fromColumn == toColumn)
            {
                int dr = Math.Sign(toRow - fromRow);
                int dc = Math.Sign(toColumn - fromColumn);
                if(PathClear(board, fromRow, fromColumn, toRow, toColumn, dr, dc))
                    result= true;
            }
            return result;
        }
    }
}
