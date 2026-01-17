
namespace Chess.Models
{
    public class HighlightSquareArgs(int row, int column) : EventArgs
    {
        public int Row { get; set; } = row;
        public int Column { get; set; } = column;
    }
}


