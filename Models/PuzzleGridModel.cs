using Chess.ModelsLogic;

namespace Chess.Models
{
    public abstract class PuzzleGridModel:Grid
    {
        public Piece[,]? BoardPieces;
        public abstract void InitPuzzleGrid(Grid board);
        public EventHandler<Piece>? ButtonClicked;
        protected abstract void OnButtonClicked(object? sender, EventArgs e);
    }
}
