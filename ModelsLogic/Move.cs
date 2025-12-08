using Chess.Models;


namespace Chess.ModelsLogic
{ }
    //public class Move : MoveModel
    //{
//        public override bool IsMoveValid(PieceModel[,] board, int rFrom, int cFrom, int rTo, int cTo)
//        {
//            if (!Inside(rTo, cTo)) return false;

//            PieceModel piece = board[rFrom, cFrom];
//            if (piece == null || piece.CurrentPieceType == null)
//                return false;

//            bool white = piece.IsWhite;
//            PieceModel target = board[rTo, cTo];
//            bool empty = target == null || target.CurrentPieceType == null;
//            bool enemy = target != null && target.IsWhite != white;
//            int dr = rTo - rFrom;
//            int dc = cTo - cFrom;           
//            if (piece.CurrentPieceType == PieceModel.PieceType.King)
//            {
//                if (!piece.HasMoved && dr == 0 && Math.Abs(dc) == 2)
//                {
//                    int row = rFrom;

//                    if (dc == 2)
//                    {
//                        PieceModel rook = board[row, 7];
//                        if (rook != null && rook.CurrentPieceType == PieceModel.PieceType.Rook && !rook.HasMoved)
//                        {
//                            if (board[row, 5] == null && board[row, 6] == null)
//                                return true;
//                        }
//                    }
//                    else if (dc == -2)
//                    {
//                        PieceModel rook = board[row, 0];
//                        if (rook != null && rook.CurrentPieceType == PieceModel.PieceType.Rook && !rook.HasMoved)
//                        {
//                            if (board[row, 1] == null && board[row, 2] == null && board[row, 3] == null)
//                                return true;
//                        }
//                    }
//                }
//                return Math.Abs(dr) <= 1 && Math.Abs(dc) <= 1 && (empty || enemy);
//            }
//            return piece.CurrentPieceType switch
//            {
//                PieceModel.PieceType.Pawn => Pawn(rFrom, cFrom, rTo, cTo, white, board),
//                PieceModel.PieceType.Rook => Straight(board, rFrom, cFrom, rTo, cTo),
//                PieceModel.PieceType.Bishop => Diagonal(board, rFrom, cFrom, rTo, cTo),
//                PieceModel.PieceType.Queen => Straight(board, rFrom, cFrom, rTo, cTo) ||
//                                          Diagonal(board, rFrom, cFrom, rTo, cTo),
//                PieceModel.PieceType.Knight => (Math.Abs(dr), Math.Abs(dc)) is (2, 1) or (1, 2)
//                                          && (empty || enemy),
//                _ => false
//            };
//        }
//        protected override bool Pawn(int fr, int fc, int tr, int tc, bool w, PieceModel[,] b)
//        {
//            int dir = w ? -1 : 1;
//            if (fc == tc && b[tr, tc] == null && tr == fr + dir)
//                return true;
//            if (fc == tc && b[tr, tc] == null && b[fr + dir, fc] == null &&
//               ((w && fr == 6) || (!w && fr == 1)) && tr == fr + 2 * dir)
//                return true;
//            if (Math.Abs(tc - fc) == 1 && tr == fr + dir && b[tr, tc] != null && b[tr, tc].IsWhite != w)
//                return true;

//            return false;
//        }
//        protected override bool Straight(PieceModel[,] b, int r, int c, int r2, int c2)
//        {
//            if (r != r2 && c != c2) return false;
//            int dr = Math.Sign(r2 - r), dc = Math.Sign(c2 - c);
//            return PathClear(b, r, c, r2, c2, dr, dc);
//        }
//        protected override bool Diagonal(PieceModel[,] b, int r, int c, int r2, int c2)
//        {
//            if (Math.Abs(r2 - r) != Math.Abs(c2 - c)) return false;
//            int dr = Math.Sign(r2 - r), dc = Math.Sign(c2 - c);
//            return PathClear(b, r, c, r2, c2, dr, dc);
//        }
//        protected override bool PathClear(PieceModel[,] board, int startRow, int startCollomn, int endRow, int endCollomn, int directionRow, int directionCollomn)
//        {
//            int x = startRow + directionRow, y = startCollomn + directionCollomn;
//            while (x != endRow || y != endCollomn)
//            {
//                if (board[x, y] != null) return false;
//                x += directionRow; y += directionCollomn;
//            }
//            return board[endRow, endCollomn] == null || board[endRow, endCollomn].IsWhite != board[startRow, startCollomn].IsWhite;
//        }
     
//        protected override bool Inside(int r, int c)
//        {
//            return r >= 0 && r < 8 && c >= 0 && c < 8;           
//        }
//    }
//}
