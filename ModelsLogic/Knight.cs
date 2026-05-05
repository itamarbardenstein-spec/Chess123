namespace Chess.ModelsLogic
{
    /// Represents the Knight piece in the chess game
    public partial class Knight(int row, int column, bool isWhite, string? image) : Piece(row, column, isWhite, image)
    {
        /// Validates the knight's "L" shaped movement and checks if the target square is empty or occupied by an opponent
        public override bool IsMoveValid(Piece[,] board, int fromRow, int fromColumn, int toRow, int toColumn)
        {
            bool result = false;
            int rowsMoved = Math.Abs(toRow - fromRow);
            int columnsMoved = Math.Abs(toColumn - fromColumn);
            if ((rowsMoved == 2 && columnsMoved == 1) || (rowsMoved == 1 && columnsMoved == 2))
            {
                Piece start = board[fromRow, fromColumn];
                Piece target = board[toRow, toColumn];
                if (target.StringImageSource == null || start.IsWhite != target.IsWhite)
                    result = true;
            }
            return result;
        }
    }
}