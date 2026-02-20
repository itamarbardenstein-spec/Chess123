namespace Chess.Models
{
    public class GameTime
    {
        #region Properties
        public int Time { get; set; }
        public string DisplayName => $"{Time} {Strings.Minutes}";
        #endregion
        #region Constructors
        public GameTime(int time)
        {
            Time = time;
        }
        public GameTime()
        {
            Time = 5;
        }
        #endregion
    }
}
