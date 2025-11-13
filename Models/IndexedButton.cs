namespace Chess.Models
{
    public partial class IndexedButton: Button
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public IndexedButton(int row, int column)
        {
            RowIndex = row;
            ColumnIndex = column;
            HeightRequest = 45;
            WidthRequest = 45;
        }
    }
}
