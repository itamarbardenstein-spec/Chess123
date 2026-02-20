namespace Chess.Models
{
    public class GameStatus
    {
        #region Fields
        private readonly string[] msgs = [Strings.WaitMessage, Strings.PlayMessage];
        public enum Statuses { Wait, Play }
        #endregion
        #region Properties
        public Statuses CurrentStatus { get; set; } = Statuses.Wait;
        public string StatusMessage => msgs[(int)CurrentStatus];
        #endregion
        #region Public Methods
        public void UpdateStatus()
        {
            CurrentStatus = CurrentStatus == Statuses.Play ? Statuses.Wait : Statuses.Play;
        }
        #endregion
    }
}
