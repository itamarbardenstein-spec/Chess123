namespace Chess.Models
{
    public class GameOverArgs(bool iWon,string reason) : EventArgs
    {
        public string Reason { get; set; } = reason;
        public bool IWon { get; set; }= iWon;
    }
}
