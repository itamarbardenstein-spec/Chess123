using Chess.ModelsLogic;

namespace Chess.Models
{
    public abstract partial class PieceModel: ImageButton
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public string ?StringImageSource { get; set; }   
        public bool IsWhite { get; set; }
        public PieceModel(int row, int column, bool isWhite, string? image)
        {
            RowIndex = row;
            ColumnIndex = column;
            IsWhite = isWhite;
            HeightRequest = 45;
            WidthRequest = 45;
            StringImageSource= image;
            Source = image;
        }
        public PieceModel()
        {
            HeightRequest = 45;
            WidthRequest = 45;
        }
        public abstract bool IsMoveValid(Piece[,] board,int rFrom, int cFrom, int rTo, int cTo);
        protected abstract bool PathClear(Piece[,] board, int r, int c, int r2, int c2, int dr, int dc);        
    }
}
