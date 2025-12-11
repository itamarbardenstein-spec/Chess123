using Chess.Models;

namespace Chess.ModelsLogic
{
    public abstract class Piece:PieceModel
    {
        public Piece(int row, int column, bool isWhite, string? image) : base(row, column, isWhite, image) { }
        public Piece() { }
        protected override bool PathClear(Piece[,] board, int startRow, int startCol, int endRow, int endCol, int dr, int dc)
        {
            int x = startRow + dr;
            int y = startCol + dc;
            while (x != endRow || y != endCol)
            {
                if (board[x, y] != null)
                    return false;

                x += dr;
                y += dc;
            }
            Piece start = board[startRow, startCol];
            Piece end = board[endRow, endCol];

            return end.StringImageSource == null || end.IsWhite != start.IsWhite;
        }
        
    }
}
