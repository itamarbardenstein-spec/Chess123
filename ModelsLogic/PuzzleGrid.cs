using Chess.Models;
using Microsoft.Maui.Controls.Shapes;

namespace Chess.ModelsLogic
{
    public partial class PuzzleGrid:PuzzleGridModel
    {
        public override void InitEasyPuzzleGrid(Grid board)
        {
            this.Parent = board;
            BoardPieces = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                board.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    BoardPieces[i, j] = new Pawn(i, j, false, null);

            BoardPieces[0, 0] = new Rook(0, 0, false, Strings.BlackRook);
            BoardPieces[0, 2] = new Bishop(0, 2, false, Strings.BlackBishop);
            BoardPieces[0, 3] = new King(0, 3, false, Strings.BlackKing);
            BoardPieces[0, 5] = new Bishop(0, 5, false, Strings.BlackBishop);
            BoardPieces[0, 6] = new Knight(0, 6, false, Strings.BlackKnight);
            BoardPieces[0, 7] = new Rook(0, 7, false, Strings.BlackRook);

            for (int i = 0; i < 8; i++)
                if (i != 3 && i != 4)
                    BoardPieces[1, i] = new Pawn(1, i, false, Strings.BlackPawn);

            BoardPieces[2, 1] = new Knight(2, 1, false, Strings.BlackKnight);
            BoardPieces[3, 4] = new Queen(3, 4, false, Strings.BlackQueen);
            BoardPieces[4, 3] = new Knight(4, 3, true, Strings.WhiteKnight);
            BoardPieces [4, 4] = new Pawn(4, 4, false, Strings.BlackPawn);
            BoardPieces[5, 2] = new Pawn(5, 2, true, Strings.WhitePawn);
            BoardPieces[6, 3] = new Queen(6, 3, true, Strings.WhiteQueen);

            for (int i = 0; i < 8; i++)
                if (i != 2 && i != 3 && i != 4)
                    BoardPieces[6, i] = new Pawn(6, i, true, Strings.WhitePawn);

            BoardPieces[7, 0] = new Rook(7, 0, true, Strings.WhiteRook);
            BoardPieces[7, 1] = new Knight(7, 1, true, Strings.WhiteKnight);
            BoardPieces[7, 2] = new Bishop(7, 2, true, Strings.WhiteBishop);
            BoardPieces[7, 3] = new King(7, 3, true, Strings.WhiteKing);
            BoardPieces[7, 5] = new Bishop(7, 5, true, Strings.WhiteBishop);
            BoardPieces[7, 7] = new Rook(7, 7, true, Strings.WhiteRook);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece p = BoardPieces[i, j];
                    p.BackgroundColor = (i + j) % 2 == 0
                        ? Color.FromArgb(Strings.BoardColorWhite)
                        : Color.FromArgb(Strings.BoardColorBlack);
                    p.Clicked += OnButtonClicked;
                    ((Grid)this.Parent).Add(p, j, i);
                    BoardUIMap[(i, j)] = p;
                }
            }
        }
        public override void InitMediumPuzzleGrid(Grid board)
        {
            this.Parent = board;
            BoardPieces = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                board.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    BoardPieces[i, j] = new Pawn(i, j, false, null);

            BoardPieces[0, 1] = new King(0, 1, false, Strings.BlackKing);   
            BoardPieces[0, 3] = new Rook(0, 3, false, Strings.BlackRook);  
            BoardPieces[1, 0] = new Pawn(1, 0, false, Strings.BlackPawn); 
            BoardPieces[1, 2] = new Queen(1, 2, false, Strings.BlackQueen); 
            BoardPieces[1, 6] = new Pawn(1, 6, false, Strings.BlackPawn);
            BoardPieces[2, 0] = new Pawn(2, 0, true, Strings.WhitePawn);  
            BoardPieces[2, 1] = new Pawn(2, 1, false, Strings.BlackPawn);  
            BoardPieces[2, 4] = new Pawn(2, 4, false, Strings.BlackPawn);  
            BoardPieces[3, 4] = new Bishop(3, 4, false, Strings.BlackBishop); 
            BoardPieces[4, 1] = new Pawn(4, 1, true, Strings.WhitePawn);  
            BoardPieces[4, 4] = new Queen(4, 4, true, Strings.WhiteQueen);  
            BoardPieces[4, 5] = new Pawn(4, 5, true, Strings.WhitePawn);    
            BoardPieces[4, 7] = new Pawn(4, 7, false, Strings.BlackPawn);  
            BoardPieces[5, 2] = new Pawn(5, 2, true, Strings.WhitePawn);   
            BoardPieces[5, 7] = new Pawn(5, 7, true, Strings.WhitePawn);    
            BoardPieces[6, 6] = new King(6, 6, true, Strings.WhiteKing);   
            BoardPieces[7, 3] = new Rook(7, 3, true, Strings.WhiteRook);   
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece p = BoardPieces[i, j];
                    p.BackgroundColor = (i + j) % 2 == 0
                        ? Color.FromArgb(Strings.BoardColorWhite)
                        : Color.FromArgb(Strings.BoardColorBlack);
                    p.Clicked += OnButtonClicked;
                    ((Grid)this.Parent).Add(p, j, i);
                    BoardUIMap[(i, j)] = p;
                }
            }
        }        
        public override void InitHardPuzzleGrid(Grid board)
        {
            this.Parent = board;
            BoardPieces = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                board.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    BoardPieces[i, j] = new Pawn(i, j, false, null);
            BoardPieces[0, 0] = new Rook(0, 0, false, Strings.BlackRook);   
            BoardPieces[0, 1] = new Knight(0, 1, false, Strings.BlackKnight);
            BoardPieces[0, 2] = new Bishop(0, 2, false, Strings.BlackBishop);
            BoardPieces[0, 3] = new King(0, 3, false, Strings.BlackKing);   
            BoardPieces[0, 7] = new Rook(0, 7, false, Strings.BlackRook);    
            BoardPieces[1, 0] = new Pawn(1, 0, false, Strings.BlackPawn);    
            BoardPieces[1, 1] = new Pawn(1, 1, false, Strings.BlackPawn);     
            BoardPieces[1, 4] = new Queen(1, 4, false, Strings.BlackQueen);  
            BoardPieces[2, 6] = new Knight(2, 6, false, Strings.BlackKnight); 
            BoardPieces[2, 7] = new Bishop(2, 7, false, Strings.BlackBishop); 
            BoardPieces[3, 3] = new Pawn(3, 3, false, Strings.BlackPawn);     
            BoardPieces[3, 5] = new Rook(3, 5, true, Strings.WhiteRook);      
            BoardPieces[4, 3] = new Knight(4, 3, true, Strings.WhiteKnight);  
            BoardPieces[4, 7] = new Pawn(4, 7, false, Strings.BlackPawn);     
            BoardPieces[5, 0] = new Bishop(5, 0, true, Strings.WhiteBishop);  
            BoardPieces[5, 2] = new Rook(5, 2, true, Strings.WhiteRook);      
            BoardPieces[5, 7] = new Bishop(5, 7, true, Strings.WhiteBishop);  
            BoardPieces[6, 0] = new Pawn(6, 0, true, Strings.WhitePawn);      
            BoardPieces[6, 2] = new Pawn(6, 2, true, Strings.WhitePawn);      
            BoardPieces[6, 3] = new Pawn(6, 3, true, Strings.WhitePawn);      
            BoardPieces[6, 5] = new Pawn(6, 5, true, Strings.WhitePawn);      
            BoardPieces[7, 2] = new King(7, 2, true, Strings.WhiteKing);      
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece p = BoardPieces[i, j];
                    p.BackgroundColor = (i + j) % 2 == 0
                        ? Color.FromArgb(Strings.BoardColorWhite)
                        : Color.FromArgb(Strings.BoardColorBlack);
                    p.Clicked += OnButtonClicked;
                    ((Grid)this.Parent).Add(p, j, i);
                    BoardUIMap[(i, j)] = p;
                }
            }
        }
        public override void MakeOpponentMove(string difficulty)
        {
            if (difficulty == Strings.Medium)
            {
                BoardPieces![0, 3] = CreatePiece(BoardPieces![1, 2]!, 0, 3);
                BoardPieces![1, 2] = new Pawn(1, 2, false, null);
                UpdateCellUI(1, 2);
                UpdateCellUI(0, 3);
            }
            else
            {
                BoardPieces![0, 5] = CreatePiece(BoardPieces![2, 6]!, 0, 5);
                BoardPieces![2, 6] = new Pawn(1, 2, false, null);
                UpdateCellUI(2, 6);
                UpdateCellUI(0, 5);
            }
        }
        protected override void OnButtonClicked(object? sender, EventArgs e)
        {
            ButtonClicked?.Invoke(this, (Piece)sender!);
        }
        public override void ShowLegalMoves(List<int[]> legalMoves)
        {
            ClearDots();
            foreach (int[] move in legalMoves)
            {
                int row = move[0];
                int col = move[1];
                Ellipse dot = new()
                {
                    Fill = Color.FromRgba(0, 0, 0, 0.2),
                    WidthRequest = 20,
                    HeightRequest = 20,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    InputTransparent = true,
                };
                ((Grid)this.Parent).Add(dot, col, row);
            }
        }
        public override void ClearDots()
        {
            Grid boardGrid = (Grid)this.Parent;
            List<IView> dotsToRemove = [.. boardGrid.Children.Where(dot => dot is Ellipse)];
            foreach (IView dot in dotsToRemove)
                boardGrid.Remove(dot);
        }
        public override void ShowHint(int row, int column)
        {
            Piece uiPiece = BoardUIMap[(row, column)];
            uiPiece.BackgroundColor = Color.FromRgba(255, 255, 0, 0.3);
        }
        public override void ShowCorrectMove(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            ShowHint(fromRow, fromColumn);
            ShowHint(toRow, toColumn);
        }
        public override void ClearBoardHighLights()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    Piece uiPiece = BoardUIMap[(i, j)];
                    if ((i + j) % 2 == 0)
                        uiPiece.BackgroundColor = Color.FromArgb(Strings.BoardColorWhite);
                    else
                        uiPiece.BackgroundColor = Color.FromArgb(Strings.BoardColorBlack);
                }
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
            Piece uiPiece = BoardUIMap[(row, col)];
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
    }
}
