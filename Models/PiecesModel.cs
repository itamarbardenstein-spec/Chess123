namespace Chess.Models
{
    public abstract class PiecesModel(Grid grd)
    {
        public Piece[,] Pieces = new Piece[8, 8];        
    }
}
