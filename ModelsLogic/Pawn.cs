using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class Pawn(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        public override bool IsMoveValid(Piece[,] board, int fr, int fc, int tr, int tc)
        {
            Piece pawn = board[fr, fc];
            bool white = pawn.IsWhite;
            Piece target = board[tr, tc];
            if (fc == tc && target.StringImageSource== null && tr == fr - 1)
                return true;
            if (fc == tc && board[tr, tc].StringImageSource == null && board[fr - 1, fc].StringImageSource == null && tr == fr - 2)
                return true;
            if (Math.Abs(tc - fc) == 1 && tr == fr - 1 && target?.StringImageSource != null && target.IsWhite != white)
                return true;
            return false;
        }
    }
}
