namespace Chess.ModelsLogic
{
    /// Represents the Bishop piece logic, specifically handling its diagonal movement patterns
    public partial class Bishop(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        /// Validates whether a move is legal for a Bishop based on diagonal trajectory and path clearance
        public override bool IsMoveValid(Piece[,] board, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            bool result = false;

            if (Math.Abs(toRow - fromRow) == Math.Abs(toColumn - fromColumn))
            {
                int rowDirection = Math.Sign(toRow - fromRow);
                int columnDirection = Math.Sign(toColumn - fromColumn);

                if (PathClear(board, fromRow, fromColumn, toRow, toColumn, rowDirection, columnDirection))
                {
                    result = true;
                }
            }

            return result;
        }
    }
}