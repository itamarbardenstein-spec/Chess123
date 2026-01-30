using Chess.ModelsLogic;

namespace Chess.Models
{
    public abstract class PuzzleModel
    {
        protected Piece[,]? gameBoard;
        protected int ClickCount = 0;
        protected int MoveFromRow = 0;
        protected int MoveFromColumn = 0;
        protected int MoveToRow = 0;
        protected int MoveToCulomn = 0;
        public abstract void CheckMove(int rowIndex, int columnIndex);
        public abstract void OnButtonClicked(Piece p);
        protected abstract bool IsKingInCheck(bool isWhite, Piece[,] board);
        public abstract Piece CreatePiece(Piece original, int row, int col);
        protected abstract Piece[,] FlipBoard(Piece[,] original);
        public abstract void InitPuzzleBoard();
        public EventHandler? ClearLegalMovesDots;
        public EventHandler? CorrectMove;
        public EventHandler? IncorrectMove;
        public EventHandler<List<int[]>>? LegalMoves;
    }
}
