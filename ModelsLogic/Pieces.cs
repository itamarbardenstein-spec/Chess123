using Chess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.ModelsLogic
{
    public class Pieces:PiecesModel
    {
        public Pieces(Grid grd) : base(grd)
        {
            for(int i=0; i< Pieces.GetLength(0); i++)
            {
                Pieces[0, i] = new Piece(i, 0, Piece.PieceType.Rook, false, Strings.Robash);
            }
            for (int i = 0; i < Pieces.GetLength(0); i++)
            {
                Pieces[1, i] = new Piece(i, 0, Piece.PieceType.Rook, false, Strings.Robash);
            }
            for (int i = 0; i < Pieces.GetLength(0); i++)
            {
                Pieces[6, i] = new Piece(i, 0, Piece.PieceType.Rook, false, Strings.Robash);
            }
            for (int i = 0; i < Pieces.GetLength(0); i++)
            {
                Pieces[7, i] = new Piece(i, 0, Piece.PieceType.Rook, false, Strings.Robash);
            }
            for (int i = 0; i < Pieces.GetLength(0); i++)
            {
                for (int j = 0; j < Pieces.GetLength(1); j++)
                {
                    if(Pieces[i,j] != null)
                    {
                        grd.Add(Pieces[i, j], j, i);
                    }
                }
            }
        }
    }
}
