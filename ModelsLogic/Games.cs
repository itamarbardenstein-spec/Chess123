using Chess.Models;
using Plugin.CloudFirestore;

namespace Chess.ModelsLogic
{
    /// Manages the collection of available chess games and handles real-time updates from Firestore
    public class Games : GamesModel
    {
        #region Public Methods
        /// Creates a new game instance, sets the user as host, and uploads the document to the database
        public override void AddGame()
        {
            IsBusy = true;
            _currentGame = new Game(SelectedGameTime)
            {
                IsHostUser = true
            };
            _currentGame.SetDocument(OnComplete);
        }
        /// Attaches a real-time listener to the games collection to detect any changes in the database
        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, OnChange!);
        }
        /// Stops and removes the active real-time listener to prevent memory leaks and unnecessary data usage
        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
        }
        #endregion
        #region Private Methods
        /// Callback triggered when a new game document is successfully created in the database
        protected override void OnComplete(Task task)
        {
            IsBusy = false;
            OnGameAdded?.Invoke(this, _currentGame!);
        }
        /// Handles data changes in the collection and triggers a query to fetch available (non-full) games
        protected override void OnChange(IQuerySnapshot snapshot, Exception error)
        {
            fbd.GetDocumentsWhereEqualTo(Keys.GamesCollection, nameof(GameModel.IsFull), false, OnComplete);
        }
        /// Processes the query results, populates the games list, and notifies the UI of the changes
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