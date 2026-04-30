namespace Chess.Models
{
    /// Represents a time setting for a chess game, used for selecting match duration
    public class GameTime
    {
        #region Properties
        /// The game duration in minutes
        public int Time { get; set; }
        /// Returns a formatted string of the time for UI display
        public string DisplayName => $"{Time} {Strings.Minutes}";
        #endregion
        #region Constructors
        /// Initializes a new instance with a specific time duration
        public GameTime(int time)
        {
            Time = time;
        }
        /// Initializes a new instance with a default duration of 5 minutes
        public GameTime()
        {
            Time = 5;
        }
        #endregion
    }
}