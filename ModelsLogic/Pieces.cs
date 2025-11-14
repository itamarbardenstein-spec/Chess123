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
                if(i==0 || i==7)
                {
                    Pieces[0, i] = new Piece(i, 0, Piece.PieceType.Rook, false, Strings.BlackRook);
                    Pieces[7, i] = new Piece(i, 0, Piece.PieceType.Rook, true, Strings.WhiteRook);
                }
                else if(i==1 || i==6)
                {
                    Pieces[0, i] = new Piece(i, 0, Piece.PieceType.Knight, false, Strings.BlackKnight);
                    Pieces[7, i] = new Piece(i, 0, Piece.PieceType.Knight, true, Strings.WhiteKnight);
                }
                else if(i==2 || i==5)
                {
                    Pieces[0, i] = new Piece(i, 0, Piece.PieceType.Bishop, false, Strings.BlackBishop);
                    Pieces[7, i] = new Piece(i, 0, Piece.PieceType.Bishop, true, Strings.WhiteBishop);
                }
                else if(i==3)
                {
                    Pieces[0, i] = new Piece(i, 0, Piece.PieceType.Queen, false, Strings.BlackQueen);
                    Pieces[7, i] = new Piece(i, 0, Piece.PieceType.Queen, true, Strings.WhiteQueen);
                }
                else
                {
                    Pieces[0, i] = new Piece(i, 0, Piece.PieceType.King, false, Strings.BlackKing);
                    Pieces[7, i] = new Piece(i, 0, Piece.PieceType.King, true, Strings.WhiteKing);
                }                               
            }
            for (int i = 0; i < Pieces.GetLength(0); i++)
            {
                Pieces[1, i] = new Piece(i, 0, Piece.PieceType.Pawn, false, Strings.BlackPawn);
            }
            for (int i = 0; i < Pieces.GetLength(0); i++)
            {
                Pieces[6, i] = new Piece(i, 0, Piece.PieceType.Pawn, true, Strings.WhitePawn);
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
