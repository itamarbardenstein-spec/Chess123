namespace Chess.Models
{
    public class CastlingArgs(bool right, bool isHostUser, bool MyMove) : EventArgs
    {
        public bool Right { get; set; } = right;
        public bool IsHostUser { get; set; } = isHostUser;
        public bool MyMove { get; set; } = MyMove;
    }
}
