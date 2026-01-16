using Chess.Models;
namespace Chess.ModelsLogic
{
    public partial class GameGrid : GameGridModel
    {
        public override void InitGrid(Grid board,bool IsHostUser)
        {
            this.Parent = board;    
            BoardPieces = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                board.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
            for (int i = 0; i < 8; i++)
            {
                if (IsHostUser)
                {
                    BoardPieces[1, i] = new Pawn(1, i, true, Strings.WhitePawn);
                    BoardPieces[6, i] = new Pawn(6, i, false, Strings.BlackPawn);
                }
                else
                {
                    BoardPieces[1, i] = new Pawn(1, i, false, Strings.BlackPawn);
                    BoardPieces[6, i] = new Pawn(6, i, true, Strings.WhitePawn);
                }
                if (i == 0 || i == 7)
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i] = new Rook(0, i, true, Strings.WhiteRook);
                        BoardPieces[7, i] = new Rook(7, i, false, Strings.BlackRook);
                    }
                    else
                    {
                        BoardPieces[0, i] = new Rook(0, i, false, Strings.BlackRook);
                        BoardPieces[7, i] = new Rook(7, i, true, Strings.WhiteRook);
                    }
                }
                else if (i == 1 || i == 6)
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i] = new Knight(0, i, true, Strings.WhiteKnight);
                        BoardPieces[7, i] = new Knight(7, i, false, Strings.BlackKnight);
                    }
                    else
                    {
                        BoardPieces[0, i] = new Knight(0, i, false, Strings.BlackKnight);
                        BoardPieces[7, i] = new Knight(7, i, true, Strings.WhiteKnight);
                    }
                }
                else if (i == 2 || i == 5)
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i] = new Bishop(0, i, true, Strings.WhiteBishop);
                        BoardPieces[7, i] = new Bishop(7, i, false, Strings.BlackBishop);
                    }
                    else
                    {
                        BoardPieces[0, i] = new Bishop(0, i, false, Strings.BlackBishop);
                        BoardPieces[7, i] = new Bishop(7, i, true, Strings.WhiteBishop);
                    }
                }
                else if (i == 3)
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i + 1] = new Queen(0, i + 1, true, Strings.WhiteQueen);
                        BoardPieces[7, i + 1] = new Queen(7, i + 1, false, Strings.BlackQueen);
                    }
                    else
                    {
                        BoardPieces[0, i] = new Queen(0, i, false, Strings.BlackQueen);
                        BoardPieces[7, i] = new Queen(7, i, true, Strings.WhiteQueen);
                    }
                }
                else
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i - 1] = new King(0, i - 1, true, Strings.WhiteKing);
                        BoardPieces[7, i - 1] = new King(7, i - 1, false, Strings.BlackKing);
                    }
                    else
                    {
                        BoardPieces[0, i] = new King(0, i, false, Strings.BlackKing);
                        BoardPieces[7, i] = new King(7, i, true, Strings.WhiteKing);
                    }
                }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (BoardPieces[i, j] == null)
                        BoardPieces[i, j] = new Pawn(i, j, false, null);
                    Piece p = BoardPieces[i, j];
                    if ((i + j) % 2 == 0)
                    {
                        p!.BackgroundColor = Color.FromArgb(Strings.BoardColorWhite);
                    }
                    else
                    {
                        p!.BackgroundColor = Color.FromArgb(Strings.BoardColorBlack);
                    }
                    p.Clicked += OnButtonClicked;
                    ((Grid)this.Parent).Add(p, j, i);
                    BoardUIMap[(i, j)] = p;
                }
            }
        }
        protected override void OnButtonClicked(object? sender, EventArgs e)
        {
            ButtonClicked?.Invoke(this, (Piece)sender!);
        }        
        public override void UpdateDisplay(DisplayMoveArgs e)
        {
            BoardPieces![e.ToRow, e.ToColumn] = CreatePiece(BoardPieces![e.FromRow, e.FromColomn]!, e.ToRow, e.ToColumn);
            BoardPieces![e.FromRow, e.FromColomn] = new Pawn(e.FromRow, e.FromColomn, false, null);
            UpdateCellUI(e.FromRow, e.FromColomn);
            UpdateCellUI(e.ToRow, e.ToColumn);
        }
        protected override void UpdateCellUI(int row, int col)
        {
            if (!BoardUIMap.TryGetValue((row, col), out PieceModel? uiPiece))
                return;
            Piece modelPiece = BoardPieces![row, col];
            if (modelPiece.StringImageSource == null)
            {
                uiPiece.StringImageSource = null;
                uiPiece.Source = null;
                uiPiece.IsWhite = false;
            }
            else
            {
                uiPiece.StringImageSource = modelPiece.StringImageSource;
                uiPiece.Source = modelPiece.StringImageSource;
                uiPiece.IsWhite = modelPiece.IsWhite;
            }
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
        public override void Castling(bool right,bool isHostUser,bool MyMove)
        {
            if (right)
            {
                if (MyMove)
                {
                    if (isHostUser)
                    {
                        BoardPieces![7, 4] = CreatePiece(BoardPieces[7, 7], 7, 4);
                        BoardPieces[7, 7] = new Pawn(7, 7, false, null);
                        UpdateCellUI(7, 4);
                        UpdateCellUI(7, 7);
                    }
                    else
                    {
                        BoardPieces![7, 5] = CreatePiece(BoardPieces[7, 7], 7, 5);
                        BoardPieces[7, 7] = new Pawn(7, 7, false, null);
                        UpdateCellUI(7, 5);
                        UpdateCellUI(7, 7);
                    }
                }
                else
                {
                    if (!isHostUser)
                    {
                        BoardPieces![0, 5] = CreatePiece(BoardPieces[0, 7], 0, 5);
                        BoardPieces[0, 7] = new Pawn(0, 7, false, null);
                        UpdateCellUI(0, 5);
                        UpdateCellUI(0, 7);
                    }
                    else
                    {
                        BoardPieces![0, 4] = CreatePiece(BoardPieces[0, 7], 0, 4);
                        BoardPieces[0, 7] = new Pawn(0, 7, false, null);
                        UpdateCellUI(0, 4);
                        UpdateCellUI(0, 7);
                    }
                }                
            }
            else
            {
                if (MyMove)
                {
                    if (isHostUser)
                    {
                        BoardPieces![7, 2] = CreatePiece(BoardPieces[7, 0], 7, 2);
                        BoardPieces[7, 0] = new Pawn(7, 0, false, null);
                        UpdateCellUI(7, 2);
                        UpdateCellUI(7, 0);
                    }
                    else
                    {
                        BoardPieces![7, 3] = CreatePiece(BoardPieces[7, 0], 7, 3);
                        BoardPieces[7, 0] = new Pawn(7, 0, false, null);
                        UpdateCellUI(7, 3);
                        UpdateCellUI(7, 0);
                    }
                }
                else
                {
                    if (!isHostUser)
                    {
                        BoardPieces![0, 3] = CreatePiece(BoardPieces[0, 0], 0, 3);
                        BoardPieces[0, 0] = new Pawn(0, 0, false, null);
                        UpdateCellUI(0, 3);
                        UpdateCellUI(0, 0);
                    }
                    else
                    {
                        BoardPieces![0, 2] = CreatePiece(BoardPieces[0, 0], 0, 2);
                        BoardPieces[0, 0] = new Pawn(0, 0, false, null);
                        UpdateCellUI(0, 2);
                        UpdateCellUI(0, 0);
                    }
                }
            }            
        }
        public override void Promotion(bool IsHostUser, int row, int column, string pieceToSwitch, bool MyMove)
        {
            if (pieceToSwitch == Strings.Queen)
            {
                if (MyMove)
                    BoardPieces![row, column] = IsHostUser ? new Queen(row, column, false, Strings.BlackQueen) : new Queen(row, column, true, Strings.WhiteQueen);
                else
                    BoardPieces![row, column] = IsHostUser ? new Queen(row, column, true, Strings.WhiteQueen) : new Queen(row, column, false, Strings.BlackQueen);
            }
            else if (pieceToSwitch == Strings.Rook)
            {
                if (MyMove)
                    BoardPieces![row, column] = IsHostUser ? new Rook(row, column, false, Strings.BlackRook) : new Rook(row, column, true, Strings.WhiteRook);
                else
                    BoardPieces![row, column] = IsHostUser ? new Rook(row, column, true, Strings.WhiteRook) : new Rook(row, column, false, Strings.BlackRook);
            }
            else if (pieceToSwitch == Strings.Bishop)
            {
                if (MyMove)
                    BoardPieces![row, column] = IsHostUser ? new Bishop(row, column, false, Strings.BlackBishop) : new Bishop(row, column, true, Strings.WhiteBishop);
                else
                    BoardPieces![row, column] = IsHostUser ? new Bishop(row, column, true, Strings.WhiteBishop) : new Bishop(row, column, false, Strings.BlackBishop);
            }
            else
            {
                if (MyMove)
                    BoardPieces![row, column] = IsHostUser ? new Knight(row, column, false, Strings.BlackKnight) : new Knight(row, column, true, Strings.WhiteKnight);
                else
                    BoardPieces![row, column] = IsHostUser ? new Knight(row, column, true, Strings.WhiteKnight) : new Knight(row, column, false, Strings.BlackKnight);

            }
            UpdateCellUI(row, column);
        }
        public void ShowLegalMoves(List<int[]> legalMoves)
        {
            ClearDots();
            foreach (var move in legalMoves)
            {
                int row = move[0];
                int col = move[1];
                var cell = GetCell(row, col);
                if (cell != null)
                {                    
                    cell.BackgroundColor = Color.FromRgba(255, 255, 0, 0.4);
                }
            }
        }
        private Piece? GetCell(int row, int col)
        {
            if (BoardUIMap.TryGetValue((row, col), out PieceModel? uiPiece))
            {
                return uiPiece as Piece;
            }
            return null;
        }
        public void ClearDots()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var cell = GetCell(i, j);
                    if (cell != null)
                    {
                        if ((i + j) % 2 == 0)
                            cell.BackgroundColor = Color.FromArgb(Strings.BoardColorWhite);
                        else
                            cell.BackgroundColor = Color.FromArgb(Strings.BoardColorBlack);
                    }
                }
            }
        }
    }
}
