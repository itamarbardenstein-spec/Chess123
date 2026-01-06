
using Chess.ModelsLogic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace Chess.Models
{
    public abstract class GameModel
    {
        public GameGrid? gameGrid = [];
        protected int ClickCount = 0;
        protected abstract GameStatus Status { get; }       
        protected enum Actions { Changed, Deleted }      
        protected Actions action = Actions.Changed;
        protected TimerSettings timerSettings = new(Keys.TimerTotalTime, Keys.TimerInterval);
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        [Ignored]
        public GameStatus _status = new();       
        [Ignored]
        public EventHandler? OnGameChanged;
        [Ignored]
        public EventHandler? TimeLeftChanged;
        [Ignored]
        public EventHandler? InvalidMove;
        [Ignored]
        public EventHandler? OnGameDeleted;
        [Ignored]
        public EventHandler<DisplayMoveArgs>? DisplayChanged;
        [Ignored]
        public EventHandler<GameOverArgs>? GameOver;
        [Ignored]
        public string StatusMessage => Status.StatusMessage;    
        [Ignored]
        public string TimeName => $"{Time} min";
        [Ignored]
        public abstract string OpponentName { get; }
        [Ignored]
        public string MyName { get; set; } = new User().UserName;
        [Ignored]
        public string Id { get; set; } = string.Empty;
        [Ignored]
        public bool IsHostUser { get; set; }
        [Ignored]
        public string TimeLeft { get; protected set; } = string.Empty;
        public string GuestName { get; set; } = string.Empty;         
        public int Time {  get; set; }
        public bool IsGameOver { get; set; }
        public DateTime Created { get; set; }
        public string HostName { get; set; } = string.Empty;
        public bool? WinnerIsWhite { get; set; }
        public bool IsFull { get; set; }
        public long WhiteTimeLeft { get; set; } 
        public bool TimeRanOut { get; set; }
        public long BlackTimeLeft { get; set; }
        public bool IsHostTurn { get; set; } = false;
        public List<int> MoveFrom { get; set; } = [Keys.NoMove, Keys.NoMove];
        public List<int> MoveTo { get; set; } = [Keys.NoMove, Keys.NoMove];        
        protected abstract void UpdateStatus();       
        public abstract void Play(int rowIndex, int columnIndex, bool MyMove);
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void AddSnapshotListener();
        public abstract void RemoveSnapshotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);        
        protected abstract void UpdateFbMove();
        protected abstract void UpdateFbGameOver();
        protected abstract void OnComplete(Task task);
        protected abstract bool IsCheckmate(bool isWhite, Piece[,] board);
        protected abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);        
        protected abstract bool IsKingInCheck(bool isWhite, Piece[,] board);
        protected abstract bool HasAnyLegalMove(bool isWhite, Piece[,] board);
        public abstract void CheckMove(Piece p);
        protected abstract Piece[,] FlipBoard(Piece[,] original);
        protected abstract void RegisterTimer();
        protected abstract void OnMessageReceived(long timeLeft);
        public abstract void UpdateGuestUser(Action<Task> OnComplete);
        protected abstract void UpdateFbJoinGame(Action<Task> OnComplete);
        public abstract string GameOverMessageTitle(bool IWon);
        public abstract string GameOverMessageReason(bool IWon,bool IsCheckmate);
        public abstract void Promotion(int row, int column);
    }
}
