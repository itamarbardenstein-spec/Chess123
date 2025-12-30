    using Chess.Models;
    using CommunityToolkit.Mvvm.Messaging;
    using Chess.Views;
    using CommunityToolkit.Maui.Views;
    using Plugin.CloudFirestore;

    namespace Chess.ModelsLogic
    {
        public class Game : GameModel
        {
            public override string OpponentName => IsHostUser ? GuestName : HostName;
            protected override GameStatus Status => _status;
            public Game() { RegisterTimer(); }
            public Game(GameTime selectedGameTime)
            {
                RegisterTimer();
                Created = DateTime.Now;
                HostName = new User().UserName;
                IsHostUser = true;
                Time = selectedGameTime.Time;
                UpdateStatus();
                long totalMillis = Time * 60 * 1000;
                WhiteTimeLeft = totalMillis;
                BlackTimeLeft = totalMillis;
            }
            private void RegisterTimer()
            {
                WeakReferenceMessenger.Default.Register<AppMessage<long>>(this, (r, m) =>
                {
                    OnMessageReceived(m.Value);
                });
            }
            private void OnMessageReceived(long timeLeft)
            {
                // שמירה למשתנה הנכון כדי שלא יאבד
                if (IsHostUser) // או בדיקה אחרת של הצבע שלך
                    BlackTimeLeft = timeLeft;
                else                   
                    WhiteTimeLeft = timeLeft;
            // עדכון התצוגה
                TimeLeft = double.Round(timeLeft / 1000.0, 1).ToString();
                TimeLeftChanged?.Invoke(this, EventArgs.Empty);
            }

        public override void SetDocument(Action<Task> OnComplete)
            {
                Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
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
                UpdateStatus();
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
            public override void CheckMove(Piece p)
            {
                if (_status.CurrentStatus == GameStatus.Statuses.Play)
                {
                    if (ClickCount == 0)
                    {
                        if (p?.StringImageSource != null && (IsHostUser ? (!p.IsWhite) : p.IsWhite))
                        {
                            ClickCount++;
                            MoveFrom[0] = p.RowIndex;
                            MoveFrom[1] = p.ColumnIndex;
                        }
                    }
                    else
                    {
                        if (gameGrid!.BoardPieces![MoveFrom[0], MoveFrom[1]].IsMoveValid(gameGrid?.BoardPieces!, MoveFrom[0], MoveFrom[1], p.RowIndex, p.ColumnIndex))
                            Play(p.RowIndex, p.ColumnIndex, true);
                        else
                            InvalidMove?.Invoke(this, EventArgs.Empty);
                        ClickCount = 0;
                    }
                }
            }
            public override void Play(int rowIndex, int columnIndex, bool MyMove)
            {
                if (gameGrid?.BoardPieces![MoveFrom[0], MoveFrom[1]] is King king&&MyMove)
                        king.HasKingMoved = true;
                if (gameGrid?.BoardPieces![MoveFrom[0], MoveFrom[1]] is Rook rook)
                {
                    if (MoveFrom[1] == 0) rook.HasLeftRookMoved = true;
                    else rook.HasRightRookMoved = true;
                }
                Piece PieceToMove = gameGrid?.BoardPieces![MoveFrom[0], MoveFrom[1]]!;
                if (PieceToMove is King && Math.Abs(MoveFrom[1] - columnIndex) == 2)
                {
                    bool isKingSide=false;
                    if (!IsHostUser)
                    {
                        isKingSide = columnIndex > MoveFrom[1];
                        gameGrid?.Castling(isKingSide, IsHostUser,MyMove);
                    }
                    else
                    {
                        isKingSide = columnIndex < MoveFrom[1];
                        gameGrid?.Castling(!isKingSide, IsHostUser,MyMove);
                    }
                    
                
                }
                DisplayMoveArgs args = new(MoveFrom[0],MoveFrom[1],rowIndex, columnIndex);
                DisplayChanged?.Invoke(this, args);           
                if (MyMove)
                {
                    MoveTo[0] = rowIndex;
                    MoveTo[1] = columnIndex;
                    _status.UpdateStatus();
                    IsHostTurn = !IsHostTurn;
                    UpdateFbMove();
                    if (!IsGameOver)
                    {
                        bool opponentIsWhite = !PieceToMove.IsWhite;
                        if (IsCheckmate(opponentIsWhite, FlipBoard(gameGrid?.BoardPieces!)))
                        {
                            IsGameOver = true;
                            WinnerIsWhite = PieceToMove.IsWhite;
                            
                            Dictionary<string, object> dict = new()
                            {
                                { nameof(IsGameOver), true },
                                { nameof(WinnerIsWhite), WinnerIsWhite }
                            };
                            fbd.UpdateFields(Keys.GamesCollection, Id, dict, _ => { });
                            MainThread.InvokeOnMainThreadAsync(async () =>
                            {
                                bool iWon = WinnerIsWhite == (!IsHostUser);
                                await Shell.Current.CurrentPage.ShowPopupAsync(
                                    new GameResultPopup(iWon));
                            });
                        }
                    }
                }
                else
                {
                    OnGameChanged?.Invoke(this, EventArgs.Empty);
                }
            }
            protected override bool IsCheckmate(bool isWhite, Piece[,] board)
            {
                if (IsKingInCheck(isWhite, board))
                    if (!HasAnyLegalMove(isWhite, board))
                        return true;
                return false;
            }

        protected override void UpdateFbMove()
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(MoveFrom), MoveFrom },
                { nameof(MoveTo), MoveTo },
                { nameof(IsHostTurn), IsHostTurn },
                { nameof(WhiteTimeLeft), WhiteTimeLeft },
                { nameof(BlackTimeLeft), BlackTimeLeft }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
            {
                Game? updatedGame = snapshot?.ToObject<Game>();
                if (updatedGame != null)
                {
                    IsFull = updatedGame.IsFull;
                    GuestName = updatedGame.GuestName;
                    IsHostTurn = updatedGame.IsHostTurn;
                    MoveFrom = updatedGame.MoveFrom;
                    MoveTo = updatedGame.MoveTo;
                    WhiteTimeLeft = updatedGame.WhiteTimeLeft;
                    BlackTimeLeft = updatedGame.BlackTimeLeft;
                    UpdateStatus();
                    OnGameChanged?.Invoke(this, EventArgs.Empty);
                    if (_status.CurrentStatus == GameStatus.Statuses.Play && updatedGame.MoveFrom[0] != Keys.NoMove)
                    {
                        long myCurrentTime = IsHostUser ? BlackTimeLeft : WhiteTimeLeft; // (הנחה: מארח הוא לבן)
                        // יצירת הגדרות טיימר עם הזמן שנשאר לי בפועל!
                        TimerSettings currentTimer = new(myCurrentTime, Keys.TimerInterval);
                        WeakReferenceMessenger.Default.Send(new AppMessage<TimerSettings>(currentTimer));
                        MoveFrom[0] = 7 - MoveFrom[0];
                        MoveTo[0] = 7 - MoveTo[0];
                        MoveFrom[1] = 7 - MoveFrom[1];
                        MoveTo[1] = 7 - MoveTo[1];
                        Play(MoveTo[0], MoveTo[1], false);
                    }
                    else
                    {
                        WeakReferenceMessenger.Default.Send(new AppMessage<bool>(true));
                        TimeLeft = string.Empty;
                        TimeLeftChanged?.Invoke(this, EventArgs.Empty);
                    }
                    if (updatedGame.IsGameOver && !IsGameOver)
                    {
                        IsGameOver = true;
                        WinnerIsWhite = updatedGame.WinnerIsWhite;
                        MainThread.InvokeOnMainThreadAsync(async () =>
                        {
                            bool iWon = WinnerIsWhite == (!IsHostUser);
                            await Shell.Current.CurrentPage.ShowPopupAsync(
                                new GameResultPopup(iWon));
                        });
                    }
                }
                else
                {
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        OnGameDeleted?.Invoke(this, EventArgs.Empty);
                        Shell.Current.Navigation.PopAsync();                  
                    });

                }
            }

        
            protected override bool IsKingInCheck(bool isWhite, Piece[,] board)
            {
                int kingRow = -1, kingCol = -1;
                bool found = false;

                for (int i = 0; i < 8 && !found; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (board![i, j] is King k && k.IsWhite == isWhite)
                        {
                            kingRow = i;
                            kingCol = j;
                            found = true;
                            break;
                        }
                    }
                }
                if (!found)
                    return false;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Piece p = board![i, j];
                        if (p.StringImageSource != null && p.IsWhite != isWhite)
                        {
                            if (p.IsMoveValid(board!, i, j, kingRow, kingCol))
                                return true;
                        }
                    }
                }

                return false;
            }
            protected override bool HasAnyLegalMove(bool isWhite, Piece[,] board)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Piece piece = board![i, j];
                        if (piece.StringImageSource == null || piece.IsWhite != isWhite)
                            continue;

                        for (int rTo = 0; rTo < 8; rTo++)
                        {
                            for (int cTo = 0; cTo < 8; cTo++)
                            {
                                if (!piece.IsMoveValid(board!, i, j, rTo, cTo))
                                    continue;
                                Piece fromBackup = board[i, j];
                                Piece toBackup = board[rTo, cTo];
                                int oldRow = piece.RowIndex;
                                int oldCol = piece.ColumnIndex;
                                board[rTo, cTo] = gameGrid!.CreatePiece(piece, rTo, cTo);
                                board[i, j] = new Pawn(i, j, false, null);
                                bool kingStillInCheck = IsKingInCheck(isWhite, board);
                                board[i, j] = fromBackup;
                                board[rTo, cTo] = toBackup;
                                piece.RowIndex = oldRow;
                                piece.ColumnIndex = oldCol;
                                if (!kingStillInCheck)
                                    return true;
                            }
                        }
                    }
                }

                return false;
            }       
            protected override Piece[,] FlipBoard(Piece[,] original)
            {
                Piece[,] flipped = new Piece[8, 8];

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        Piece p = original[i, j];

                        int newRow = 7 - i;
                        int newCol = 7 - j;

                        if (p.StringImageSource == null)
                        {
                            flipped[newRow, newCol] = new Pawn(newRow, newCol, false, null);
                        }
                        else
                        {
                            flipped[newRow, newCol] = gameGrid!.CreatePiece(p, newRow, newCol);
                        }
                    }
                }
                return flipped;
            }
        
        }
    }
