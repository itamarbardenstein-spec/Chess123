namespace Chess.Models
{
    public partial class Piece: ImageButton
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public string ?StringImageSource { get; set; }   
        public enum PieceType { Pawn, Rook, Knight, Bishop, Queen, King }
        public PieceType? CurrentPieceType { get; set; }
        public bool IsWhite { get; set; }
        public Piece(int row, int column, PieceType? pieceType, bool isWhite, string? image)
        {
            RowIndex = row;
            ColumnIndex = column;
            CurrentPieceType = pieceType;
            IsWhite = isWhite;
            HeightRequest = 45;
            WidthRequest = 45;
            StringImageSource= image;
            Source = image;
        }
        public Piece()
        {
            HeightRequest = 45;
            WidthRequest = 45;
        }

        
    }
}
