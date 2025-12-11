using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class King(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        private bool KingHasMoved = false;
        public override bool IsMoveValid(Piece[,] board, int rFrom, int cFrom, int rTo, int cTo)
        {
            if (!Inside(rTo, cTo)) return false;
            int dr = Math.Abs(rTo - rFrom);
            int dc = Math.Abs(cTo - cFrom);
            Piece king = board[rFrom, cFrom];
            Piece target = board[rTo, cTo];
            bool enemy = target != null && target.IsWhite != king.IsWhite;
            if (dr <= 1 && dc <= 1)
                return target == null || enemy;
            if (!KingHasMoved && dr == 0 && Math.Abs(dc) == 2)
            {
                int row = rFrom;
                if (dc == 2)
                {
                    Rook rook = (Rook)board[row, 7];
                    if (rook != null && !rook.rookHasMoved)
                        return board[row, 5] == null && board[row, 6] == null;
                }
                else if (dc == -2)
                {
                    Rook rook = (Rook)board[row, 0];
                    if (rook != null && !rook.rookHasMoved)
                        return board[row, 1] == null &&
                               board[row, 2] == null &&
                               board[row, 3] == null;
                }
            }
            return false;
        }
    }
}
