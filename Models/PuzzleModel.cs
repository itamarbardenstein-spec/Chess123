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
        protected int MoveToColumn = 0;
        protected bool solved = false;
        protected string currentDifficulty = String.Empty;
        protected int moveNumber = 0;
        protected int CorrectMoveRow = 0;
        protected int CorrectMoveColumn = 0;
        protected int CorrectPieceRow = 0;
        protected int CorrectPieceColumn = 0;
        public abstract void OnButtonClicked(Piece p);
        public abstract Piece CreatePiece(Piece original, int row, int col);
        public abstract void InitEasyPuzzleBoard();
        public abstract void InitMediumPuzzleBoard();
        public abstract void Play(int rowIndex, int columnIndex);
        public abstract void InitHardPuzzleBoard();
        public abstract bool CheckMove();
        public abstract void CheckSolution();
        public EventHandler? ClearLegalMovesDots;
        public EventHandler? CorrectMove;
        public EventHandler? CorrectSolution;
        public EventHandler? IncorrectMove;
        public EventHandler<string>? MakeOpponentMove;
        public EventHandler<List<int[]>>? LegalMoves;
        public EventHandler<DisplayMoveArgs>? DisplayChanged;
    }
}
