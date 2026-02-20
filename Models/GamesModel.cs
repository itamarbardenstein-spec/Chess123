using Chess.ModelsLogic;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;

namespace Chess.Models
{
    public abstract class GamesModel
    {
        #region Fields
        protected FbData fbd = new();
        protected Game? _currentGame;
        protected IListenerRegistration? ilr;
        #endregion
        #region Events
        public EventHandler<Game>? OnGameAdded;
        public EventHandler? OnGamesChanged;
        #endregion
        #region Properties
        public bool IsBusy { get; set; }
        public Game? CurrentGame { get => _currentGame; set => _currentGame = value; }
        public ObservableCollection<Game>? GamesList { get; set; } = [];
        public ObservableCollection<GameTime>? GameTimes { get; set; } = [new GameTime(5), new GameTime(10), new GameTime(20)];
        public GameTime SelectedGameTime { get; set; } = new GameTime();
        #endregion
        #region Public Methods
        public abstract void AddSnapshotListener();
        public abstract void AddGame();
        public abstract void RemoveSnapshotListener();
        #endregion
        #region Private Methods
        protected abstract void OnComplete(Task task);
        protected abstract void OnChange(IQuerySnapshot snapshot, Exception error);
        protected abstract void OnComplete(IQuerySnapshot qs);
        #endregion
    }
}
