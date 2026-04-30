namespace Chess.Models
{
    /// Represents the arguments for highlighting a specific square on the chessboard
    public class HighlightSquareArgs(int row, int column) : EventArgs
    {
        /// The row index of the square to be highlighted
        public int Row { get; set; } = row;
        /// The column index of the square to be highlighted
        public int Column { get; set; } = column;
    }
}