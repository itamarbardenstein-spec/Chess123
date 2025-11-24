using Chess.Models;
using Chess.ModelsLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Chess.ViewModel
{
    public partial class GamePageVM : ObservableObject
    {
        private readonly Game game;
        public string MyName => game.MyName;
        public string StatusMessage => game.StatusMessage;
        public string OpponentName=> game.OpponentName;
        public GamePageVM(Game game, Grid board)
        {
            game.OnGameChanged += OnGameChanged;
            game.InitGrid(board);
            this.game = game;
            if (!game.IsHostUser)
                game.UpdateGuestUser(OnComplete);           
        }

        private void OnGameChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(OpponentName));
            OnPropertyChanged(nameof(StatusMessage));
        }

        private void OnComplete(Task task)
        {
            if(!task.IsCompletedSuccessfully)
               Toast.Make(Strings.JoinGameErr, ToastDuration.Long).Show();
        }

        public void AddSnapshotListener()
        {
           game.AddSnapshotListener();
        }

        public void RemoveSnapshotListener()
        {
            game.RemoveSnapshotListener();
        }
    }
}
