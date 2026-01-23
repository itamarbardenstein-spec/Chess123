namespace Chess.Models
{
    public class OnPromotionArgs(int row, int column,bool isHostUser) : EventArgs
    {
        public int Row { get; set; } = row;
        public int Column { get; set; } = column;
        public bool IsHostUser { get; set; }= isHostUser;
    }
}
