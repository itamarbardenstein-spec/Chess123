using Chess.Models;

namespace Chess.ModelsLogic
{
    public partial class Puzzle : PuzzleModel
    {
        public Puzzle()
        {
            InitPuzzleBoard();
        }
        public override void InitPuzzleBoard()
        {
            gameBoard = new Piece[8, 8];            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    gameBoard[i, j] = new Pawn(i, j, false, null);
                }
            }
            // 3. הצבת הכלים במיקומים הסופיים
            // שורה 0 (העליונה)
            gameBoard[0, 0] = new Rook(0, 0, true, Strings.WhiteRook);
            gameBoard[0, 2] = new Bishop(0, 2, true, Strings.WhiteBishop);
            gameBoard[0, 3] = new King(0, 3, true, Strings.WhiteKing);
            gameBoard[0, 5] = new Bishop(0, 5, true, Strings.WhiteBishop);
            gameBoard[0, 6] = new Knight(0, 6, true, Strings.WhiteKnight);
            gameBoard[0, 7] = new Rook(0, 7, true, Strings.WhiteRook);

            // שורה 1
            for (int i = 0; i < 8; i++)
            {
                if (i != 3 && i != 4)
                    gameBoard[1, i] = new Pawn(1, i, true, Strings.WhitePawn);
            }

            // שורה 2
            gameBoard[2, 1] = new Knight(2, 1, true, Strings.WhiteKnight);

            // שורה 3 - המלכה הלבנה (הוזזה שתי משבצות ימינה מטור 2 לטור 4)
            gameBoard[3, 4] = new Queen(3, 4, true, Strings.WhiteQueen);

            // שורה 4 - הפרש השחור נשאר בטור 3 (אחד שמאלה מהמקור)
            gameBoard[4, 3] = new Knight(4, 3, false, Strings.BlackKnight);
            gameBoard[4, 4] = new Pawn(4, 4, true, Strings.WhitePawn);

            // שורה 5
            gameBoard[5, 2] = new Pawn(5, 2, false, Strings.BlackPawn);

            // שורה 6 - המלכה השחורה נשארת בטור 3 (אחד שמאלה מהמקור)
            gameBoard[6, 3] = new Queen(6, 3, false, Strings.BlackQueen);
            for (int i = 0; i < 8; i++)
            {
                if (i == 2 || i == 3 || i == 4) continue;
                gameBoard[6, i] = new Pawn(6, i, false, Strings.BlackPawn);
            }
            // שורה 7 (התחתונה)
            gameBoard[7, 0] = new Rook(7, 0, false, Strings.BlackRook);
            gameBoard[7, 1] = new Knight(7, 1, false, Strings.BlackKnight);
            gameBoard[7, 2] = new Bishop(7, 2, false, Strings.BlackBishop);
            gameBoard[7, 3] = new King(7, 3, false, Strings.BlackKing);
            gameBoard[7, 5] = new Bishop(7, 5, false, Strings.BlackBishop);
            gameBoard[7, 7] = new Rook(7, 7, false, Strings.BlackRook);      
        }     
        public override void OnButtonClicked(Piece p)
        {
            if (!solved)
            {
                List<int[]> legalMoves = GetLegalMoveList(p);
                if (ClickCount == 0)
                {
                    if (p?.StringImageSource != null && !p.IsWhite)
                    {
                        ClickCount++;
                        MoveFromRow = p.RowIndex;
                        MoveFromColumn = p.ColumnIndex;
                        LegalMoves?.Invoke(this, legalMoves);
                    }
                }
                else
                {
                    if (p.RowIndex == MoveFromRow && p.ColumnIndex == MoveFromColumn)
                    {
                        ClearLegalMovesDots?.Invoke(this, EventArgs.Empty);
                        ClickCount = 0;
                    }
                    else if (p?.StringImageSource != null)
                    {
                        MoveFromRow = p.RowIndex;
                        MoveFromColumn = p.ColumnIndex;
                        LegalMoves?.Invoke(this, legalMoves);
                    }
                    else
                    {
                        if (gameBoard![MoveFromRow, MoveFromColumn].IsMoveValid(gameBoard, MoveFromRow, MoveFromColumn, p!.RowIndex, p.ColumnIndex))
                            CheckMove(p.RowIndex, p.ColumnIndex);
                        ClearLegalMovesDots?.Invoke(this, EventArgs.Empty);
                        ClickCount = 0;
                    }

                }
            }
        }               
        public override void CheckMove(int rowIndex, int columnIndex)
        {
            MoveToRow = rowIndex;
            MoveToCulomn = columnIndex;
            if (MoveFromRow == 4 && MoveFromColumn == 3 && MoveToRow == 2 && MoveToCulomn == 2)
            {
                CorrectMove?.Invoke(this, EventArgs.Empty);
                solved = true;
            }               
            else
            {
                IncorrectMove?.Invoke(this, EventArgs.Empty);
                ClickCount = 0;
            }            
        }
        private List<int[]> GetLegalMoveList(Piece p)
        {
            List<int[]> legalMoves = [];
            bool isWhite = p.IsWhite;
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    if (gameBoard![p.RowIndex, p.ColumnIndex].IsMoveValid(gameBoard!, p.RowIndex, p.ColumnIndex, r, c))
                    {
                        Piece fromPiece = gameBoard[p.RowIndex, p.ColumnIndex];
                        Piece toPiece = gameBoard[r, c];
                        int originalRow = p.RowIndex;
                        int originalCol = p.ColumnIndex;
                        gameBoard[r, c] = CreatePiece(fromPiece, r, c);
                        gameBoard[p.RowIndex, p.ColumnIndex] = new Pawn(p.RowIndex, p.ColumnIndex, false, null);
                        bool kingInCheck = IsKingInCheck(isWhite, FlipBoard(gameBoard));
                        gameBoard[p.RowIndex, p.ColumnIndex] = fromPiece;
                        gameBoard[r, c] = toPiece;
                        fromPiece.RowIndex = originalRow;
                        fromPiece.ColumnIndex = originalCol;
                        if (!kingInCheck)
                            legalMoves.Add([r, c]);
                    }
                }
            }
            return legalMoves;
        }
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
        protected override bool IsKingInCheck(bool isWhite, Piece[,] board)
        {
            int kingRow = -1, kingCol = -1;
            bool found = false;
            for (int i = 0; i < 8 && !found; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (board![i, j] is King k && !k.IsWhite)
                    {
                        kingRow = i;
                        kingCol = j;
                        found = true;
                    }
                }
            }
            if (found)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Piece p = board![i, j];
                        if (p.StringImageSource != null && p.IsWhite)
                        {
                            if (p.IsMoveValid(board!, i, j, kingRow, kingCol))
                                return true;
                        }
                    }
                }
            }
            return false;
        }
        protected override Piece[,] FlipBoard(Piece[,] original)
        {
            Piece[,] flipped = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece p = original[i, j];
                    int newRow = 7 - i;
                    int newCol = 7 - j;
                    if (p.StringImageSource == null)
                    {
                        flipped[newRow, newCol] = new Pawn(newRow, newCol, false, null);
                    }
                    else
                    {
                        flipped[newRow, newCol] = CreatePiece(p, newRow, newCol);
                    }
                }
            }
            return flipped;
        }
    }
}
