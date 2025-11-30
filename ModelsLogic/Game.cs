using System.Runtime.Intrinsics.X86;
using Chess.Models;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Plugin.CloudFirestore;

namespace Chess.ModelsLogic
{
    public class Game:GameModel
    {
        public override string OpponentName => IsHostUser ? GuestName : HostName;
        protected override GameStatus Status => _status;

        public Game(GameTime selectedGameTime)
        {
            HostName = new User().UserName;
            IsHostUser = true;
            Time = selectedGameTime.Time;
            Created = DateTime.Now;
            UpdateStatus();
        }
        
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
        public Game()
        {
            UpdateStatus();
        }
        protected override void UpdateStatus()
        {
            _status.CurrentStatus = IsHostUser && IsHostTurn || !IsHostUser && !IsHostTurn ?
                GameStatus.Statuses.Play : GameStatus.Statuses.Wait;
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
                { nameof(IsFull), IsFull },
                { nameof(GuestName), GuestName }
            };
            action = Actions.Changed;
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }

        public override void AddSnapshotListener()
        {
           ilr = fbd.AddSnapshotListener(Keys.GamesCollection, Id, OnChange);
        }
        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
            action = Actions.Deleted;
            DeleteDocument(OnComplete);
        }

        private void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                if (action == Actions.Deleted)
                    OnGameDeleted?.Invoke(this, EventArgs.Empty);
                else
                    OnGameChanged?.Invoke(this, EventArgs.Empty);
        }
        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }

        public override void InitGrid(Grid board)
        {
            GameBoard = board;
            BoardPieces =new Piece[8,8];
            for (int i = 0; i < 8; i++)
            {
                board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                board.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
            for (int i = 0; i < 8; i++)
            {
                if (IsHostUser)
                {
                    BoardPieces[1, i] = new Piece(1, i, Piece.PieceType.Pawn, false, Strings.WhitePawn);
                    BoardPieces[6, i] = new Piece(6, i, Piece.PieceType.Pawn, true, Strings.BlackPawn);
                }
                else
                {
                    BoardPieces[1, i] = new Piece(1, i, Piece.PieceType.Pawn, false, Strings.BlackPawn);
                    BoardPieces[6, i] = new Piece(6, i, Piece.PieceType.Pawn, true, Strings.WhitePawn);
                }
                if (i == 0 || i == 7)
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.Rook, false, Strings.WhiteRook);
                        BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.Rook, true, Strings.BlackRook);
                    }
                    else
                    {
                        BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.Rook, false, Strings.BlackRook);
                        BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.Rook, true, Strings.WhiteRook);
                    }
                }
                else if (i == 1 || i == 6)
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.Knight, false, Strings.WhiteKnight);
                        BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.Knight, true, Strings.BlackKnight);
                    }
                    else
                    {
                        BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.Knight, false, Strings.BlackKnight);
                        BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.Knight, true, Strings.WhiteKnight);
                    }
                }
                else if (i == 2 || i == 5)
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.Bishop, false, Strings.WhiteBishop);
                        BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.Bishop, true, Strings.BlackBishop);
                    }
                    else
                    {
                        BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.Bishop, false, Strings.BlackBishop);
                        BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.Bishop, true, Strings.WhiteBishop);
                    }
                }
                else if (i == 3)
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.Queen, false, Strings.WhiteQueen);
                        BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.Queen, true, Strings.BlackQueen);
                    }
                    else
                    {
                        BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.Queen, false, Strings.BlackQueen);
                        BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.Queen, true, Strings.WhiteQueen);
                    }
                }
                else
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.King, false, Strings.WhiteKing);
                        BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.King, true, Strings.BlackKing);
                    }
                    else
                    {
                        BoardPieces[0, i] = new Piece(0, i, Piece.PieceType.King, false, Strings.BlackKing);
                        BoardPieces[7, i] = new Piece(7, i, Piece.PieceType.King, true, Strings.WhiteKing);
                    }
                }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece p = BoardPieces[i, j] ?? new Piece(i, j, null, false, null);
                    if ((i + j) % 2 == 0)
                    {
                        p.BackgroundColor = Color.FromArgb("#F0D9B5");
                    }
                    else
                    {
                        p.BackgroundColor = Color.FromArgb("#B58863");
                    }
                    p.Clicked += OnButtonClicked;
                    board.Add(p, j, i);
                    BoardUIMap[(i, j)] = p;
                }
            }
        }
        protected override void OnButtonClicked(object? sender, EventArgs e)
        {
            Piece? p = sender as Piece;
            if (_status.CurrentStatus == GameStatus.Statuses.Play)
            {
                if (ClickCount == 0)
                {
                    ClickCount++;
                    MoveFrom[0] = p!.RowIndex;
                    MoveFrom[1] = p.ColumnIndex;
                }
                else
                {
                    if (p == null || p.IsWhite != BoardPieces?[MoveFrom[0], MoveFrom[1]].IsWhite)
                    {
                        if (Move.IsMoveValid(BoardPieces!,MoveFrom[0],MoveFrom[1],p!.RowIndex,p.ColumnIndex))
                        Play(p!.RowIndex, p.ColumnIndex, true);
                        else
                        {
                            MainThread.InvokeOnMainThreadAsync(() =>
                            {
                                Toast.Make(Strings.InvalidMove, ToastDuration.Long, 14).Show();
                            });
                        }
                    }                    
                    ClickCount = 0;
                }                                     
            }            
        }
        protected override void Play(int rowIndex, int columnIndex, bool MyMove)
        {
            Piece PieceToMove = BoardPieces![MoveFrom[0], MoveFrom[1]];
            BoardPieces[rowIndex, columnIndex] = PieceToMove;
            BoardPieces[MoveFrom[0], MoveFrom[1]] = null;
            UpdateCellUI(MoveFrom[0], MoveFrom[1]);
            UpdateCellUI(rowIndex, columnIndex);          
            if (MyMove)
            {
                MoveTo[0] = rowIndex;
                MoveTo[1] = columnIndex;
                _status.UpdateStatus();
                IsHostTurn = !IsHostTurn;
                UpdateFbMove();
            }
            else
            {
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        protected override void UpdateFbMove()
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(MoveFrom), MoveFrom },
                { nameof(MoveTo), MoveTo },
                { nameof(IsHostTurn), IsHostTurn }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                IsFull = updatedGame.IsFull;
                GuestName = updatedGame.GuestName;
                OnGameChanged?.Invoke(this, EventArgs.Empty);
                IsHostTurn = updatedGame.IsHostTurn;
                MoveFrom = updatedGame.MoveFrom;
                MoveTo = updatedGame.MoveTo;
                UpdateStatus();
                if (_status.CurrentStatus == GameStatus.Statuses.Play && updatedGame.MoveFrom[0] != Keys.NoMove)
                {
                    MoveFrom[0] = 7 - MoveFrom[0];
                    MoveTo[0] = 7 - MoveTo[0];
                    Play(MoveTo[0],MoveTo[1], false);
                }
            }                  
            else
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.Navigation.PopAsync();
                    Toast.Make(Strings.GameDeleted, ToastDuration.Long, 14).Show();
                });

            }
        }
        private void UpdateCellUI(int row, int col)
        {
            if (!BoardUIMap.TryGetValue((row, col), out Piece? uiPiece))
                return;

            Piece modelPiece = BoardPieces![row, col];

            if (modelPiece == null)
            {
                uiPiece.Source = null;
                uiPiece.CurrentPieceType = null;
                uiPiece.IsWhite = false;
            }
            else
            {
                uiPiece.Source = modelPiece.StringImageSource;
                uiPiece.CurrentPieceType = modelPiece.CurrentPieceType;
                uiPiece.IsWhite = modelPiece.IsWhite;
            }
        }
    }
}
