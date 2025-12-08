using Chess.Models;

namespace Chess.ModelsLogic
{
    public class Pawn:Piece
    {
        public Pawn(int row, int column, bool isWhite, string? image):base(row, column, isWhite, image) { }
        public override bool IsMoveValid(Piece[,] board, int fr, int fc, int tr, int tc)
        {
            if (!Inside(tr, tc)) return false;
            Piece pawn = board[fr, fc];
            bool white = pawn.IsWhite;
            int dir = white ? -1 : 1;
            Piece target = board[tr, tc];
            // צעד אחד קדימה
            if (fc == tc && target == null && tr == fr + dir)
                return true;
            // צעד פתיחה כפול
            if (fc == tc &&
                target == null &&
                board[fr + dir, fc] == null &&
                ((white && fr == 6) || (!white && fr == 1)) &&
                tr == fr + 2 * dir)
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
