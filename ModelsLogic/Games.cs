using Chess.Models;
using Plugin.CloudFirestore;
namespace Chess.ModelsLogic
{
    public class Games : GamesModel
    {
        #region Public Methods
        public override void AddGame()
        {
            IsBusy = true;
            _currentGame = new Game(SelectedGameTime)
            {
                IsHostUser = true
            };
            _currentGame.SetDocument(OnComplete);
        }
        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, OnChange!);
        }
        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
        }
        #endregion
        #region Private Methods
        protected override void OnComplete(Task task)
        {
            IsBusy = false;
            OnGameAdded?.Invoke(this, _currentGame!);
        }
        protected override void OnChange(IQuerySnapshot snapshot, Exception error)
        {
            fbd.GetDocumentsWhereEqualTo(Keys.GamesCollection, nameof(GameModel.IsFull), false, OnComplete);
        }
        protected override void OnComplete(IQuerySnapshot qs)
        {
            GamesList!.Clear();
            foreach (IDocumentSnapshot ds in qs.Documents)
            {
                Game? game = ds.ToObject<Game>();
                if (game != null)
                {
                    game.Id = ds.Id;
                    game.InitGameBoard();
                    GamesList.Add(game);
                }
            }
            OnGamesChanged?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
