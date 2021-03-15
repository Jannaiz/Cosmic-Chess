using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece 
{


    private void Awake()
    {
        setPos();
        pieceType = Board.PieceType.Bishop;
    }
    private void Start()
    {
        
    }
}
