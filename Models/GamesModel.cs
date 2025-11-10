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
        public EventHandler<Game>? OnGameAdded;
        public EventHandler? OnGamesChanged;
        public abstract void AddSnapshotListener();
        public abstract void RemoveSnapshotListener();
    }
}
