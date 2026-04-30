namespace Chess.Models
{
    /// Represents the arguments for a pawn promotion event, identifying the location and player
    public class OnPromotionArgs(int row, int column, bool isHostUser) : EventArgs
    {
        /// The row index where the pawn reached the promotion rank
        public int Row { get; set; } = row;
        /// The column index where the promotion is occurring
        public int Column { get; set; } = column;
        /// Indicates if the player triggering the promotion is the host
        public bool IsHostUser { get; set; } = isHostUser;
    }
}