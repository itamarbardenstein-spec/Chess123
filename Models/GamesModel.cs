using Chess.ModelsLogic;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;

namespace Chess.Models
{
    public abstract class GamesModel
    {
        protected FbData fbd = new();
        protected Game? currentGame;
        protected IListenerRegistration? ilr;
        public bool IsBusy { get; set; }
        public Game? CurrentGame { get=> currentGame; set => currentGame = value;}
        public ObservableCollection<Game>? GamesList { get; set; } = [];
        public ObservableCollection<GameTime>? GameTimes { get; set; } = [new GameTime(5), new GameTime(10), new GameTime(20)];
        public GameTime SelectedGameTime { get; set; } = new GameTime();
        public EventHandler<Game>? OnGameAdded;
        public EventHandler? OnGamesChanged;
        public abstract void AddSnapshotListener();
        public abstract void RemoveSnapshotListener();
        
    }
}
