using Chess.Models;
using Chess.ModelsLogic;
using Chess.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;

namespace Chess.ViewModel
{
    public partial class GamePageVM : ObservableObject
    {
        private readonly GameGrid grdBoard = [];
        private readonly Game game;
        public string MyName => game.MyName;
        public string StatusMessage => game.StatusMessage;
        public string MyTime => game.IsHostUser ? FormatTime(game.BlackTimeLeft) : FormatTime(game.WhiteTimeLeft);
        public string OpponentTime => game.IsHostUser ? FormatTime(game.WhiteTimeLeft) : FormatTime(game.BlackTimeLeft);
        public string OpponentName => game.OpponentName;
        private static string FormatTime(long millis)
        {
            TimeSpan t = TimeSpan.FromMilliseconds(millis);
            return $"{t.Minutes:D2}:{t.Seconds:D2}";
        }
        public GamePageVM(Game game, Grid board)
        {
            this.game = game;
            game.InvalidMove += InvalidMove;
            game.OnGameChanged += OnGameChanged; 
            game.OnGameDeleted += OnGameDeleted;
            //game.OnPromotion += Promotion;
            game.DisplayChanged += OnDisplayChanged;
            game.OnCastling += Castling;
            //game.PawnPromotionGrid += PromotionGrid;
            game.TimeLeftChanged += OnTimeLeftChanged;
            game.GameOver += OnGameOver;
            grdBoard.InitGrid(board, game.IsHostUser);
            grdBoard.ButtonClicked += OnButtonClicked;
            if (!game.IsHostUser)
                game.UpdateGuestUser(OnComplete);
        }

        //private void Promotion(object? sender, OnPromotionArgs e)
        //{
        //    Shell.Current.ShowPopup(new PawnPromotionPopup(e.Row, e.Column, game));
        //}
        //private void PromotionGrid(object? sender, PawnPromotionArgs e)
        //{
        //    grdBoard.Promotion(e.IsHostUser, e.Row, e.Column, e.PieceToSwitch, e.MyMove);
        //}
        private void Castling(object? sender, CastlingArgs e)
        {
            grdBoard?.Castling(e.Right, e.IsHostUser, e.MyMove);
        }
        private void OnGameOver(object? sender, GameOverArgs e)
        {
            string reason = game.GameOverMessageReason(e.IWon, e.IsCheckmate);
            string title = game.GameOverMessageTitle(e.IWon);
            Shell.Current.ShowPopup(new GameResultPopup(title, reason));
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
            if (!task.IsCompletedSuccessfully)
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
