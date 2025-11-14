 using Chess.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;

namespace Chess.ModelsLogic
{
    public class Game:GameModel
    {
        public override string OpponentName => IsHostUser ? GuestName : HostName;
        protected override GameStatus Status => IsHostUser && IsHostTurn || !IsHostUser && !IsHostTurn ?
            new GameStatus { CurrentStatus = GameStatus.Status.Play } :
            new GameStatus { CurrentStatus = GameStatus.Status.Wait };

        public Game(GameTime selectedGameTime)
        {
            HostName = new User().UserName;
            Time = selectedGameTime.Time;
            Created = DateTime.Now;
        }
        
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
        public Game()
        {
        }
        public void UpdateGuestUser(Action<Task> OnComplete)
        {
            GuestName = MyName;
            IsFull = true;
            UpdateFbJoinGame(OnComplete);
        }

        private void UpdateFbJoinGame(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(GuestName), GuestName },
                { nameof(IsFull), IsFull }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }

        public override void AddSnapshotListener()
        {
           ilr = fbd.AddSnapshotListener(Keys.GamesCollection, Id, OnChange);
        }
        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
            DeleteDocument(OnComplete);
        }

        private void OnComplete(Task task)
        {
            if(task.IsCompletedSuccessfully)
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }

        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                GuestName = updatedGame.GuestName;
                IsFull = updatedGame.IsFull;
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.Navigation.PopAsync();
                    Toast.Make(Strings.GameDeleted, ToastDuration.Long).Show(); 
                });
            }
        }

        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }

        public override void InitGrid(Grid board)
        {
            for(int i = 0;i< 8; i++)
            {
                board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto } );
                board.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
            for(int i = 0;i<8; i++)
            {
                for(int j = 0;j<8; j++)
                {
                    Piece piece = new ();
                    if(i == 0||i==1)
                    {
                        piece.IsWhite = false;
                        if(i==1)
                        {
                            piece.CurrentPieceType = Piece.PieceType.Pawn;
                        }
                        else
                        {
                            switch (j)
                            {
                                case 0:
                                case 7:
                                    piece.CurrentPieceType = Piece.PieceType.Rook;
                                    break;
                                case 1:
                                case 6:
                                    piece.CurrentPieceType = Piece.PieceType.Knight;
                                    break;
                                case 2:
                                case 5:
                                    piece.CurrentPieceType = Piece.PieceType.Bishop;
                                    break;
                                case 3:
                                    piece.CurrentPieceType = Piece.PieceType.Queen;
                                    break;
                                case 4:
                                    piece.CurrentPieceType = Piece.PieceType.King;
                                    break;
                            }
                        }
                    }
                    else if(i==6||i==7)
                    {
                        piece.IsWhite = true;
                        if (i == 6)
                        {
                            piece.CurrentPieceType = Piece.PieceType.Pawn;
                        }
                        else
                        {
                            switch (j)
                            {
                                case 0:
                                case 7:
                                    piece.CurrentPieceType = Piece.PieceType.Rook;
                                    break;
                                case 1:
                                case 6:
                                    piece.CurrentPieceType = Piece.PieceType.Knight;
                                    break;
                                case 2:
                                case 5:
                                    piece.CurrentPieceType = Piece.PieceType.Bishop;
                                    break;
                                case 3:
                                    piece.CurrentPieceType = Piece.PieceType.Queen;
                                    break;
                                case 4:
                                    piece.CurrentPieceType = Piece.PieceType.King;
                                    break;
                            }
                        }
                    }
                    if ((i + j) % 2 == 0)
                    {
                        piece.BackgroundColor = Color.FromArgb("#F0D9B5");
                    }                       
                    else
                    {
                        piece.BackgroundColor = Color.FromArgb("#B58863");
                    }                        
                    board.Add(piece, j, i);
                }
            }

        }
       
    }
}
