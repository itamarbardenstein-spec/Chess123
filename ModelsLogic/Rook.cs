using Chess.Models;

namespace Chess.ModelsLogic
{
    public class Rook:Piece
    {
        public Rook(int row, int column, bool isWhite, string? image) : base(row, column, isWhite, image) { }
        public bool rookHasMoved = false;
        public override bool IsMoveValid(Piece[,] board, int rFrom, int cFrom, int rTo, int cTo)
        {
            if (!Inside(rTo, cTo)) return false;
            if (rFrom != rTo && cFrom != cTo)
                return false;

            int dr = Math.Sign(rTo - rFrom);
            int dc = Math.Sign(cTo - cFrom);

            return PathClear(board, rFrom, cFrom, rTo, cTo, dr, dc);
        }
    }
}
