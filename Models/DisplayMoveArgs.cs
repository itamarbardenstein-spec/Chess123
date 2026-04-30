namespace Chess.Models
{
    /// Represents the arguments for a move display event, containing the starting and ending coordinates
    public class DisplayMoveArgs(int fromRow, int fromColumn, int toRow, int toColumn) : EventArgs
    {
        /// The starting row index of the piece
        public int FromRow { get; set; } = fromRow;
        /// The starting column index of the piece
        public int FromColomn { get; set; } = fromColumn;
        /// The target row index for the move
        public int ToRow { get; set; } = toRow;
        /// The target column index for the move
        public int ToColumn { get; set; } = toColumn;
    }
}