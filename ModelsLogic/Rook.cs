using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class Rook(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        //public bool rookHasMoved = false;
        public override bool IsMoveValid(Piece[,] board, int rFrom, int cFrom, int rTo, int cTo)
        {          
            if (rFrom != rTo && cFrom != cTo)
                return false;
            int dr = Math.Sign(rTo - rFrom);
            int dc = Math.Sign(cTo - cFrom);
            return PathClear(board, rFrom, cFrom, rTo, cTo, dr, dc);
        }
    }
}
