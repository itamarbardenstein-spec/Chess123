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
            new GameStatus { CurrentStatus = GameStatus.Status.Play }:
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
                BoardPieces[1, i] = new Piece(1, i, Piece.PieceType.Pawn, false, Strings.BlackPawn);
                BoardPieces[6, i] = new Piece(6, i, Piece.PieceType.Pawn, true, Strings.WhitePawn);
                if (i == 0 || i == 7)
                {
                    BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.Rook, false, Strings.BlackRook);
                    BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.Rook, true, Strings.WhiteRook);
                }
                else if (i == 1 || i == 6)
                {
                    BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.Knight, false, Strings.BlackKnight);
                    BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.Knight, true, Strings.WhiteKnight);
                }
                else if (i == 2 || i == 5)
                {
                    BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.Bishop, false, Strings.BlackBishop);
                    BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.Bishop, true, Strings.WhiteBishop);
                }
                else if (i == 3)
                {
                    BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.Queen, false, Strings.BlackQueen);
                    BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.Queen, true, Strings.WhiteQueen);
                }
                else
                {
                    BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.King, false, Strings.BlackKing);
                    BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.King, true, Strings.WhiteKing);
                }
            }
            for(int i = 0;i<8; i++)
            {
                for(int j = 0;j<8; j++)
                {
                    Piece p = BoardPieces[i, j];
                    if ((i + j) % 2 == 0)
                    {
                        p.BackgroundColor = Color.FromArgb("#F0D9B5");
                    }                       
                    else
                    {
                        p.BackgroundColor = Color.FromArgb("#B58863");
                    }                                                        
                    board.Add(p, j, i);
                }
            }
        }
       
    }
}
