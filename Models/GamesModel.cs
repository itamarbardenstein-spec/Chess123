using Chess.ModelsLogic;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;

namespace Chess.Models
{
    public abstract class GamesModel
    {
        protected FbData fbd = new();
        protected Game? _currentGame;
        protected IListenerRegistration? ilr;
        public bool IsBusy { get; set; }
        public Game? CurrentGame { get => _currentGame; set => _currentGame = value; }
        public ObservableCollection<Game>? GamesList { get; set; } = [];
        public ObservableCollection<GameTime>? GameTimes { get; set; } = [new GameTime(5), new GameTime(10), new GameTime(20)];
        public GameTime SelectedGameTime { get; set; } = new GameTime();
        public EventHandler<Game>? OnGameAdded;
        public EventHandler? OnGamesChanged;
        protected abstract void OnComplete(Task task);
        protected abstract void OnChange(IQuerySnapshot snapshot, Exception error);
        protected abstract void OnComplete(IQuerySnapshot qs);
        public abstract void AddSnapshotListener();
        public abstract void AddGame();
        public abstract void RemoveSnapshotListener();
        
    }
}
