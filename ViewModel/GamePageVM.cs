using Chess.Models;
using Chess.ModelsLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace Chess.ViewModel
{
    public partial class GamePageVM : ObservableObject
    {
        private readonly GameGrid grdBoard = [];
        private readonly Game game;
        public string MyName => game.MyName;
        public string StatusMessage => game.StatusMessage;
        public string MyTime => game.IsHostUser? FormatTime(game.BlackTimeLeft) :FormatTime(game.WhiteTimeLeft);
        public string OpponentTime => game.IsHostUser? FormatTime(game.WhiteTimeLeft) : FormatTime(game.BlackTimeLeft);
        public string OpponentName=> game.OpponentName;
        public string TimeLeft => game.TimeLeft;
        private static string FormatTime(long millis)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(millis);
            return $"{t.Minutes:D2}:{t.Seconds:D2}";
        }
        public GamePageVM(Game game, Grid board)
        {
            this.game = game;
            this.game.gameGrid = grdBoard;
            game.InvalidMove += InvalidMove;
            game.OnGameChanged += OnGameChanged;
            game.OnGameDeleted += OnGameDeleted;
            game.DisplayChanged += OnDisplayChanged;
            game.TimeLeftChanged += OnTimeLeftChanged;
            grdBoard.InitGrid(board,game.IsHostUser);
            grdBoard.ButtonClicked += OnButtonClicked;             
            if (!game.IsHostUser)   
                game.UpdateGuestUser(OnComplete);
        }       
        private void OnTimeLeftChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(MyTime));
        }
        private void InvalidMove(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.InvalidMove, ToastDuration.Long, 14).Show();
            });
        }

        private void OnGameDeleted(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.GameDeleted, ToastDuration.Long).Show();
            });
        }
        private void OnDisplayChanged(object? sender, DisplayMoveArgs e)
        {
            grdBoard.UpdateDisplay(e);
        }
        private void OnButtonClicked(object? sender, Piece e)
        {
            game.CheckMove(e);
        }
        private void OnGameChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(OpponentName));
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(OpponentTime));
            OnPropertyChanged(nameof(MyTime));
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
