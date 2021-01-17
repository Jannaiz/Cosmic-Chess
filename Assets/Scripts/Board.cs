using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public enum PieceType
    {
        Empty,
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }

    public static int dimentions = 3;
    public static int[] size = { 8, 8, 2 };

    public Plane[,,] planes = new Plane[1, 1, 2];
    private Piece[,,] MathBoard = new Piece[size[0], size[1], size[2]];

    [SerializeField] public Vector3 unityPiecePosOffset = new Vector3(0, 0, 0);


    // Start is called before the first frame update
    void Start()
    {
        makeBoard();
    }

    public void makeBoard()
    {


        foreach (Plane plane in FindObjectsOfType<Plane>())
        {

            setPlaneFromBoard(plane.originMathPos, plane);

        }


        //Debug.Log(mathBoard[0, 0]);
        //List<Piece> piecesOnBoard = new List<Piece>();
        int n = 0;
        foreach (Piece piece in FindObjectsOfType<Piece>())
        {
            if (piece.pieceType == PieceType.Empty) continue;
            /*Debug.Log("------");
            Debug.Log(Mathf.Floor(piece.transform.position.x));
            Debug.Log(Mathf.Floor(piece.transform.position.z));
            Debug.Log(piece.pieceType);
            Debug.Log(piece.white);
            Debug.Log(piece.mathPos[0]+ " "+ piece.mathPos[1]);*/
            //MathBoard[(int)(Mathf.Floor(piece.transform.position.x)), (int)(Mathf.Floor(piece.transform.position.z)), 0] = piece;

            setPieceOnBoard(getPieceMathPos(piece), piece);
            n++;
        }
        /* Debug.Log(mathBoard[0, 0]);
         Debug.Log(n);*/

        /*for (int z = 0; z < 8; z++)
        {
            for (int x = 0; x < 8; x++)
            {
                Debug.Log(mathBoard[z, x] + " " + z + "," + x);
            }
        }*/



    }

    // Update is called once per frame
    void Update()
    {

    }


    public bool move(int[] startPos, int[] endPos)
    {


        // Get the piece that we want to move
        Piece movingPiece = getPieceFromBoard(startPos);

        if (movingPiece == null)
        {
            //Debug.LogError("Moving piece on  " + startPos + " failed. There is no piece there.");
            return false;
        }

        // Get the piece on the end pos if there is one 
        Piece targetPiece = getPieceFromBoard(endPos);


        if (targetPiece != null)
        {
            if (targetPiece.white == movingPiece.white)
            {
                return false;
            }
            // Destroy That piece
            Destroy(targetPiece.gameObject);
            setPieceOnBoard(endPos, null);  // Meaby unesesery
        }


        //Plane plane = getPlaneFromBoard(endPos);
        //movingPiece.gameObject.transform.SetParent(getPlaneFromBoard(endPos).gameObject.transform);
        movingPiece.gameObject.transform.parent = getPlaneFromBoard(endPos).gameObject.transform;
        movingPiece.mathPos = endPos;
        movingPiece.move(mathPosToUnityPos(endPos));
        setPieceOnBoard(endPos, movingPiece);
        setPieceOnBoard(startPos, null);


        /*


    int[] mathPos = piece.mathPos;

    MathBoard[mathPos[0], mathPos[1]] = null; 
    piece.move(pos);
    mathPos[0] = (int)(Mathf.Floor(pos.x));
    mathPos[1] = (int)(Mathf.Floor(pos.z));

    MathBoard[mathPos[0], mathPos[1]] = piece;
    for (int z = 0; z < 8; z++)
    {
        for (int x = 0; x < 8; x++)
        {
            Debug.Log(MathBoard[z, x] + " " + z + "," + x);
        }
    }*/



        return true;
    }



    /// <summary>
    /// Combine the lower 2D part and higher dimention part of Posistions
    /// </summary>
    /// <param name="LowerMathPos"> list of 2D Posistions</param>
    /// <param name="HighMathPos"><list of Posistions above 2D</param>
    /// <returns>The combined Posistions</returns>
    public int[] CombineDimention(int[] LowerMathPos, int[] HighMathPos)
    {
        int[] FullMathPos = { LowerMathPos[0], LowerMathPos[1], HighMathPos[0] };
        return FullMathPos;
    }


    public int[] removeHigherDimention(int[] FullMathPos)
    {
        int[] LowerMathPos = { FullMathPos[0], FullMathPos[0] };
        return LowerMathPos;
    }

    public int[] removeLowerDimention(int[] FullMathPos)
    {
        int[] HighMathPos = { FullMathPos[2] };
        return HighMathPos;
    }


    public Piece getPieceFromBoard(int[] mathPos)
    {
        //Debug.Log(mathPos[0] + " "+ mathPos[1] + " " + mathPos[2]);
        return MathBoard[mathPos[0], mathPos[1], mathPos[2]];
    }
    public void setPieceOnBoard(int[] mathPos, Piece piece)
    {
        MathBoard[mathPos[0], mathPos[1], mathPos[2]] = piece;
    }

    public Plane getPlaneFromBoard(int[] mathPos)
    {
        return planes[0, 0, mathPos[2]];
    }
    public void setPlaneFromBoard(int[] mathPos, Plane plane)
    {
        planes[0, 0, mathPos[2]] = plane;
    }

    public int[] getPieceMathPos(Piece piece)
    {
        int[] mathPos = CombineDimention(piece.mathPos, removeLowerDimention(piece.getPlane().originMathPos));
        return mathPos;


    }

    public Vector3 mathPosToUnityPos(int[] mathPos)
    {
        Plane plane = getPlaneFromBoard(mathPos);
        Vector3 pos = plane.transform.position + new Vector3(mathPos[0], 0, mathPos[1]) + unityPiecePosOffset;

        return pos;
    }

}
