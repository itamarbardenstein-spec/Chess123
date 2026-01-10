namespace Chess.Models
{
    public class OnPromotionArgs(int row, int column) : EventArgs
    {
        public int Row { get; set; } = row;
        public int Column { get; set; } = column;
    }
}
