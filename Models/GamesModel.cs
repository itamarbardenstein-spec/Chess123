using Chess.ModelsLogic;
using Plugin.CloudFirestore;
using System.Collections.ObjectModel;

namespace Chess.Models
{
    /// Base model for managing a collection of chess games and handling real-time database updates
    public abstract class GamesModel
    {
        #region Fields
        /// Service for handling Firebase operations
        protected FbData fbd = new();
        /// Stores the reference to the currently active or selected game
        protected Game? _currentGame;
        /// Registration for the real-time collection listener in Firestore
        protected IListenerRegistration? ilr;
        #endregion
        #region Events
        /// Triggered when a new game is successfully created and added
        public EventHandler<Game>? OnGameAdded;
        /// Triggered when the list of available games is updated or modified
        public EventHandler? OnGamesChanged;
        #endregion
        #region Properties
        /// Indicates if a background process or network operation is currently running
        public bool IsBusy { get; set; }
        /// The game currently being played by the user
        public Game? CurrentGame { get => _currentGame; set => _currentGame = value; }
        /// List of active games available 
        public ObservableCollection<Game>? GamesList { get; set; } = [];
        /// Available time presets for creating a new game
        public ObservableCollection<GameTime>? GameTimes { get; set; } = [new GameTime(5), new GameTime(10), new GameTime(20)];
        /// The time setting currently selected by the user for a new game
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