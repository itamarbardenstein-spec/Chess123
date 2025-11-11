using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public class GameTime
    {
        public int Time { get; set; }
        public string DisplayName => $"{Time} min";
        public GameTime(int time)
        {
            Time = time;
        }
        public GameTime()
        {
            Time = 5;
        }
    }
}
