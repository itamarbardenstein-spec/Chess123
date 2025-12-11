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
            BoardPieces = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                board.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
            for (int i = 0; i < 8; i++)
            {
                if (IsHostUser)
                {
                    BoardPieces[1, i] = new Pawn(1, i, true, Strings.WhitePawn);
                    BoardPieces[6, i] = new Pawn(6, i, false, Strings.BlackPawn);
                }
                else
                {
                    BoardPieces[1, i] = new Pawn(1, i, false, Strings.BlackPawn);
                    BoardPieces[6, i] = new Pawn(6, i, true, Strings.WhitePawn);
                }
                if (i == 0 || i == 7)
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i] = new Rook(0, i, true, Strings.WhiteRook);
                        BoardPieces[7, i] = new Rook(7, i, false, Strings.BlackRook);
                    }
                    else
                    {
                        BoardPieces[0, i] = new Rook(0, i, false, Strings.BlackRook);
                        BoardPieces[7, i] = new Rook(7, i, true, Strings.WhiteRook);
                    }
                }
                else if (i == 1 || i == 6)
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i] = new Knight(0, i, true, Strings.WhiteKnight);
                        BoardPieces[7, i] = new Knight(7, i, false, Strings.BlackKnight);
                    }
                    else
                    {
                        BoardPieces[0, i] = new Knight(0, i, false, Strings.BlackKnight);
                        BoardPieces[7, i] = new Knight(7, i, true, Strings.WhiteKnight);
                    }
                }
                else if (i == 2 || i == 5)
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i] = new Bishop(0, i, true, Strings.WhiteBishop);
                        BoardPieces[7, i] = new Bishop(7, i, false, Strings.BlackBishop);
                    }
                    else
                    {
                        BoardPieces[0, i] = new Bishop(0, i, false, Strings.BlackBishop);
                        BoardPieces[7, i] = new Bishop(7, i, true, Strings.WhiteBishop);
                    }
                }
                else if (i == 3)
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i+1] = new Queen(0, i, true, Strings.WhiteQueen);
                        BoardPieces[7, i+1] = new Queen(7, i, false, Strings.BlackQueen);
                    }
                    else
                    {
                        BoardPieces[0, i] = new Queen(0, i, false, Strings.BlackQueen);
                        BoardPieces[7, i] = new Queen(7, i, true, Strings.WhiteQueen);
                    }
                }
                else
                {
                    if (IsHostUser)
                    {
                        BoardPieces[0, i-1] = new King(0, i, true, Strings.WhiteKing);
                        BoardPieces[7, i-1] = new King(7, i, false, Strings.BlackKing);
                    }
                    else
                    {
                        BoardPieces[0, i] = new King(0, i, false, Strings.BlackKing);
                        BoardPieces[7, i] = new King(7, i, true, Strings.WhiteKing);
                    }
                }
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Piece p = BoardPieces[i, j] ?? new Pawn(i, j, false, null);
                    if ((i + j) % 2 == 0)
                    {
                        p!.BackgroundColor = Color.FromArgb("#F0D9B5");
                    }
                    else
                    {
                        p!.BackgroundColor = Color.FromArgb("#B58863");
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
                    if (p?.StringImageSource!=null)
                    {
                        ClickCount++;
                        MoveFrom[0] = p!.RowIndex;
                        MoveFrom[1] = p.ColumnIndex;
                    }                   
                }
                else
                {                  
                    if (BoardPieces![MoveFrom[0],MoveFrom[1]].IsMoveValid(BoardPieces!,MoveFrom[0],MoveFrom[1],p!.RowIndex,p.ColumnIndex))
                    Play(p!.RowIndex, p.ColumnIndex, true);
                    else
                    {
                        MainThread.InvokeOnMainThreadAsync(() =>
                        {
                            Toast.Make(Strings.InvalidMove, ToastDuration.Long, 14).Show();
                        });
                    }                   
                    ClickCount = 0;
                }                                     
            }            
        }
        protected override void Play(int rowIndex, int columnIndex, bool MyMove)
        {
            Piece PieceToMove = BoardPieces![MoveFrom[0], MoveFrom[1]];
            //bool isCastling = PieceToMove is King && Math.Abs(columnIndex - MoveFrom[1]) == 2;
            if(PieceToMove is Pawn)
            {
                BoardPieces[rowIndex, columnIndex]=new Pawn(rowIndex, columnIndex, PieceToMove.IsWhite, PieceToMove.StringImageSource);
            }
            else
            {
                BoardPieces[rowIndex, columnIndex] = CreatePiece(PieceToMove, rowIndex, columnIndex);
            }              
            //BoardPieces[rowIndex, columnIndex].HasMoved = true;
            BoardPieces[MoveFrom[0], MoveFrom[1]] = null;
            UpdateCellUI(MoveFrom[0], MoveFrom[1]);
            UpdateCellUI(rowIndex, columnIndex);
            //if (isCastling)
            //{
            //    if (columnIndex == 6)
            //    {
            //        Piece rook = BoardPieces[rowIndex, 7];
            //        BoardPieces[rowIndex, 5] =CreatePiece(rook, rowIndex, 5);
            //        //BoardPieces[rowIndex, 5].HasMoved = true;
            //        //BoardPieces[rowIndex, 7] = null;
            //        //BoardPieces[rowIndex, 7] = null;
            //        UpdateCellUI(rowIndex, 7);
            //        UpdateCellUI(rowIndex, 5);
            //    }
            //    else if (columnIndex == 2)
            //    {
            //        Piece rook = BoardPieces[rowIndex, 0];
            //        BoardPieces[rowIndex, 3] =CreatePiece(rook, rowIndex, 3);
            //        //BoardPieces[rowIndex, 3].HasMoved = true;
            //        //BoardPieces[rowIndex, 0] = null;
            //        //BoardPieces[rowIndex, 0] = null;
            //        UpdateCellUI(rowIndex, 0);
            //        UpdateCellUI(rowIndex, 3);
            //    }
            //}
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
                    MoveFrom[1] = 7 - MoveFrom[1];
                    MoveTo[1] = 7 - MoveTo[1];
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
        public static Piece CreatePiece(Piece original, int row, int col)
        {
            bool isWhite = original.IsWhite;
            string? img = original.StringImageSource;

            return original switch
            {
                Pawn => new Pawn(row, col, isWhite, img),
                Rook => new Rook(row, col, isWhite, img),
                Knight => new Knight(row, col, isWhite, img),
                Bishop => new Bishop(row, col, isWhite, img),
                Queen => new Queen(row, col, isWhite, img),
                King => new King(row, col, isWhite, img),

                _ => throw new Exception()
            };
        }
        private void UpdateCellUI(int row, int col)
        {
            if (!BoardUIMap.TryGetValue((row, col), out PieceModel? uiPiece))
                return;

            Piece modelPiece = BoardPieces![row, col];

            if (modelPiece == null)
            {
                uiPiece.StringImageSource = null;
                uiPiece.Source = null;
                uiPiece.IsWhite = false;
            }
            else
            {
                uiPiece.StringImageSource = modelPiece.StringImageSource;
                uiPiece.Source = modelPiece.StringImageSource;
                uiPiece.IsWhite = modelPiece.IsWhite;
            }
        }
    }
}
