using Chess.Models;

namespace Chess.ModelsLogic
{
    public class Game:GameModel
    {
        public override string OpponentName => IsHost ? GuestName : HostName;
        public Game()
        {
            HostName = new User().UserName;            
            Created = DateTime.Now;
        }
        
        public override void SetDocument(Action<System.Threading.Tasks.Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
    }
}
