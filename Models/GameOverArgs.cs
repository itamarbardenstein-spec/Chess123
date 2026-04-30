namespace Chess.Models
{
    /// Represents the arguments for the game over event, detailing the result and the cause
    public class GameOverArgs(bool iWon, string reason) : EventArgs
    {
        /// The specific reason the game ended, such as checkmate, time, or resignation
        public string Reason { get; set; } = reason;
        /// Indicates whether the local player won the game
        public bool IWon { get; set; } = iWon;
    }
}