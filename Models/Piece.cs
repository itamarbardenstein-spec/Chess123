namespace Chess.Models
{
    public partial class Piece: Button
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public enum PieceType { Pawn, Rook, Knight, Bishop, Queen, King }
        public PieceType CurrentPieceType { get; set; }
        public bool IsWhite { get; set; }
        public Piece(int row, int column, PieceType pieceType, bool isWhite)
        {
            RowIndex = row;
            ColumnIndex = column;
            CurrentPieceType = pieceType;
            IsWhite = isWhite;
            HeightRequest = 45;
            WidthRequest = 45;
        }
        public Piece()
        {
            HeightRequest = 45;
            WidthRequest = 45;
        }
    }
}
