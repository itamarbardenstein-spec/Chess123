using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class King(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        public override bool IsMoveValid(Piece[,] board, int rFrom, int cFrom, int rTo, int cTo)
        { 
            int dr = Math.Abs(rTo - rFrom);
            int dc = Math.Abs(cTo - cFrom);
            Piece king = board[rFrom, cFrom];
            Piece target = board[rTo, cTo];
            bool enemy = target.StringImageSource != null && target.IsWhite != king.IsWhite;
            if (dr <= 1 && dc <= 1)
                return target.StringImageSource == null || enemy;           
            return false;
        }
    }
}
