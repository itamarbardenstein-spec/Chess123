namespace Chess.Models
{
    public class GameOverArgs(bool iWon, bool IsCheckmate) : EventArgs
    {
        public bool IsCheckmate { get; set; } = IsCheckmate;
        public bool IWon { get; set; }= iWon;
    }
}
