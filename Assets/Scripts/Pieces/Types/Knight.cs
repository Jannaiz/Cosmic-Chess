using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    
    private void Awake()
    {
        setPos();
        pieceType = Board.PieceType.Knight;
    }
}
