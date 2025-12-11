using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class Pawn(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        public override bool IsMoveValid(Piece[,] board, int fr, int fc, int tr, int tc)
        {
            if (!Inside(tr, tc)) return false;
            Piece pawn = board[fr, fc];
            bool white = pawn.IsWhite;
            int dir = 1;
            Piece target = board[tr, tc];
            // צעד אחד קדימה
            if (fc == tc && target == null && tr == fr + dir)
                return true;
            // צעד פתיחה כפול
            if (fc == tc && board[tr, tc] == null && board[fr - dir, fc] == null && tr == fr - 2)
                return true;
            // אכילה באלכסון
            if (Math.Abs(tc - fc) == 1 &&
                tr == fr + dir &&
                target != null &&
                target.IsWhite != white)
                return true;
            return false;
        }
    }
}
