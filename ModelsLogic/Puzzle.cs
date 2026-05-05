using Chess.Models;

namespace Chess.ModelsLogic
{
    /// Class representing the chess puzzle logic, including board initialization and solution checking
    public partial class Puzzle : PuzzleModel
    {
        #region Constructor
        /// Initializes a new puzzle based on the specified difficulty level
        public Puzzle(string difficulty)
        {
            if (difficulty == Strings.Easy)
                InitEasyPuzzleBoard();
            else if (difficulty == Strings.Medium)
                InitMediumPuzzleBoard();
            else
                InitHardPuzzleBoard();
        }
        #endregion
        #region Public Methods
        /// Initializes the board layout and move requirements for an easy puzzle
        public override void InitEasyPuzzleBoard()
        {
            currentDifficulty = Strings.Easy;
            CorrectMoveRow = 2;
            CorrectMoveColumn = 2;
            CorrectPieceRow = 4;
            CorrectPieceColumn = 3;
            gameBoard = new Piece[8, 8];
            // Initialize board with empty squares (pawns without images)
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    gameBoard[i, j] = new Pawn(i, j, false, null);
            // Place specific pieces for the puzzle scenario
            gameBoard[0, 0] = new Rook(0, 0, false, Strings.BlackRook);
            gameBoard[0, 2] = new Bishop(0, 2, false, Strings.BlackBishop);
            gameBoard[0, 3] = new King(0, 3, false, Strings.BlackKing);
            gameBoard[0, 5] = new Bishop(0, 5, false, Strings.BlackBishop);
            gameBoard[0, 6] = new Knight(0, 6, false, Strings.BlackKnight);
            gameBoard[0, 7] = new Rook(0, 7, false, Strings.BlackRook);
            for (int i = 0; i < 8; i++)
                if (i != 3 && i != 4)
                    gameBoard[1, i] = new Pawn(1, i, false, Strings.BlackPawn);
            gameBoard[2, 1] = new Knight(2, 1, false, Strings.BlackKnight);
            gameBoard[3, 4] = new Queen(3, 4, false, Strings.BlackQueen);
            gameBoard[4, 3] = new Knight(4, 3, true, Strings.WhiteKnight);
            gameBoard[4, 4] = new Pawn(4, 4, false, Strings.BlackPawn);
            gameBoard[5, 2] = new Pawn(5, 2, true, Strings.WhitePawn);
            gameBoard[6, 3] = new Queen(6, 3, true, Strings.WhiteQueen);
            for (int i = 0; i < 8; i++)
                if (i != 2 && i != 3 && i != 4)
                    gameBoard[6, i] = new Pawn(6, i, true, Strings.WhitePawn);
            gameBoard[7, 0] = new Rook(7, 0, true, Strings.WhiteRook);
            gameBoard[7, 1] = new Knight(7, 1, true, Strings.WhiteKnight);
            gameBoard[7, 2] = new Bishop(7, 2, true, Strings.WhiteBishop);
            gameBoard[7, 3] = new King(7, 3, true, Strings.WhiteKing);
            gameBoard[7, 5] = new Bishop(7, 5, true, Strings.WhiteBishop);
            gameBoard[7, 7] = new Rook(7, 7, true, Strings.WhiteRook);
        }
        /// Initializes the board layout and move requirements for a medium puzzle
        public override void InitMediumPuzzleBoard()
        {
            currentDifficulty = Strings.Medium;
            CorrectMoveRow = 0;
            CorrectMoveColumn = 3;
            CorrectPieceRow = 7;
            CorrectPieceColumn = 3;
            gameBoard = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    gameBoard![i, j] = new Pawn(i, j, false, null);
            gameBoard![0, 1] = new King(0, 1, false, Strings.BlackKing);
            gameBoard[0, 3] = new Rook(0, 3, false, Strings.BlackRook);
            gameBoard[1, 0] = new Pawn(1, 0, false, Strings.BlackPawn);
            gameBoard[1, 2] = new Queen(1, 2, false, Strings.BlackQueen);
            gameBoard[1, 6] = new Pawn(1, 6, false, Strings.BlackPawn);
            gameBoard[2, 0] = new Pawn(2, 0, true, Strings.WhitePawn);
            gameBoard[2, 1] = new Pawn(2, 1, false, Strings.BlackPawn);
            gameBoard[2, 4] = new Pawn(2, 4, false, Strings.BlackPawn);
            gameBoard[3, 4] = new Bishop(3, 4, false, Strings.BlackBishop);
            gameBoard[4, 1] = new Pawn(4, 1, true, Strings.WhitePawn);
            gameBoard[4, 4] = new Queen(4, 4, true, Strings.WhiteQueen);
            gameBoard[4, 5] = new Pawn(4, 5, true, Strings.WhitePawn);
            gameBoard[4, 7] = new Pawn(4, 7, false, Strings.BlackPawn);
            gameBoard[5, 2] = new Pawn(5, 2, true, Strings.WhitePawn);
            gameBoard[5, 7] = new Pawn(5, 7, true, Strings.WhitePawn);
            gameBoard[6, 6] = new King(6, 6, true, Strings.WhiteKing);
            gameBoard[7, 3] = new Rook(7, 3, true, Strings.WhiteRook);
        }
        /// Initializes the board layout and move requirements for a hard puzzle
        public override void InitHardPuzzleBoard()
        {
            currentDifficulty = Strings.Hard;
            CorrectMoveRow = 0;
            CorrectMoveColumn = 5;
            CorrectPieceRow = 3;
            CorrectPieceColumn = 5;
            gameBoard = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    gameBoard[i, j] = new Pawn(i, j, false, null);
            gameBoard[0, 0] = new Rook(0, 0, false, Strings.BlackRook);
            gameBoard[0, 1] = new Knight(0, 1, false, Strings.BlackKnight);
            gameBoard[0, 2] = new Bishop(0, 2, false, Strings.BlackBishop);
            gameBoard[0, 3] = new King(0, 3, false, Strings.BlackKing);
            gameBoard[0, 7] = new Rook(0, 7, false, Strings.BlackRook);
            gameBoard[1, 0] = new Pawn(1, 0, false, Strings.BlackPawn);
            gameBoard[1, 1] = new Pawn(1, 1, false, Strings.BlackPawn);
            gameBoard[1, 4] = new Queen(1, 4, false, Strings.BlackQueen);
            gameBoard[2, 6] = new Knight(2, 6, false, Strings.BlackKnight);
            gameBoard[2, 7] = new Bishop(2, 7, false, Strings.BlackBishop);
            gameBoard[3, 3] = new Pawn(3, 3, false, Strings.BlackPawn);
            gameBoard[3, 5] = new Rook(3, 5, true, Strings.WhiteRook);
            gameBoard[4, 3] = new Knight(4, 3, true, Strings.WhiteKnight);
            gameBoard[4, 7] = new Pawn(4, 7, false, Strings.BlackPawn);
            gameBoard[5, 0] = new Bishop(5, 0, true, Strings.WhiteBishop);
            gameBoard[5, 2] = new Rook(5, 2, true, Strings.WhiteRook);
            gameBoard[5, 7] = new Bishop(5, 7, true, Strings.WhiteBishop);
            gameBoard[6, 0] = new Pawn(6, 0, true, Strings.WhitePawn);
            gameBoard[6, 2] = new Pawn(6, 2, true, Strings.WhitePawn);
            gameBoard[6, 3] = new Pawn(6, 3, true, Strings.WhitePawn);
            gameBoard[6, 5] = new Pawn(6, 5, true, Strings.WhitePawn);
            gameBoard[7, 2] = new King(7, 2, true, Strings.WhiteKing);
        }
        /// Handles the logic when a puzzle board square is clicked
        public override void OnButtonClicked(Piece p)
        {
            if (!solved)
            {
                List<int[]> legalMoves = GetLegalMoveList(p);
                // First click: Select piece
                if (ClickCount == 0)
                {
                    if (p?.StringImageSource != null && p.IsWhite)
                    {
                        ClickCount++;
                        MoveFromRow = p.RowIndex;
                        MoveFromColumn = p.ColumnIndex;
                        LegalMoves?.Invoke(this, legalMoves);
                    }
                }
                // Second click: Handle deselection, changing selection, or making a move
                else
                {
                    // Deselect if same square clicked
                    if (p.RowIndex == MoveFromRow && p.ColumnIndex == MoveFromColumn)
                    {
                        ClearLegalMovesDots?.Invoke(this, EventArgs.Empty);
                        ClickCount = 0;
                    }
                    // Change selection to another white piece
                    else if (p?.StringImageSource != null && p.IsWhite)
                    {
                        MoveFromRow = p.RowIndex;
                        MoveFromColumn = p.ColumnIndex;
                        LegalMoves?.Invoke(this, legalMoves);
                    }
                    // Attempt to move to the target square
                    else
                    {
                        if (gameBoard![MoveFromRow, MoveFromColumn].IsMoveValid(gameBoard, MoveFromRow, MoveFromColumn, p!.RowIndex, p.ColumnIndex))
                            Play(p.RowIndex, p.ColumnIndex);
                        ClearLegalMovesDots?.Invoke(this, EventArgs.Empty);
                        ClickCount = 0;
                    }

                }
            }
        }
        /// Executes a move and triggers subsequent validation/solution checks
        public override void Play(int rowIndex, int columnIndex)
        {
            MoveToRow = rowIndex;
            MoveToColumn = columnIndex;
            // Check if the user's move matches the puzzle's required move
            if (!CheckMove())
                IncorrectMove?.Invoke(this, EventArgs.Empty);
            else
            {
                // Update board state
                gameBoard![rowIndex, columnIndex] = CreatePiece(gameBoard![MoveFromRow, MoveFromColumn]!, rowIndex, columnIndex);
                gameBoard![MoveFromRow, MoveFromColumn] = new Pawn(MoveFromRow, MoveFromColumn, false, null);
                DisplayMoveArgs args = new(MoveFromRow, MoveFromColumn, rowIndex, columnIndex);
                DisplayChanged?.Invoke(this, args);
                moveNumber++;
                CheckSolution();
            }
        }
        /// Determines if the current move coordinates match the puzzle solution
        public override bool CheckMove()
        {
            return MoveFromRow == CorrectPieceRow && MoveFromColumn == CorrectPieceColumn && MoveToRow == CorrectMoveRow && MoveToColumn == CorrectMoveColumn;
        }
        /// Validates the puzzle state and triggers opponent moves or solution completion
        public override void CheckSolution()
        {
            // Easy puzzles or reaching move limit for complex puzzles ends the game
            if (currentDifficulty == Strings.Easy || moveNumber == 2)
            {
                CorrectSolution?.Invoke(this, EventArgs.Empty);
                solved = true;
            }
            else
            {
                CorrectMove?.Invoke(this, EventArgs.Empty);
                // Setup the next requirement for multi-move puzzles (Medium/Hard)
                if (currentDifficulty == Strings.Medium)
                {
                    CorrectMoveRow = 1;
                    CorrectMoveColumn = 1;
                    CorrectPieceRow = 4;
                    CorrectPieceColumn = 4;
                    MakeOpponentMove?.Invoke(this, Strings.Medium);
                    RemoveHighlight?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    CorrectMoveRow = 0;
                    CorrectMoveColumn = 2;
                    CorrectPieceRow = 5;
                    CorrectPieceColumn = 2;
                    MakeOpponentMove?.Invoke(this, Strings.Hard);
                    RemoveHighlight?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        /// Factory method to create a new instance of a piece based on an existing one
        public override Piece CreatePiece(Piece original, int row, int col)
        {
            bool isWhite = original.IsWhite;
            string? img = original.StringImageSource;
            return original switch
            {
                Pawn => new Pawn(row, col, isWhite, img),
                Rook r => new Rook(row, col, isWhite, img)
                {
                    HasLeftRookMoved = r.HasLeftRookMoved,
                    HasRightRookMoved = r.HasRightRookMoved
                },
                Knight => new Knight(row, col, isWhite, img),
                Bishop => new Bishop(row, col, isWhite, img),
                Queen => new Queen(row, col, isWhite, img),
                King k => new King(row, col, isWhite, img)
                {
                    HasKingMoved = k.HasKingMoved
                },
                _ => throw new Exception()
            };
        }
        /// Provides a hint by highlighting the correct piece to move
        public override void HintSquare()
        {
            HighlightHintSquare?.Invoke(this, new HighlightSquareArgs(CorrectPieceRow, CorrectPieceColumn));
        }
        /// Provides a hint showing the full correct move (start and end squares)
        public override void CorrectMoveSquares()
        {
            HighlightCorrectMoveHint?.Invoke(this, new DisplayMoveArgs(CorrectPieceRow, CorrectPieceColumn, CorrectMoveRow, CorrectMoveColumn));
        }
        #endregion
        #region Private Methods
        /// Scans the board to find all legal move coordinates for a given piece
        protected override List<int[]> GetLegalMoveList(Piece p)
        {
            List<int[]> legalMoves = [];
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    if (gameBoard![p.RowIndex, p.ColumnIndex].IsMoveValid(gameBoard!, p.RowIndex, p.ColumnIndex, r, c))
                        legalMoves.Add([r, c]);
            return legalMoves;
        }
        #endregion
    }
}