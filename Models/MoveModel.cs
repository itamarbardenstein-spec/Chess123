namespace Chess.Models
{
    public abstract class MoveModel
    {
        public abstract bool IsMoveValid(PieceModel[,] board, int rFrom, int cFrom, int rTo, int cTo);
        protected abstract bool Pawn(int fr, int fc, int tr, int tc, bool w, PieceModel[,] b);
        protected abstract bool Straight(PieceModel[,] b, int r, int c, int r2, int c2);
        protected abstract bool Diagonal(PieceModel[,] b, int r, int c, int r2, int c2);
        protected abstract bool PathClear(PieceModel[,] b, int sr, int sc, int er, int ec, int dr, int dc);
        protected abstract bool Inside(int r, int c);        
    }
}
