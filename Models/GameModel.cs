
using Chess.ModelsLogic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace Chess.Models
{
    public abstract class GameModel
    {      
        protected abstract GameStatus Status { get; }
        protected int ClickCount = 0;
        protected enum Actions { Changed, Deleted }
        protected Dictionary<(int row, int col), PieceModel> BoardUIMap = [];
        protected Actions action = Actions.Changed;
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        protected GameStatus _status = new();
        protected Grid? GameBoard;
        [Ignored]
        public Piece[,]? BoardPieces;
        [Ignored]
        public EventHandler? OnGameChanged;
        [Ignored]
        public EventHandler? OnGameDeleted;            
        [Ignored]
        public string StatusMessage => Status.StatusMessage;
        [Ignored]     
        public string HostName { get; set; } = string.Empty;
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
        public string GuestName { get; set; } = string.Empty;         
        public int Time {  get; set; }
        public DateTime Created { get; set; }
        public bool IsFull { get; set; }
        public bool IsHostTurn { get; set; } = false;
        public List<int> MoveFrom { get; set; } = [Keys.NoMove, Keys.NoMove];
        public List<int> MoveTo { get; set; } = [Keys.NoMove, Keys.NoMove];        
        protected abstract void UpdateStatus();
        protected abstract void OnButtonClicked(object? sender, EventArgs e);
        protected abstract void Play(int rowIndex, int columnIndex, bool MyMove);
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void AddSnapshotListener();
        public abstract void RemoveSnapshotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void InitGrid(Grid board);
        protected abstract void UpdateFbMove();

    }
}
