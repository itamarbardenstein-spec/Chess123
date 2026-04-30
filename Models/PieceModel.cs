using Chess.ModelsLogic;

namespace Chess.Models
{
    /// Base UI component for a chess piece, handling position, appearance, and movement validation
    public abstract partial class PieceModel : ImageButton
    {
        #region Properties
        /// The current row position of the piece on the board (0-7)
        public int RowIndex { get; set; }
        /// The current column position of the piece on the board (0-7)
        public int ColumnIndex { get; set; }
        /// The file path or URI of the piece's image asset
        public string? StringImageSource { get; set; }
        /// Indicates if the piece belongs to the white set
        public bool IsWhite { get; set; }
        #endregion

        #region Constructors
        /// Initializes a piece with specific coordinates, color, and visual asset
        public PieceModel(int row, int column, bool isWhite, string? image)
        {
            RowIndex = row;
            ColumnIndex = column;
            IsWhite = isWhite;
            HeightRequest = 45;
            WidthRequest = 45;
            StringImageSource = image;
            Source = image;
        }

        /// Default constructor for creating a piece with standard dimensions
        public PieceModel()
        {
            HeightRequest = 45;
            WidthRequest = 45;
        }
        #endregion

        #region Public Methods
        public abstract bool IsMoveValid(Piece[,] board, int rFrom, int cFrom, int rTo, int cTo);
        #endregion

        #region Private Methods
        protected abstract bool PathClear(Piece[,] board, int row, int collumn, int row2, int collumn2, int directionRow, int directionColumn);
        #endregion
    }
}