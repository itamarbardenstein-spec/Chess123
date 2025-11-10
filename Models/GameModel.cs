
using Chess.ModelsLogic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace Chess.Models
{
    public abstract class GameModel
    {
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        [Ignored]
        public EventHandler? OnGameChanged;
        [Ignored]
        public EventHandler? OnGameDeleted;
        public string HostName { get; set; } = string.Empty;   
        public string GuestName { get; set; } = string.Empty;         
        public DateTime Created { get; set; }       
        public bool IsFull { get; set; }
        [Ignored]
        public abstract string OpponentName { get;}
        [Ignored]
        public string MyName { get; set; } = new User().UserName;
        [Ignored]
        public string Id { get; set; } = string.Empty;
        [Ignored]
        public bool IsHostUser { get; set; }
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void AddSnapshotListener();
        public abstract void RemoveSnapshotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);

    }
}
