
namespace Chess.Models
{
    public class PawnPromotionArgs(bool isHostUser, int row, int column, string PieceToSwitch, bool myMove) : EventArgs
    {
        public bool IsHostUser { get; set; } = isHostUser;
        public int Column { get; set; } = column;
        public int Row { get; set; } = row;
        public string PieceToSwitch { get; set; } = PieceToSwitch;
        public bool MyMove { get; set; } = myMove;
    }
}
