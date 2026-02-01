using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class King(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        public bool HasKingMoved { get; set; } = false;
        public override bool IsMoveValid(Piece[,] board, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            int rowsMoved = Math.Abs(toRow - fromRow);
            int columnsMoved = Math.Abs(toColumn - fromColumn);
            Piece king = board[fromRow, fromColumn];
            bool result = false;
            if (columnsMoved == 2 && rowsMoved == 0 && !HasKingMoved)
            {
                int step = Math.Sign(toColumn - fromColumn);
                if (toColumn == 6 || toColumn == 5)
                    if (board[fromRow, 7] is Rook rightRook && !rightRook.HasRightRookMoved)
                        if (PathClear(board, fromRow, fromColumn, fromRow, 7, 0, step))
                            result= true;
                else if (toColumn == 2 || toColumn == 1)
                    if (board[fromRow, 0] is Rook leftRook && !leftRook.HasLeftRookMoved)
                        if (PathClear(board, fromRow, fromColumn, fromRow, 0, 0, step))
                            result= true;
            }
            else if( rowsMoved <= 1 && columnsMoved <= 1 && (board[toRow, toColumn].StringImageSource == null || board[toRow, toColumn].IsWhite != king.IsWhite))
                result= true;
            return result;
        }
    }
}
