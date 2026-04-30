namespace Chess.Models
{
    /// Configuration settings for a game timer, specifying the starting duration and update frequency
    public class TimerSettings(long totalTimeInMilliseconds, long intervalInMilliseconds)
    {
        /// The initial total duration of the timer in milliseconds
        public long TotalTimeInMilliseconds { get; set; } = totalTimeInMilliseconds;
        /// The time interval between each timer tick or update in milliseconds
        public long IntervalInMilliseconds { get; set; } = intervalInMilliseconds;
    }
}