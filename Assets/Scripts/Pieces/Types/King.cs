﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private void Awake()
    {
        setPos();
        pieceType = Board.PieceType.Knight;
    }
}