using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Models
{
    public abstract class PiecesModel(Grid grd)
    {
        public Piece[,] Pieces = new Piece[8, 8];
        
    }
}
