namespace Chess.Models
{
    public abstract class MoveModel
    {
        public abstract bool IsMoveValid(Piece[,] board, int rFrom, int cFrom, int rTo, int cTo);
        protected abstract bool Pawn(int fr, int fc, int tr, int tc, bool w, Piece[,] b);
        protected abstract bool Straight(Piece[,] b, int r, int c, int r2, int c2);
        protected abstract bool Diagonal(Piece[,] b, int r, int c, int r2, int c2);
        protected abstract bool PathClear(Piece[,] b, int sr, int sc, int er, int ec, int dr, int dc);
        protected abstract bool Inside(int r, int c);        
    }
}
