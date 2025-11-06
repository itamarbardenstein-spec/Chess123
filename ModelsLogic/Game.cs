using Chess.Models;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.ModelsLogic
{
    internal class Game:GameModel
    {
        internal Game()
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
