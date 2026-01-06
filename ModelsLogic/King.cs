using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class King(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        public bool HasKingMoved { get; set; } = false;        
        public override bool IsMoveValid(Piece[,] board, int rFrom, int cFrom, int rTo, int cTo)
        {
            int dr = Math.Abs(rTo - rFrom);
            int dc = Math.Abs(cTo - cFrom);
            Piece king = board[rFrom, cFrom];
            Piece target = board[rTo, cTo];
            bool enemy = target.StringImageSource != null && target.IsWhite != king.IsWhite;
            if (dc == 2 && dr == 0 && !HasKingMoved)
            {
                if (cTo == 6||cTo==5)
                {
                    if (board[rFrom, 7] is Rook rightRook && !rightRook.HasRightRookMoved)
                    {
                        int step = Math.Sign(cTo - cFrom);
                        if (PathClear(board, rFrom, cFrom, rTo, cTo, 0, step))
                        {
                            return true;
                        }
                    }
                }
                else if (cTo == 2||cTo==1)
                {
                    if (board[rFrom, 0] is Rook leftRook && !leftRook.HasLeftRookMoved)
                    {
                        int step = Math.Sign(cTo - cFrom);
                        if (PathClear(board, rFrom, cFrom, rTo, cTo, 0, step))
                        {
                            return true;
                        }
                    }
                }              
            }
            if (dr <= 1 && dc <= 1)
                return target.StringImageSource == null || enemy;
            return false;
        }
    }
}
