namespace Chess.Models
{
    public class DisplayMoveArgs(int fromRow, int fromColumn,int toRow,int toColumn) : EventArgs
    {
        public int FromRow { get; set; } = fromRow;
        public int FromColomn { get; set; } = fromColumn;
        public int ToRow { get; set; } = toRow;
        public int ToColumn { get; set; } = toColumn;
    }
}
