using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{

    

    public int angleAmount => 1;

    private void Awake()
    {
        setPos();
        pieceType = Board.PieceType.Pawn;
        




    }
    
}



