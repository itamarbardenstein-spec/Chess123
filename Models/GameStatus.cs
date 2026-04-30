namespace Chess.Models
{
    /// Manages the current state of a player's turn and provides corresponding UI messages
    public class GameStatus
    {
        #region Fields
        /// Array containing the localized string messages for each game status
        private readonly string[] msgs = [Strings.WaitMessage, Strings.PlayMessage];
        /// Defines the possible states of a player's turn
        public enum Statuses { Wait, Play }
        #endregion
        #region Properties
        /// Gets or sets the current turn status of the player
        public Statuses CurrentStatus { get; set; } = Statuses.Wait;
        /// Returns the descriptive text message associated with the current status
        public string StatusMessage => msgs[(int)CurrentStatus];
        #endregion
        #region Public Methods
        /// Toggles the current status between 'Play' and 'Wait'
        public void UpdateStatus()
        {
            CurrentStatus = CurrentStatus == Statuses.Play ? Statuses.Wait : Statuses.Play;
        }
        #endregion
    }
}