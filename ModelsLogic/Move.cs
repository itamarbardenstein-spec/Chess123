using Chess.Models;


namespace Chess.ModelsLogic
{
    public class Move:MoveModel
    {
        private Game game = new();
        public bool IsMoveValid(int fromRow, int fromCol, int toRow, int toCol)
        {
            
            if (game.BoardPieces?[fromRow, fromCol] == null)
                return false;

            var piece = game.BoardPieces?[fromRow, fromCol];
            var target = game.BoardPieces?[toRow, toCol];

            // אי אפשר לאכול כלי של אותו צבע
            if (target != null && target.IsWhite == piece.IsWhite)
                return false;

            int rowDiff = toRow - fromRow;
            int colDiff = toCol - fromCol;

            switch (piece.CurrentPieceType)
            {
                case Piece.PieceType.Pawn:
                    return IsPawnMoveValid(piece, fromRow, fromCol, toRow, toCol);

                case Piece.PieceType.Rook:
                    return IsRookMoveValid(fromRow, fromCol, toRow, toCol);

                case Piece.PieceType.Knight:
                    return IsKnightMoveValid(rowDiff, colDiff);

                case Piece.PieceType.Bishop:
                    return IsBishopMoveValid(fromRow, fromCol, toRow, toCol);

                case Piece.PieceType.Queen:
                    return IsQueenMoveValid(fromRow, fromCol, toRow, toCol);

                case Piece.PieceType.King:
                    return IsKingMoveValid(rowDiff, colDiff);

                default:
                    return false;
            }
        }
        private bool IsPawnMoveValid(Piece pawn, int fromRow, int fromCol, int toRow, int toCol)
        {
            int direction = pawn.IsWhite ? -1 : 1; // לבן עולה למעלה, שחור למטה

            // צעד אחד קדימה ריק
            if (toCol == fromCol && game.BoardPieces?[toRow, toCol] == null && toRow - fromRow == direction)
                return true;

            // צעד שני מהשורה הראשונה
            if (toCol == fromCol && game.BoardPieces?[toRow, toCol] == null &&
                ((pawn.IsWhite && fromRow == 6) || (!pawn.IsWhite && fromRow == 1)) &&
                toRow - fromRow == 2 * direction &&
                game.BoardPieces?[fromRow + direction, fromCol] == null)
                return true;

            // אכילה אלכסונית
            if (Math.Abs(toCol - fromCol) == 1 && toRow - fromRow == direction &&
                game.BoardPieces?[toRow, toCol] != null)
                return true;

            return false;
        }

        private bool IsRookMoveValid(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (fromRow != toRow && fromCol != toCol) return false;

            int rowStep = Math.Sign(toRow - fromRow);
            int colStep = Math.Sign(toCol - fromCol);

            int r = fromRow + rowStep;
            int c = fromCol + colStep;

            while (r != toRow || c != toCol)
            {
                if (game.BoardPieces?[r, c] != null) return false;
                r += rowStep;
                c += colStep;
            }
            return true;
        }

        private bool IsBishopMoveValid(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (Math.Abs(toRow - fromRow) != Math.Abs(toCol - fromCol)) return false;

            int rowStep = Math.Sign(toRow - fromRow);
            int colStep = Math.Sign(toCol - fromCol);

            int r = fromRow + rowStep;
            int c = fromCol + colStep;

            while (r != toRow && c != toCol)
            {
                if (game.BoardPieces?[r, c] != null) return false;
                r += rowStep;
                c += colStep;
            }
            return true;
        }

        private bool IsKnightMoveValid(int rowDiff, int colDiff)
        {
            return (Math.Abs(rowDiff) == 2 && Math.Abs(colDiff) == 1) ||
                   (Math.Abs(rowDiff) == 1 && Math.Abs(colDiff) == 2);
        }

        private bool IsQueenMoveValid(int fromRow, int fromCol, int toRow, int toCol)
        {
            return IsRookMoveValid(fromRow, fromCol, toRow, toCol) ||
                   IsBishopMoveValid(fromRow, fromCol, toRow, toCol);
        }

        private bool IsKingMoveValid(int rowDiff, int colDiff)
        {
            return Math.Abs(rowDiff) <= 1 && Math.Abs(colDiff) <= 1;
        }
    }
}
