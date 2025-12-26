using System.Collections.ObjectModel;
using System.Windows.Input;
using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;

namespace Chess.ViewModel
{
    public partial class MainPageVm: ObservableObject
    {
        private readonly Games games = new();
        public ICommand AddGameCommand { get; }
        public bool IsBusy => games.IsBusy;
        public ObservableCollection<GameTime>? GameTimes { get => games.GameTimes; set => games.GameTimes = value; }
        public GameTime SelectedGameTime { get => games.SelectedGameTime; set => games.SelectedGameTime = value; }
        public ObservableCollection<Game>? GamesList => games.GamesList;
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

        private void AddGame()
        {
            if (!IsBusy)
            {
                games.AddGame();
                OnPropertyChanged(nameof(IsBusy));
            }                
        }

        public MainPageVm()
        {
            AddGameCommand = new Command(AddGame);
            games.OnGameAdded += OnGameAdded;
            games.OnGamesChanged += OnGamesChanged;
        }

        private void OnGamesChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(GamesList));
        }

        private void OnGameAdded(object? sender, Game game)
        {
            OnPropertyChanged(nameof(IsBusy));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PushAsync(new GamePage(game), true);
            });
        }
        public void AddSnapshotListener()
        {
            games.AddSnapshotListener();
        }

        public void RemoveSnapshotListener()
        {
            games.RemoveSnapshotListener();
        }
    }
}

