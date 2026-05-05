namespace Chess.ModelsLogic
{
    /// Logic for the Rook piece, supporting straight-line movement and tracking for castling
    public partial class Rook(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        #region Properties
        // Flags to track if specific rooks have moved, essential for validating castling rights
        public bool HasRightRookMoved { get; set; } = false;
        public bool HasLeftRookMoved { get; set; } = false;
        #endregion
        #region Public Methods
        /// Validates moves along horizontal or vertical lines and ensures the path is unobstructed
        public override bool IsMoveValid(Piece[,] board, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            bool result = false;
            // Verify the move is strictly horizontal or vertical
            if (fromRow == toRow || fromColumn == toColumn)
            {
                // Determine the movement direction (+1, -1, or 0) for rows and columns
                int rowDirection = Math.Sign(toRow - fromRow);
                int columnDirection = Math.Sign(toColumn - fromColumn);

                // Check if any pieces are positioned between the start and end coordinates
                if (PathClear(board, fromRow, fromColumn, toRow, toColumn, rowDirection, columnDirection))
                    result = true;
            }
            return result;
        }
        #endregion
    }
}