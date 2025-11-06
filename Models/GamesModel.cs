using Chess.ModelsLogic;
using Microsoft.Maui.Controls;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    internal class GamesModel
    {
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        public bool IsBusy { get; set; }
        public ObservableCollection<Game>? GamesList { get; set; } = [];
        public EventHandler<bool>? OnGameAdded;
        public EventHandler? OnGamesChanged;
    }
}
