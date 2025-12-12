using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class Knight(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        public override bool IsMoveValid(Piece[,] board, int rFrom, int cFrom, int rTo, int cTo)
        {
            int dr = Math.Abs(rTo - rFrom);
            int dc = Math.Abs(cTo - cFrom);
            if (!((dr == 2 && dc == 1) || (dr == 1 && dc == 2)))
                return false;
            Piece start = board[rFrom, cFrom];
            Piece target = board[rTo, cTo];
            return target.StringImageSource == null || start.IsWhite != target.IsWhite;
        }
    }
}
