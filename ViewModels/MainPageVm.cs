using System.Collections.ObjectModel;
using System.Windows.Input;
using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;

namespace Chess.ViewModel
{
    /// ViewModel for the main lobby, managing the list of available games and game creation
    public partial class MainPageVm : ObservableObject
    {
        #region Fields
        /// Logic handler for managing multiple game instances and lobby state
        private readonly Games games = new();
        #endregion
        #region Commands
        /// Command to create and host a new game session
        public ICommand AddGameCommand { get; }
        /// Command to display the rules and game instructions to the user
        public ICommand InstructionsCommand { get; private set; }
        #endregion
        #region Properties
        /// Indicates if a lobby operation (like creating a game) is currently in progress
        public bool IsBusy => games.IsBusy;
        /// Collection of available time control settings for new games
        public ObservableCollection<GameTime>? GameTimes { get => games.GameTimes; set => games.GameTimes = value; }
        /// The currently selected time control for a new game
        public GameTime SelectedGameTime { get => games.SelectedGameTime; set => games.SelectedGameTime = value; }
        /// The list of active games available in the lobby
        public ObservableCollection<Game>? GamesList => games.GamesList;
        /// Handles game selection from the list and triggers navigation to the board
        public Game? SelectedItem
        {
            get => games.CurrentGame;
            set
            {
                if (value != null)
                {
                    games.CurrentGame = value;
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.Navigation.PushAsync(new GamePage(value), true);
                    });
                }
            }
        }
        #endregion
        #region Constructor
        /// Initializes lobby commands and subscribes to game list update events
        public MainPageVm()
        {
            AddGameCommand = new Command(AddGame);
            games.OnGameAdded += OnGameAdded;
            games.OnGamesChanged += OnGamesChanged;
            InstructionsCommand = new Command(ShowInstructionsPrompt);
        }
        #endregion
        #region Public Methods
        /// Displays a native alert dialog containing the game instructions
        public static void ShowInstructionsPrompt(object obj)
        {
            Application.Current!.MainPage!.DisplayAlert(Strings.Instructions, Strings.InstructionsTxt, Strings.Ok);
        }
        /// Starts listening for real-time updates to the games collection in the database
        public void AddSnapshotListener()
        {
            games.AddSnapshotListener();
        }
        /// Stops listening for database updates to conserve resources
        public void RemoveSnapshotListener()
        {
            games.RemoveSnapshotListener();
        }
        #endregion
        #region Private Methods
        /// Initiates the creation of a new game if the system is not currently busy
        private void AddGame()
        {
            if (!IsBusy)
            {
                games.AddGame();
                OnPropertyChanged(nameof(IsBusy));
            }
        }
        /// Refreshes the UI list when games are added, removed, or updated in the lobby
        private void OnGamesChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(GamesList));
        }
        /// Navigates the host to the game page immediately after their game is successfully created
        private void OnGameAdded(object? sender, Game game)
        {
            OnPropertyChanged(nameof(IsBusy));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PushAsync(new GamePage(game), true);
            });
        }
        #endregion        
    }
}