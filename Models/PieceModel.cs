using Chess.ModelsLogic;

namespace Chess.Models
{
    public abstract partial class PieceModel: ImageButton
    {
        #region Properties
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public string ?StringImageSource { get; set; }   
        public bool IsWhite { get; set; }
        #endregion
        #region Constructors
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
        #endregion
        #region Public Methods
        public abstract bool IsMoveValid(Piece[,] board,int rFrom, int cFrom, int rTo, int cTo);
        #endregion
        #region Private Methods
        protected abstract bool PathClear(Piece[,] board, int row, int collumn, int row2, int collumn2, int directionRow, int directionColumn);
        #endregion
    }
}
