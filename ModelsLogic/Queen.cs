namespace Chess.ModelsLogic
{
    /// Logic for the Queen piece, combining Rook and Bishop movement
    public partial class Queen(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        /// Validates move based on straight lines or diagonals and checks for path obstructions
        public override bool IsMoveValid(Piece[,] board, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            bool result = false;
            // Check if the target is on a straight line (Rook-like) or a diagonal (Bishop-like)
            bool straight = fromRow == toRow || fromColumn == toColumn;
            bool diagonal = Math.Abs(toRow - fromRow) == Math.Abs(toColumn - fromColumn);

            if (straight || diagonal)
            {
                // Determine the step direction for row and column movement
                int rowDirection = Math.Sign(toRow - fromRow);
                int columnDirection = Math.Sign(toColumn - fromColumn);

                // Verify no other pieces are blocking the path to the target square
                if (PathClear(board, fromRow, fromColumn, toRow, toColumn, rowDirection, columnDirection))
                    result = true;
            }
            return result;
        }
    }
}