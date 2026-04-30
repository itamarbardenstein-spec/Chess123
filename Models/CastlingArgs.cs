namespace Chess.Models
{
    /// Represents the arguments for a castling event, containing direction and player information
    public class CastlingArgs(bool right, bool isHostUser, bool MyMove) : EventArgs
    {
        /// Indicates if the castling is to the right side (kingside)
        public bool Right { get; set; } = right;
        /// Indicates if the player performing the move is the host
        public bool IsHostUser { get; set; } = isHostUser;
        /// Indicates if the move was performed by the local player
        public bool MyMove { get; set; } = MyMove;
    }
}