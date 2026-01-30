using Chess.Models;
using Microsoft.Maui.Controls.Shapes;

namespace Chess.ModelsLogic
{
    public partial class PuzzleGrid:PuzzleGridModel
    {
        public override void InitPuzzleGrid(Grid board)
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
                for (int j = 0; j < 8; j++)
                {
                    BoardPieces[i, j] = new Pawn(i, j, false, null);
                }
            }

            // 3. הצבת הכלים במיקומים הסופיים

            // שורה 0 (העליונה)
            BoardPieces[0, 0] = new Rook(0, 0, true, Strings.WhiteRook);
            BoardPieces[0, 2] = new Bishop(0, 2, true, Strings.WhiteBishop);
            BoardPieces[0, 3] = new King(0, 3, true, Strings.WhiteKing);
            BoardPieces[0, 5] = new Bishop(0, 5, true, Strings.WhiteBishop);
            BoardPieces[0, 6] = new Knight(0, 6, true, Strings.WhiteKnight);
            BoardPieces[0, 7] = new Rook(0, 7, true, Strings.WhiteRook);

            // שורה 1
            for (int i = 0; i < 8; i++)
            {
                if (i != 3 && i != 4)
                    BoardPieces[1, i] = new Pawn(1, i, true, Strings.WhitePawn);
            }

            // שורה 2
            BoardPieces[2, 1] = new Knight(2, 1, true, Strings.WhiteKnight);

            // שורה 3 - המלכה הלבנה (הוזזה שתי משבצות ימינה מטור 2 לטור 4)
            BoardPieces[3, 4] = new Queen(3, 4, true, Strings.WhiteQueen);

            // שורה 4 - הפרש השחור נשאר בטור 3 (אחד שמאלה מהמקור)
            BoardPieces[4, 3] = new Knight(4, 3, false, Strings.BlackKnight);
            BoardPieces[4, 4] = new Pawn(4, 4, true, Strings.WhitePawn);

            // שורה 5
            BoardPieces[5, 2] = new Pawn(5, 2, false, Strings.BlackPawn);

            // שורה 6 - המלכה השחורה נשארת בטור 3 (אחד שמאלה מהמקור)
            BoardPieces[6, 3] = new Queen(6, 3, false, Strings.BlackQueen);
            for (int i = 0; i < 8; i++)
            {
                if (i == 2 || i == 3 || i == 4) continue;
                BoardPieces[6, i] = new Pawn(6, i, false, Strings.BlackPawn);
            }

            // שורה 7 (התחתונה)
            BoardPieces[7, 0] = new Rook(7, 0, false, Strings.BlackRook);
            BoardPieces[7, 1] = new Knight(7, 1, false, Strings.BlackKnight);
            BoardPieces[7, 2] = new Bishop(7, 2, false, Strings.BlackBishop);
            BoardPieces[7, 3] = new King(7, 3, false, Strings.BlackKing);
            BoardPieces[7, 5] = new Bishop(7, 5, false, Strings.BlackBishop);
            BoardPieces[7, 7] = new Rook(7, 7, false, Strings.BlackRook);

            // 4. רינדור הלוח
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
                }
            }
        }
        protected override void OnButtonClicked(object? sender, EventArgs e)
        {
            ButtonClicked?.Invoke(this, (Piece)sender!);
        }
        public void ShowLegalMoves(List<int[]> legalMoves)
        {
            ClearDots();
            if (this.Parent is Grid boardGrid)
            {
                foreach (int[] move in legalMoves)
                {
                    int row = move[0];
                    int col = move[1];
                    var dot = new Ellipse
                    {
                        Fill = Color.FromRgba(0, 0, 0, 0.2),
                        WidthRequest = 20,
                        HeightRequest = 20,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        InputTransparent = true,
                        ClassId = "LegalMoveDot"
                    };
                    boardGrid.Add(dot, col, row);
                }
            }
        }
        public void ClearDots()
        {
            if (this.Parent is Grid boardGrid)
            {
                var dotsToRemove = boardGrid.Children
                    .Where(c => c is Ellipse e && e.ClassId == "LegalMoveDot")
                    .ToList();

                foreach (var dot in dotsToRemove)
                {
                    boardGrid.Remove(dot);
                }
            }
        }
        public void ShowHint()
        {
            BoardPieces![4, 3].BackgroundColor = Color.FromRgba(255, 255, 0, 0.3);
        }
        public void ShowCorrectMove()
        {
            BoardPieces![4, 3].BackgroundColor = Color.FromRgba(255, 255, 0, 0.3);
            BoardPieces![2,2].BackgroundColor = Color.FromRgba(255, 255, 0, 0.3);
        }
    }
}
