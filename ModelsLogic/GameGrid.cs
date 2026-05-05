using Chess.Models;
using Microsoft.Maui.Controls.Shapes;

namespace Chess.ModelsLogic
{
    /// Logic and UI management for the chess board grid
    public partial class GameGrid : GameGridModel
    {
        #region Public Methods
        /// Initializes the board structure, row/column definitions, and sets initial piece positions based on user role
        public override void InitGrid(Grid board, bool IsHostUser)
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
                // Assign pawn positions based on whether the user is host or guest
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
                // Place major pieces in the back ranks
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
                    // Setup Queen positions
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
                    // Setup King positions
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
            // Generate visual UI squares for the entire grid
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    if (BoardPieces[i, j] == null)
                        BoardPieces[i, j] = new Pawn(i, j, false, null);
                    Piece p = BoardPieces[i, j];
                    // Apply checkered board coloring based on coordinates
                    p!.BackgroundColor = Color.FromArgb(((i + j) % 2 == 0) ? Strings.BoardColorWhite : Strings.BoardColorBlack);
                    p.Clicked += OnButtonClicked;
                    ((Grid)this.Parent).Add(p, j, i);
                    BoardUIMap[(i, j)] = p;
                }
        }
        /// Updates the visual board state after a move is made
        public override void UpdateDisplay(DisplayMoveArgs e)
        {
            // Reset highlights and transfer piece data to new coordinates
            ClearBoardHighLights();
            BoardPieces![e.ToRow, e.ToColumn] = CreatePiece(BoardPieces![e.FromRow, e.FromColomn]!, e.ToRow, e.ToColumn);
            BoardPieces![e.FromRow, e.FromColomn] = new Pawn(e.FromRow, e.FromColomn, false, null);
            // Sync UI elements for both source and destination cells
            UpdateCellUI(e.FromRow, e.FromColomn);
            UpdateCellUI(e.ToRow, e.ToColumn);
            HighlightMove(e.FromRow, e.FromColomn, e.ToRow, e.ToColumn);
        }
        /// Creates a new instance of a piece at a target location while preserving its current state
        public override Piece CreatePiece(Piece original, int row, int col)
        {
            bool isWhite = original.IsWhite;
            string? img = original.StringImageSource;
            // Re-instantiate piece type while maintaining movement history flags
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
                _ => throw new Exception("Unknown piece type")
            };
        }
        /// Handles the visual execution of the castling move for both host and guest perspectives
        public override void Castling(bool right, bool isHostUser, bool MyMove)
        {
            // Logic for moving the Rook during a castling maneuver
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
        /// Visually promotes a pawn to a queen at the end of the board
        public override void Promotion(bool IsHostUser, int row, int column)
        {
            // Replace pawn with a queen of the appropriate color
            if (IsHostUser)
                BoardPieces![row, column] = new Queen(row, column, false, Strings.BlackQueen);
            else
                BoardPieces![row, column] = new Queen(row, column, true, Strings.WhiteQueen);
            UpdateCellUI(row, column);
        }
        /// Displays indicators for all valid moves available for the selected piece
        public override void ShowLegalMoves(List<int[]> legalMoves)
        {
            ClearDots();
            foreach (int[] move in legalMoves)
            {
                int row = move[0];
                int col = move[1];
                // Select visual style: solid dot for empty square or ring for capture
                bool isCapture = BoardPieces![row, col].StringImageSource != null;
                VisualElement indicator;
                if (isCapture)
                    indicator = new Ellipse()
                    {
                        Stroke = Color.FromRgba(0, 0, 0, 0.2),
                        StrokeThickness = 5,
                        WidthRequest = 45,
                        HeightRequest = 45,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        InputTransparent = true
                    };
                else
                    indicator = new Ellipse()
                    {
                        Fill = Color.FromRgba(0, 0, 0, 0.2),
                        WidthRequest = 20,
                        HeightRequest = 20,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        InputTransparent = true
                    };
                ((Grid)this.Parent).Add(indicator, col, row);
            }
        }
        /// Removes all legal move indicators (dots and rings) from the board
        public override void ClearDots()
        {
            // Remove all Ellipse objects representing move indicators from the grid
            Grid boardGrid = (Grid)this.Parent;
            List<IView> dotsToRemove = [.. boardGrid.Children.Where(dot => dot is Ellipse)];
            foreach (IView dot in dotsToRemove)
                boardGrid.Remove(dot);
        }
        /// Applies a highlight color to a specific square on the grid
        public override void HighlightSquare(int row, int column)
        {
            Piece uiPiece = BoardUIMap[(row, column)];
            uiPiece.BackgroundColor = Color.FromRgba(255, 255, 0, 0.3);
        }
        /// Highlights both the starting and ending squares of the last move
        public override void HighlightMove(int fromRow, int fromColumn, int toRow, int toColumn)
        {
            HighlightSquare(fromRow, fromColumn);
            HighlightSquare(toRow, toColumn);
        }
        /// Resets all squares on the board to their default checkered colors
        public override void ClearBoardHighLights()
        {
            // Reset background color of all squares based on grid position
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
        /// Restores the original color of a single specific square
        public override void ClearSquareHighlight(int row, int col)
        {
            Piece uiPiece = BoardUIMap[(row, col)];
            if ((row + col) % 2 == 0)
                uiPiece.BackgroundColor = Color.FromArgb(Strings.BoardColorWhite);
            else
                uiPiece.BackgroundColor = Color.FromArgb(Strings.BoardColorBlack);
            if (BoardPieces![row, col] != null)
                BoardPieces[row, col].BackgroundColor = uiPiece.BackgroundColor;
        }
        #endregion
        #region Private Methods
        /// Invokes the click event when a piece or square is tapped
        protected override void OnButtonClicked(object? sender, EventArgs e)
        {
            ButtonClicked?.Invoke(this, (Piece)sender!);
        }
        /// Synchronizes the UI element's image and properties with the logical piece data
        protected override void UpdateCellUI(int row, int col)
        {
            // Sync image source and color properties between logic model and UI element
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
        #endregion
    }
}