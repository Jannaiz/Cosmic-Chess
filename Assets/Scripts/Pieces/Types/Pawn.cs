using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{
    private void Awake()
    {
        setPos();
        pieceType = Board.PieceType.Pawn;
    }
}
