using Chess.Models;
using CommunityToolkit.Mvvm.Messaging;
using Plugin.CloudFirestore;

namespace Chess.ModelsLogic
{
    public class Game : GameModel
    {
        #region Properties
        public override string OpponentName => IsHostUser ? GuestName : HostName;
        protected override GameStatus Status => _status;
        #endregion
        #region Constructors
        public Game() { RegisterTimer(); }
        public Game(GameTime selectedGameTime)
        {
            RegisterTimer();
            Created = DateTime.Now;
            HostName = new User().UserName;
            IsHostUser = true;
            Time = selectedGameTime.Time;
            UpdateStatus();
            long totalMillis = Time * Keys.FromHourToMilliSeconds;
            WhiteTimeLeft = totalMillis;
            BlackTimeLeft = totalMillis;
            InitGameBoard();
        }
        #endregion
        #region Public Methods
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
        public override void UpdateGuestUser(Action<Task> OnComplete)
        {
            GuestName = MyName;
            IsFull = true;
            UpdateStatus();
            UpdateFbJoinGame(OnComplete);
        }
        public override void InitGameBoard()
        {
            gameBoard = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
            {
                if (IsHostUser)
                {
                    gameBoard![1, i] = new Pawn(1, i, true, Strings.WhitePawn);
                    gameBoard[6, i] = new Pawn(6, i, false, Strings.BlackPawn);
                }
                else
                {
                    gameBoard![1, i] = new Pawn(1, i, false, Strings.BlackPawn);
                    gameBoard[6, i] = new Pawn(6, i, true, Strings.WhitePawn);
                }
                if (i == 0 || i == 7)
                {
                    if (IsHostUser)
                    {
                        gameBoard[0, i] = new Rook(0, i, true, Strings.WhiteRook);
                        gameBoard[7, i] = new Rook(7, i, false, Strings.BlackRook);
                    }
                    else
                    {
                        gameBoard[0, i] = new Rook(0, i, false, Strings.BlackRook);
                        gameBoard[7, i] = new Rook(7, i, true, Strings.WhiteRook);
                    }
                }
                else if (i == 1 || i == 6)
                {
                    if (IsHostUser)
                    {
                        gameBoard[0, i] = new Knight(0, i, true, Strings.WhiteKnight);
                        gameBoard[7, i] = new Knight(7, i, false, Strings.BlackKnight);
                    }
                    else
                    {
                        gameBoard[0, i] = new Knight(0, i, false, Strings.BlackKnight);
                        gameBoard[7, i] = new Knight(7, i, true, Strings.WhiteKnight);
                    }
                }
                else if (i == 2 || i == 5)
                {
                    if (IsHostUser)
                    {
                        gameBoard[0, i] = new Bishop(0, i, true, Strings.WhiteBishop);
                        gameBoard[7, i] = new Bishop(7, i, false, Strings.BlackBishop);
                    }
                    else
                    {
                        gameBoard[0, i] = new Bishop(0, i, false, Strings.BlackBishop);
                        gameBoard[7, i] = new Bishop(7, i, true, Strings.WhiteBishop);
                    }
                }
                else if (i == 3)
                {
                    if (IsHostUser)
                    {
                        gameBoard[0, i + 1] = new Queen(0, i + 1, true, Strings.WhiteQueen);
                        gameBoard[7, i + 1] = new Queen(7, i + 1, false, Strings.BlackQueen);
                    }
                    else
                    {
                        gameBoard[0, i] = new Queen(0, i, false, Strings.BlackQueen);
                        gameBoard[7, i] = new Queen(7, i, true, Strings.WhiteQueen);
                    }
                }
                else
                {
                    if (IsHostUser)
                    {
                        gameBoard[0, i - 1] = new King(0, i - 1, true, Strings.WhiteKing);
                        gameBoard[7, i - 1] = new King(7, i - 1, false, Strings.BlackKing);
                    }
                    else
                    {
                        gameBoard[0, i] = new King(0, i, false, Strings.BlackKing);
                        gameBoard[7, i] = new King(7, i, true, Strings.WhiteKing);
                    }
                }
            }
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (gameBoard[i, j] == null)
                        gameBoard[i, j] = new Pawn(i, j, false, null);
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
        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }
        public override void CheckMove(Piece p)
        {
            List<int[]> legalMoves = GetLegalMoveList(p);
            if (!IsGameOver)
                if (_status.CurrentStatus == GameStatus.Statuses.Play)
                {
                    if (ClickCount == 0)
                    {
                        if (p?.StringImageSource != null && (IsHostUser ? (!p.IsWhite) : p.IsWhite))
                        {
                            ClickCount++;
                            MoveFrom[0] = p.RowIndex;
                            MoveFrom[1] = p.ColumnIndex;
                            LegalMoves?.Invoke(this, legalMoves);
                            HighlightSquare?.Invoke(this, new HighlightSquareArgs(p.RowIndex, p.ColumnIndex));
                        }
                    }
                    else
                    {
                        if (p.RowIndex == MoveFrom[0] && p.ColumnIndex == MoveFrom[1])
                        {
                            ClearLegalMovesDots?.Invoke(this, EventArgs.Empty);
                            ClearSquareHighLight?.Invoke(this, new HighlightSquareArgs(p.RowIndex, p.ColumnIndex));
                            ClickCount = 0;
                        }
                        else if (p?.StringImageSource != null && p.IsWhite == gameBoard?[MoveFrom[0], MoveFrom[1]].IsWhite)
                        {
                            ClearSquareHighLight?.Invoke(this, new HighlightSquareArgs(MoveFrom[0], MoveFrom[1]));
                            MoveFrom[0] = p.RowIndex;
                            MoveFrom[1] = p.ColumnIndex;
                            LegalMoves?.Invoke(this, legalMoves);
                            HighlightSquare?.Invoke(this, new HighlightSquareArgs(p.RowIndex, p.ColumnIndex));
                        }
                        else
                        {
                            if (gameBoard![MoveFrom[0], MoveFrom[1]].IsMoveValid(gameBoard, MoveFrom[0], MoveFrom[1], p!.RowIndex, p.ColumnIndex))
                                Play(p.RowIndex, p.ColumnIndex, true);
                            else
                                ClearSquareHighLight?.Invoke(this, new HighlightSquareArgs(MoveFrom[0], MoveFrom[1]));
                            ClearLegalMovesDots?.Invoke(this, EventArgs.Empty);
                            ClickCount = 0;
                        }
                    }
                }
        }
        public override void Play(int rowIndex, int columnIndex, bool MyMove)
        {
            Piece eatenPiece = gameBoard![rowIndex, columnIndex];
            if (eatenPiece.StringImageSource != null)
            {
                if (eatenPiece.IsWhite)
                    WhiteCapturedImages?.Add(eatenPiece.StringImageSource);
                else
                    BlackCapturedImages?.Add(eatenPiece.StringImageSource);
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
            gameBoard![rowIndex, columnIndex] = CreatePiece(gameBoard![MoveFrom[0], MoveFrom[1]], rowIndex, columnIndex);
            gameBoard![MoveFrom[0], MoveFrom[1]] = new Pawn(MoveFrom[0], MoveFrom[1], false, null);
            if (MyMove && IsKingInCheck(gameBoard![rowIndex, columnIndex].IsWhite, FlipBoard(gameBoard)))
            {
                gameBoard![MoveFrom[0], MoveFrom[1]] = CreatePiece(gameBoard![rowIndex, columnIndex]!, MoveFrom[0], MoveFrom[1]);
                gameBoard![rowIndex, columnIndex] = eatenPiece;
            }
            else
            {
                gameBoard![MoveFrom[0], MoveFrom[1]] = CreatePiece(gameBoard![rowIndex, columnIndex]!, MoveFrom[0], MoveFrom[1]);
                gameBoard![rowIndex, columnIndex] = eatenPiece;
                CheckCastling(columnIndex, MyMove);
                gameBoard![rowIndex, columnIndex] = CreatePiece(gameBoard![MoveFrom[0], MoveFrom[1]]!, rowIndex, columnIndex);
                gameBoard![MoveFrom[0], MoveFrom[1]] = new Pawn(MoveFrom[0], MoveFrom[1], false, null);
                DisplayMoveArgs args = new(MoveFrom[0], MoveFrom[1], rowIndex, columnIndex);
                DisplayChanged?.Invoke(this, args);
                if (MyMove && gameBoard![rowIndex, columnIndex] is Pawn && rowIndex == 0)
                {
                    gameBoard[rowIndex, columnIndex] = IsHostUser ? new Queen(rowIndex, columnIndex, false, Strings.BlackQueen) :
                                                             new Queen(rowIndex, columnIndex, true, Strings.WhiteQueen);
                    OnPromotionArgs promoArgs = new(rowIndex, columnIndex, IsHostUser);
                    OnPromotion?.Invoke(this, promoArgs);
                }
                if (MyMove)
                {
                    MoveTo[0] = rowIndex;
                    MoveTo[1] = columnIndex;
                    _status.UpdateStatus();
                    IsHostTurn = !IsHostTurn;
                    UpdateFbMove();
                    CheckGameOver(gameBoard[rowIndex, columnIndex]);
                }
                else
                    OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public override string GameOverMessageTitle(bool IWon, string reason)
        {
            string result;
            if (reason == Strings.Draw)
                result = Strings.Draw;
            else
                result = IWon ? Strings.YouWon : Strings.YouLost;
            return result;
        }
        public override string GameOverMessageReason(bool IWon, string reason)
        {
            string result;
            if (reason == Strings.Checkmate)
                result = IWon ? Strings.WinCheckmate : Strings.LoseCheckmate;
            else if (reason == Strings.Time)
                result = IWon ? Strings.WinTime : Strings.LoseTime;
            else if (reason == Strings.Draw)
                result = Strings.YouDrew;
            else
                result = IWon ? Strings.OpponentResigned : Strings.YouResigned;
            return result;
        }
        public override Piece CreatePiece(Piece original, int row, int col)
        {
            bool isWhite = original.IsWhite;
            string? img = original.StringImageSource;
            return original switch
            {
                Pawn => new Pawn(row, col, isWhite, img),
                Rook r => new Rook(row, col, isWhite, img)
                {
                    HasLeftRookMoved = r.HasLeftRookMoved,
                    HasRightRookMoved = r.HasRightRookMoved
                },
                Knight => new Knight(row, col, isWhite, img),
                Bishop => new Bishop(row, col, isWhite, img),
                Queen => new Queen(row, col, isWhite, img),
                King k => new King(row, col, isWhite, img)
                {
                    HasKingMoved = k.HasKingMoved
                },
                _ => throw new Exception()
            };
        }
        public override void ResignGame()
        {
            if (!IsGameOver && IsFull)
            {
                if (IsHostTurn && IsHostUser || !IsHostTurn && !IsHostUser)
                {
                    _status.UpdateStatus();
                    IsHostTurn = !IsHostTurn;
                    UpdateFbMove();
                }
                IsGameOver = true;
                GameOverReason = Strings.Resignation;
                UpdateFbGameOver();
                GameOverArgs GameOverArgs = new(false, Strings.Resignation);
                GameOver?.Invoke(this, GameOverArgs);
            }
        }
        public override List<string>? GetMyCapturedPiecesList(bool MyList)
        {
            List<string>? result;
            if (IsHostUser)
            {
                if (!MyList)
                    result = BlackCapturedImages;
                else
                    result = WhiteCapturedImages;
            }
            else
            {
                if (!MyList)
                    result = WhiteCapturedImages;
                else
                    result = BlackCapturedImages;
            }
            return result;
        }
        public override List<CapturedPieceGroup>? GetGroupedCapturedPieces(bool myList)
        {
            List<CapturedPieceGroup>? result = null;
            List<string>? rawList = GetMyCapturedPiecesList(myList);
            if (rawList != null)
                result = [.. rawList.GroupBy(img => img)
                    .Select(cpg => new CapturedPieceGroup
                {
                    ImageSource = cpg.Key,
                    Count = cpg.Count()
                })
                .OrderBy(x => x.ImageSource)];
            return result;
        }
        #endregion
        #region Private Methods
        protected override void RegisterTimer()
        {
            WeakReferenceMessenger.Default.Register<AppMessage<long>>(this, (r, m) =>
            {
                OnMessageReceived(m.Value);
            });
        }
        protected override void OnMessageReceived(long timeLeft)
        {
            if (timeLeft == Keys.FinishedSignal && !IsGameOver)
            {
                ilr?.Remove();
                IsGameOver = true;
                GameOverReason = Strings.Time;
                if (!string.IsNullOrEmpty(Id))
                    UpdateFbGameOver();
                GameOver?.Invoke(this, new GameOverArgs(false, Strings.Time));
            }
            else
            {
                if (IsHostUser)
                    BlackTimeLeft = timeLeft;
                else
                    WhiteTimeLeft = timeLeft;
                TimeLeftChanged?.Invoke(this, EventArgs.Empty);
            }
        }       
        protected override void UpdateStatus()
        {
            _status.CurrentStatus = IsHostUser && IsHostTurn || !IsHostUser && !IsHostTurn ?
            GameStatus.Statuses.Play : GameStatus.Statuses.Wait;
        }      
        protected override void UpdateFbJoinGame(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(IsFull), IsFull },
                { nameof(GuestName), GuestName }
            };
            action = Actions.Changed;
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }       
        protected override void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                if (action == Actions.Deleted)
                    OnGameDeleted?.Invoke(this, EventArgs.Empty);
                else
                    OnGameChanged?.Invoke(this, EventArgs.Empty);
        }       
        protected override List<int[]> GetLegalMoveList(Piece p)
        {
            List<int[]> legalMoves = [];
            bool isWhite = p.IsWhite;
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 8; c++)
                    if (gameBoard![p.RowIndex, p.ColumnIndex].IsMoveValid(gameBoard!, p.RowIndex, p.ColumnIndex, r, c))
                    {
                        Piece fromPiece = gameBoard[p.RowIndex, p.ColumnIndex];
                        Piece toPiece = gameBoard[r, c];
                        int originalRow = p.RowIndex;
                        int originalCol = p.ColumnIndex;
                        gameBoard[r, c] = CreatePiece(fromPiece, r, c);
                        gameBoard[p.RowIndex, p.ColumnIndex] = new Pawn(p.RowIndex, p.ColumnIndex, false, null);
                        bool kingInCheck = IsKingInCheck(isWhite, FlipBoard(gameBoard));
                        gameBoard[p.RowIndex, p.ColumnIndex] = fromPiece;
                        gameBoard[r, c] = toPiece;
                        fromPiece.RowIndex = originalRow;
                        fromPiece.ColumnIndex = originalCol;
                        if (!kingInCheck)
                            legalMoves.Add([r, c]);
                    }
            return legalMoves;
        }        
        protected override void CheckGameOver(Piece movedPiece)
        {
            if (!IsGameOver)
            {
                bool opponentIsWhite = !movedPiece.IsWhite;
                if (IsCheckmate(opponentIsWhite, FlipBoard(gameBoard!)))
                {
                    IsGameOver = true;
                    GameOverReason = Strings.Checkmate;
                    UpdateFbGameOver();
                    GameOverArgs GameOverArgs = new(true, Strings.Checkmate);
                    GameOver?.Invoke(this, GameOverArgs);
                }
                else if (!HasAnyLegalMove(opponentIsWhite, FlipBoard(gameBoard!)) && !IsKingInCheck(opponentIsWhite, FlipBoard(gameBoard!)))
                {
                    IsGameOver = true;
                    GameOverReason = Strings.Draw;
                    UpdateFbGameOver();
                    GameOverArgs GameOverArgs = new(false, Strings.Draw);
                    GameOver?.Invoke(this, GameOverArgs);
                }
                else
                {
                    bool KnightFound = false;
                    bool BishopFound = false;
                    bool WinnerPieceFound = false;
                    for (int i = 0; i < 8; i++)
                        for (int j = 0; j < 8; j++)
                        {
                            Piece p = gameBoard![i, j];
                            if (p.StringImageSource != null)
                            {
                                if (p is Knight)
                                    KnightFound = true;
                                else if (p is Bishop)
                                    BishopFound = true;
                                else if (p is not King)
                                    WinnerPieceFound = true;
                            }
                        }
                    if (!WinnerPieceFound && !(KnightFound && BishopFound))
                    {
                        IsGameOver = true;
                        GameOverReason = Strings.Draw;
                        UpdateFbGameOver();
                        GameOverArgs GameOverArgs = new(false, Strings.Draw);
                        GameOver?.Invoke(this, GameOverArgs);
                    }
                }
            }
        }
        protected override bool IsCheckmate(bool isWhite, Piece[,] board)
        {
            bool result = false;
            if (IsKingInCheck(isWhite, board) && !HasAnyLegalMove(isWhite, board))
                result = true;
            return result;
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
                if (updatedGame.IsGameOver && !IsGameOver)
                {
                    IsGameOver = true;
                    GameOverReason = updatedGame.GameOverReason;
                    GameOverArgs GameOverArgs;
                    if (GameOverReason == Strings.Time)
                        GameOverArgs = new(true, Strings.Time);
                    else if (GameOverReason == Strings.Checkmate)
                        GameOverArgs = new(false, Strings.Checkmate);
                    else if (GameOverReason == Strings.Draw)
                        GameOverArgs = new(false, Strings.Draw);
                    else
                        GameOverArgs = new(true, Strings.Resignation);
                    GameOver?.Invoke(this, GameOverArgs);
                    WeakReferenceMessenger.Default.Send(new AppMessage<bool>(true));
                }
                else
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
                        long myCurrentTime = IsHostUser ? BlackTimeLeft : WhiteTimeLeft;
                        TimerSettings currentTimer = new(myCurrentTime, Keys.TimerInterval);
                        WeakReferenceMessenger.Default.Send(new AppMessage<TimerSettings>(currentTimer));
                        MoveFrom[0] = 7 - MoveFrom[0];
                        MoveTo[0] = 7 - MoveTo[0];
                        MoveFrom[1] = 7 - MoveFrom[1];
                        MoveTo[1] = 7 - MoveTo[1];
                        Play(MoveTo[0], MoveTo[1], false);
                        if (MoveFrom[0] != Keys.NoMove && gameBoard?[MoveTo[0], MoveTo[1]] is Pawn && MoveTo[0] == 7)
                        {
                            if (IsHostUser)
                                gameBoard![MoveTo[0], MoveTo[1]] = new Queen(MoveTo[0], MoveTo[1], true, Strings.WhiteQueen);
                            else
                                gameBoard![MoveTo[0], MoveTo[1]] = new Queen(MoveTo[0], MoveTo[1], false, Strings.BlackQueen);
                            OnPromotionArgs promoArgs = new(MoveTo[0], MoveTo[1], !IsHostUser);
                            OnPromotion?.Invoke(this, promoArgs);
                        }
                    }
                    else
                        WeakReferenceMessenger.Default.Send(new AppMessage<bool>(true));
                }
            }
            else
            {
                WeakReferenceMessenger.Default.Send(new AppMessage<bool>(true));
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    OnGameDeleted?.Invoke(this, EventArgs.Empty);
                    Shell.Current.Navigation.PopAsync();
                });
            }
        }
        protected override void UpdateFbGameOver()
        {
            Dictionary<string, object> dict = new()
            {
               { nameof(IsGameOver), true },
               { nameof(GameOverReason), GameOverReason },
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        protected override bool IsKingInCheck(bool isWhite, Piece[,] board)
        {
            bool result = false;
            int kingRow = -1, kingCol = -1;
            bool found = false;
            for (int i = 0; i < 8 && !found; i++)
                for (int j = 0; j < 8; j++)
                    if (board![i, j] is King k && k.IsWhite == isWhite)
                    {
                        kingRow = i;
                        kingCol = j;
                        found = true;
                    }
            if (found)
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        Piece p = board![i, j];
                        if (p.StringImageSource != null && p.IsWhite != isWhite)
                            if (p.IsMoveValid(board!, i, j, kingRow, kingCol))
                                result = true;
                    }
            }
            return result;
        }
        protected override bool HasAnyLegalMove(bool isWhite, Piece[,] board)
        {
            bool result = false;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    Piece piece = board![i, j];
                    if (piece.StringImageSource != null && piece.IsWhite == isWhite)
                        for (int rTo = 0; rTo < 8; rTo++)
                            for (int cTo = 0; cTo < 8; cTo++)
                                if (piece.IsMoveValid(board!, i, j, rTo, cTo))
                                {
                                    Piece fromBackup = board[i, j];
                                    Piece toBackup = board[rTo, cTo];
                                    int oldRow = piece.RowIndex;
                                    int oldCol = piece.ColumnIndex;
                                    board[rTo, cTo] = CreatePiece(piece, rTo, cTo);
                                    board[i, j] = new Pawn(i, j, false, null);
                                    bool kingStillInCheck = IsKingInCheck(isWhite, board);
                                    board[i, j] = fromBackup;
                                    board[rTo, cTo] = toBackup;
                                    piece.RowIndex = oldRow;
                                    piece.ColumnIndex = oldCol;
                                    if (!kingStillInCheck)
                                        result = true;
                                }
                }
            return result;
        }
        protected override void CheckCastling(int columnIndex, bool MyMove)
        {
            if (gameBoard?[MoveFrom[0], MoveFrom[1]] is King king && MyMove)
                king.HasKingMoved = true;
            if (gameBoard?[MoveFrom[0], MoveFrom[1]] is Rook rook)
            {
                if (MoveFrom[1] == 0)
                    rook.HasLeftRookMoved = true;
                else
                    rook.HasRightRookMoved = true;
            }
            Piece PieceToMove = gameBoard?[MoveFrom[0], MoveFrom[1]]!;
            if (PieceToMove is King && Math.Abs(MoveFrom[1] - columnIndex) == 2)
            {
                bool isKingSide;
                if (!IsHostUser)
                {
                    isKingSide = columnIndex > MoveFrom[1];
                    Castling(isKingSide, IsHostUser, MyMove);
                    CastlingArgs CastleArgs = new(isKingSide, IsHostUser, MyMove);
                    OnCastling?.Invoke(this, CastleArgs);
                }
                else
                {
                    isKingSide = columnIndex < MoveFrom[1];
                    Castling(!isKingSide, IsHostUser, MyMove);
                    CastlingArgs CastleArgs = new(!isKingSide, IsHostUser, MyMove);
                    OnCastling?.Invoke(this, CastleArgs);
                }
            }
        }
        protected override void Castling(bool right, bool isHostUser, bool MyMove)
        {
            if (right)
            {
                if (MyMove)
                {
                    if (isHostUser)
                    {
                        gameBoard![7, 4] = CreatePiece(gameBoard[7, 7], 7, 4);
                        gameBoard[7, 7] = new Pawn(7, 7, false, null);
                    }
                    else
                    {
                        gameBoard![7, 5] = CreatePiece(gameBoard[7, 7], 7, 5);
                        gameBoard[7, 7] = new Pawn(7, 7, false, null);
                    }
                }
                else
                {
                    if (!isHostUser)
                    {
                        gameBoard![0, 5] = CreatePiece(gameBoard[0, 7], 0, 5);
                        gameBoard[0, 7] = new Pawn(0, 7, false, null);
                    }
                    else
                    {
                        gameBoard![0, 4] = CreatePiece(gameBoard[0, 7], 0, 4);
                        gameBoard[0, 7] = new Pawn(0, 7, false, null);
                    }
                }
            }
            else
            {
                if (MyMove)
                {
                    if (isHostUser)
                    {
                        gameBoard![7, 2] = CreatePiece(gameBoard[7, 0], 7, 2);
                        gameBoard[7, 0] = new Pawn(7, 0, false, null);
                    }
                    else
                    {
                        gameBoard![7, 3] = CreatePiece(gameBoard[7, 0], 7, 3);
                        gameBoard[7, 0] = new Pawn(7, 0, false, null);
                    }
                }
                else
                {
                    if (!isHostUser)
                    {
                        gameBoard![0, 3] = CreatePiece(gameBoard[0, 0], 0, 3);
                        gameBoard[0, 0] = new Pawn(0, 0, false, null);
                    }
                    else
                    {
                        gameBoard![0, 2] = CreatePiece(gameBoard[0, 0], 0, 2);
                        gameBoard[0, 0] = new Pawn(0, 0, false, null);
                    }
                }
            }
        }
        protected override Piece[,] FlipBoard(Piece[,] original)
        {
            Piece[,] flipped = new Piece[8, 8];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                {
                    Piece p = original[i, j];
                    int newRow = 7 - i;
                    int newCol = 7 - j;
                    if (p.StringImageSource == null)
                        flipped[newRow, newCol] = new Pawn(newRow, newCol, false, null);
                    else
                        flipped[newRow, newCol] = CreatePiece(p, newRow, newCol);
                }
            return flipped;
        }
        #endregion
    }
}
