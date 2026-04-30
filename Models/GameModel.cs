using Chess.ModelsLogic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace Chess.Models
{
    /// Base abstraction for the chess game, containing state, sync data, and board logic
    public abstract class GameModel
    {
        #region Fields
        /// Logical representation of the 8x8 chessboard
        protected Piece[,]? gameBoard;
        /// List of valid moves for the currently selected piece
        protected List<int[]> legalMoves = [];
        /// Counter for piece selection clicks (first click selects, second moves)
        protected int ClickCount = 0;
        /// Internal enum to track database operation types
        protected enum Actions { Changed, Deleted }
        /// Current database action being performed
        protected Actions action = Actions.Changed;
        /// Settings and intervals for the game countdown clocks
        protected TimerSettings timerSettings = new(Keys.TimerTotalTime, Keys.TimerInterval);
        /// Service for Firebase data operations
        protected FbData fbd = new();
        /// Registration for real-time Firestore updates
        protected IListenerRegistration? ilr;
        /// Current status and turn information for the UI
        [Ignored]
        public GameStatus _status = new();
        #endregion

        #region Events
        /// Triggered when the remote game data changes
        [Ignored]
        public EventHandler? OnGameChanged;
        /// Triggered when a player's remaining time is updated
        [Ignored]
        public EventHandler? TimeLeftChanged;
        /// Triggered when a move puts a king in check
        [Ignored]
        public EventHandler? KingIsInCheck;
        /// Triggered to clear all square highlights from the UI
        [Ignored]
        public EventHandler? ClearBoardHighLights;
        /// Triggered to remove legal move dots from the UI
        [Ignored]
        public EventHandler? ClearLegalMovesDots;
        /// Triggered when the game document is removed from the database
        [Ignored]
        public EventHandler? OnGameDeleted;
        /// Sends the list of legal move coordinates to the UI
        [Ignored]
        public EventHandler<List<int[]>>? LegalMoves;
        /// Signals the UI to move a piece visually
        [Ignored]
        public EventHandler<DisplayMoveArgs>? DisplayChanged;
        /// Highlights a specific square on the board
        [Ignored]
        public EventHandler<HighlightSquareArgs>? HighlightSquare;
        /// Removes highlight from a specific square
        [Ignored]
        public EventHandler<HighlightSquareArgs>? ClearSquareHighLight;
        /// Triggered when a pawn reaches the back rank
        [Ignored]
        public EventHandler<OnPromotionArgs>? OnPromotion;
        /// Signals that a castling move has occurred
        [Ignored]
        public EventHandler<CastlingArgs>? OnCastling;
        /// Triggered when the game concludes (win/loss/draw)
        [Ignored]
        public EventHandler<GameOverArgs>? GameOver;
        #endregion

        #region Properties
        protected abstract GameStatus Status { get; }
        /// Human-readable message based on current game status
        [Ignored]
        public string StatusMessage => Status.StatusMessage;
        /// Formatted string representing the total game time
        [Ignored]
        public string TimeName => $"{Time} min";
        /// Unique document ID for the game in Firestore
        [Ignored]
        public string Id { get; set; } = string.Empty;
        /// Username of the local player
        [Ignored]
        public string MyName { get; set; } = new User().UserName;
        [Ignored]
        public abstract string OpponentName { get; }
        /// List of images of white pieces captured during the game
        [Ignored]
        public List<string>? WhiteCapturedImages { get; set; } = [];
        /// List of images of black pieces captured during the game
        [Ignored]
        public List<string>? BlackCapturedImages { get; set; } = [];
        /// True if the local player created the game
        [Ignored]
        public bool IsHostUser { get; set; }
        /// True if both players have joined the session
        public bool IsFull { get; set; }
        /// True if the match has finished
        public bool IsGameOver { get; set; }
        /// Tracks whether it is the host's turn to move
        public bool IsHostTurn { get; set; } = false;
        /// Timestamp when the game was created
        public DateTime Created { get; set; }
        /// Username of the joining player
        public string GuestName { get; set; } = string.Empty;
        /// Username of the creating player
        public string HostName { get; set; } = string.Empty;
        /// Text describing why the game ended
        public string GameOverReason { get; set; } = string.Empty;
        /// Initial time allotted for each player in minutes
        public int Time { get; set; }
        /// Remaining time for the white player in milliseconds
        public long WhiteTimeLeft { get; set; }
        /// Remaining time for the black player in milliseconds
        public long BlackTimeLeft { get; set; }
        /// Starting coordinates of the last move [row, col]
        public List<int> MoveFrom { get; set; } = [Keys.NoMove, Keys.NoMove];
        /// Ending coordinates of the last move [row, col]
        public List<int> MoveTo { get; set; } = [Keys.NoMove, Keys.NoMove];
        #endregion

        #region Public Methods      
        public abstract void InitGameBoard();
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void AddSnapshotListener();
        public abstract void RemoveSnapshotListener();
        public abstract void CheckMove(Piece p);
        public abstract void UpdateGuestUser(Action<Task> OnComplete);
        public abstract string GameOverMessageTitle(bool IWon, string reason);
        public abstract string GameOverMessageReason(bool IWon, string reason);
        public abstract void ResignGame();
        public abstract List<CapturedPieceGroup>? GetGroupedCapturedPieces(bool myList);
        #endregion

        #region Private Methods
        protected abstract List<string>? GetMyCapturedPiecesList(bool MyList);
        protected abstract void Play(int rowIndex, int columnIndex, bool MyMove);
        protected abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
        protected abstract void UpdateFbMove();
        protected abstract void UpdateFbGameOver();
        protected abstract Piece CreatePiece(Piece original, int row, int col);
        protected abstract void OnComplete(Task task);
        protected abstract bool IsCheckmate(bool isWhite, Piece[,] board);
        protected abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);
        protected abstract bool IsKingInCheck(bool isWhite, Piece[,] board);
        protected abstract bool HasAnyLegalMove(bool isWhite, Piece[,] board);
        protected abstract Piece[,] FlipBoard(Piece[,] original);
        protected abstract void RegisterTimer();
        protected abstract void OnMessageReceived(long timeLeft);
        protected abstract void UpdateFbJoinGame(Action<Task> OnComplete);
        protected abstract void Castling(bool right, bool isHostUser, bool MyMove);
        protected abstract void CheckCastling(int columnIndex, bool MyMove);
        protected abstract void CheckGameOver(Piece movedPiece);
        protected abstract void GetLegalMoveList(Piece p);
        protected abstract void UpdateStatus();
        #endregion
    }
}