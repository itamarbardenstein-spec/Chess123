using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class Bishop(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        public override bool IsMoveValid(Piece[,] board, int rFrom, int cFrom, int rTo, int cTo)
        {
            if (!Inside(rTo, cTo)) return false;
            if (Math.Abs(rTo - rFrom) != Math.Abs(cTo - cFrom))
                return false;
            int dr = Math.Sign(rTo - rFrom);
            int dc = Math.Sign(cTo - cFrom);

            return PathClear(board, rFrom, cFrom, rTo, cTo, dr, dc);
        }
    }
}
