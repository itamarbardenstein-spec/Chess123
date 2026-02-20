using Chess.ModelsLogic;

namespace Chess.Models
{
    public abstract class PuzzleGridModel:Grid
    {
        #region Fields
        protected Piece[,]? BoardPieces;
        protected int moveCount = 0;
        protected Dictionary<(int row, int col), Piece> BoardUIMap = [];
        #endregion
        #region Events
        public EventHandler<Piece>? ButtonClicked;
        #endregion
        #region Public Methods
        public abstract void InitEasyPuzzleGrid(Grid board);
        public abstract void InitMediumPuzzleGrid(Grid board);
        public abstract void InitHardPuzzleGrid(Grid board);       
        public abstract Piece CreatePiece(Piece original, int row, int col);
        public abstract void UpdateDisplay(DisplayMoveArgs e);
        public abstract void MakeOpponentMove(string difficulty);
        public abstract void ShowLegalMoves(List<int[]> legalMoves);
        public abstract void ClearDots();
        public abstract void ShowHint(int row, int column);
        public abstract void ShowCorrectMove(int fromRow, int fromColumn, int toRow, int toColumn);
        public abstract void ClearBoardHighLights();
        #endregion
        #region Private Methods
        protected abstract void OnButtonClicked(object? sender, EventArgs e);
        protected abstract void UpdateCellUI(int row, int col);
        #endregion
    }
}
