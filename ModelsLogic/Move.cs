using Chess.Models;


namespace Chess.ModelsLogic
{
    public class Move : MoveModel
    {
        public static bool IsMoveValid(Piece[,] board, int rFrom, int cFrom, int rTo, int cTo)
        {
            if (!Inside(rTo, cTo)) return false;

            Piece piece = board[rFrom, cFrom];
            if (piece == null || piece.CurrentPieceType == null)
                return false;

            bool white = piece.IsWhite;
            Piece target = board[rTo, cTo];
            bool empty = target == null || target.CurrentPieceType == null;
            bool enemy = target != null && target.IsWhite != white;

            int dr = rTo - rFrom;
            int dc = cTo - cFrom;

            return piece.CurrentPieceType switch
            {
                Piece.PieceType.Pawn => Pawn(rFrom, cFrom, rTo, cTo, white, board),
                Piece.PieceType.Rook => Straight(board, rFrom, cFrom, rTo, cTo),
                Piece.PieceType.Bishop => Diagonal(board, rFrom, cFrom, rTo, cTo),
                Piece.PieceType.Queen => Straight(board, rFrom, cFrom, rTo, cTo) ||
                                          Diagonal(board, rFrom, cFrom, rTo, cTo),
                Piece.PieceType.Knight => (Math.Abs(dr), Math.Abs(dc)) is (2, 1) or (1, 2)
                                          && (empty || enemy),
                Piece.PieceType.King => Math.Abs(dr) <= 1 && Math.Abs(dc) <= 1
                                          && (empty || enemy),
                _ => false
            };


            bool Pawn(int fr, int fc, int tr, int tc, bool w, Piece[,] b)
            {
                int dir = w ? -1 : 1;

                // צעד רגיל קדימה
                if (fc == tc && b[tr, tc] == null && tr == fr + dir)
                    return true;

                // שתי צעדים מהשורה הראשונה
                if (fc == tc && b[tr, tc] == null && b[fr + dir, fc] == null &&
                   ((w && fr == 6) || (!w && fr == 1)) && tr == fr + 2 * dir)
                    return true;

                // אכילה באלכסון
                if (Math.Abs(tc - fc) == 1 && tr == fr + dir && b[tr, tc] != null && b[tr, tc].IsWhite != w)
                    return true;

                return false;
            }


            // ============== קו ישר צריח/מלכה ==============
            bool Straight(Piece[,] b, int r, int c, int r2, int c2)
            {
                if (r != r2 && c != c2) return false;
                int dr = Math.Sign(r2 - r), dc = Math.Sign(c2 - c);
                return PathClear(b, r, c, r2, c2, dr, dc);
            }

            // ============== אלכסון רץ/מלכה ==============
            bool Diagonal(Piece[,] b, int r, int c, int r2, int c2)
            {
                if (Math.Abs(r2 - r) != Math.Abs(c2 - c)) return false;
                int dr = Math.Sign(r2 - r), dc = Math.Sign(c2 - c);
                return PathClear(b, r, c, r2, c2, dr, dc);
            }

            // בודק שהדרך פנויה (ללא קפיצה)
            bool PathClear(Piece[,] b, int sr, int sc, int er, int ec, int dr, int dc)
            {
                int x = sr + dr, y = sc + dc;
                while (x != er || y != ec)
                {
                    if (b[x, y] != null) return false;
                    x += dr; y += dc;
                }
                return b[er, ec] == null || b[er, ec].IsWhite != b[sr, sc].IsWhite;
            }

            // גבולות לוח
            bool Inside(int r, int c) => r >= 0 && r < 8 && c >= 0 && c < 8;
        }
    }
}
