using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public enum PieceType
    {
        Empty = 0,
        Pawn = 1,
        Rook = 2,
        Knight = 3,
        Bishop = 4,
        Queen = 5,
        King = 6
    }

    public static int dimentions = 4;
    public static int[] size = { 8, 8, 4 ,4};

    public Plane[,,,] planes = new Plane[1, 1, size[2], size[3]];
    private Piece[,,,] MathBoard = new Piece[size[0], size[1], size[2], size[3]];

    private List<GameObject> ghosts = new List<GameObject>();

    [SerializeField] public Vector3 unityPiecePosOffset = new Vector3(0, 0, 0);


    [SerializeField] public GameObject ghostRook;
    [SerializeField] public GameObject ghostPawn;
    [SerializeField] public GameObject ghostQween;
    [SerializeField] public GameObject ghostKing;
    [SerializeField] public GameObject ghostKinght;
    [SerializeField] public GameObject ghostBishop;


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

            setPieceOnBoard(HighMath.getPieceMathPos(piece), piece);
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
            Debug.LogError("Moving piece on  " + startPos + " failed. There is no piece there.");
            return false;
        }

        // Get the piece on the end pos if there is one 
        Piece targetPiece = getPieceFromBoard(endPos);


        if (targetPiece != null)
        {
            if (targetPiece.white == movingPiece.white)
            {

                // If the target piece is of it's own color, do noting 
                return false;
            }
            // Destroy That piece
            Destroy(targetPiece.gameObject);
            setPieceOnBoard(endPos, null);  // Meaby unesesery
        }


        //Plane plane = getPlaneFromBoard(endPos);
        //movingPiece.gameObject.transform.SetParent(getPlaneFromBoard(endPos).gameObject.transform);
        movingPiece.transform.parent = getPlaneFromBoard(endPos).transform;
        movingPiece.mathPos = endPos;
        movingPiece.move(mathPosToUnityPos(endPos));
        setPieceOnBoard(endPos, movingPiece);
        setPieceOnBoard(startPos, null);

        Debug.Log("done");
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

    public void placeGhostPiece(Piece piece)
    {
        List<int[]> mathGhostPos;


        switch ((int)piece.pieceType)
        {
            case 1:
                Debug.Log("Gettin gosth of pawn");
                mathGhostPos = getPawnGhostPiecePos(piece.getPos(), (piece.white)?0:1);
                
                foreach (int[] ghostPos in mathGhostPos)
                {
                    Debug.Log(" -----");
                    Debug.Log(ghostPos[0]);
                    Debug.Log(ghostPos[1]);
                    Debug.Log(ghostPos[2]);

                    GameObject ghost = Instantiate(ghostPawn, mathPosToUnityPos(ghostPos), Quaternion.identity, getPlaneFromBoard(ghostPos).gameObject.transform);
                    ghosts.Add(ghost);
                    ghost.GetComponent<Piece>().updatePos();

                }

                break;
            case 2:
                Debug.Log("Gettin gosth of Rook");
                mathGhostPos = getRookGhostPiecePos(piece.getPos(), (piece.white) ? 0 : 1);

                foreach (int[] ghostPos in mathGhostPos)
                {
                    Debug.Log(" -----");
                    Debug.Log(ghostPos[0]);
                    Debug.Log(ghostPos[1]);
                    Debug.Log(ghostPos[2]);

                    GameObject ghost = Instantiate(ghostRook, mathPosToUnityPos(ghostPos), Quaternion.identity, getPlaneFromBoard(ghostPos).gameObject.transform);
                    ghosts.Add(ghost);
                    ghost.GetComponent<Piece>().updatePos();

                }
                break;

                
            case 3:
                
                Debug.Log("Gettin gosth of Knight");
                mathGhostPos = getKnightGhostPiecePos(piece.getPos(), (piece.white) ? 0 : 1);

                foreach (int[] ghostPos in mathGhostPos)
                {
                    Debug.Log(" -----");
                    Debug.Log(ghostPos[0]);
                    Debug.Log(ghostPos[1]);
                    Debug.Log(ghostPos[2]);

                    GameObject ghost = Instantiate(ghostKinght, mathPosToUnityPos(ghostPos), Quaternion.identity, getPlaneFromBoard(ghostPos).gameObject.transform);
                    ghosts.Add(ghost);
                    ghost.GetComponent<Piece>().updatePos();

                }
                break;
            case 4:

                

                Debug.Log("Gettin gosth of Bishop");
                mathGhostPos = getBishopGhostPiecePos(piece.getPos(), (piece.white) ? 0 : 1);

                foreach (int[] ghostPos in mathGhostPos)
                {
                    Debug.Log(" -----");
                    Debug.Log(ghostPos[0]);
                    Debug.Log(ghostPos[1]);
                    Debug.Log(ghostPos[2]);

                    GameObject ghost = Instantiate(ghostBishop, mathPosToUnityPos(ghostPos), Quaternion.identity, getPlaneFromBoard(ghostPos).gameObject.transform);
                    ghosts.Add(ghost);
                    ghost.GetComponent<Piece>().updatePos();

                }
                break;


            case 5:

                Debug.Log("Gettin gosth of Queen");
                mathGhostPos = getQueenGhostPiecePos(piece.getPos(), (piece.white) ? 0 : 1);

                foreach (int[] ghostPos in mathGhostPos)
                {
                    Debug.Log(" -----");
                    Debug.Log(ghostPos[0]);
                    Debug.Log(ghostPos[1]);
                    Debug.Log(ghostPos[2]);

                    GameObject ghost = Instantiate(ghostQween, mathPosToUnityPos(ghostPos), Quaternion.identity, getPlaneFromBoard(ghostPos).gameObject.transform);
                    ghosts.Add(ghost);
                    ghost.GetComponent<Piece>().updatePos();

                }

                
                break;
            case 6:

                Debug.Log("Gettin gosth of King");
                mathGhostPos = getKingGhostPiecePos(piece.getPos(), (piece.white) ? 0 : 1);

                foreach (int[] ghostPos in mathGhostPos)
                {
                    Debug.Log(" -----");
                    Debug.Log(ghostPos[0]);
                    Debug.Log(ghostPos[1]);
                    Debug.Log(ghostPos[2]);

                    GameObject ghost = Instantiate(ghostKing, mathPosToUnityPos(ghostPos), Quaternion.identity, getPlaneFromBoard(ghostPos).gameObject.transform);
                    ghosts.Add(ghost);
                    ghost.GetComponent<Piece>().updatePos();

                }


                break;


            default:
                
                break;
        }
    }

    public void deleteGhosts()
    {
        //Debug.LogError("Deleting");

        foreach(GameObject ghost in ghosts)
        {
            Destroy(ghost);
        }

        ghosts.Clear();

    }


    public Piece getPieceFromBoard(int[] mathPos)
    {
        //Debug.Log(mathPos[0] + " "+ mathPos[1] + " " + mathPos[2]);
        return MathBoard[mathPos[0], mathPos[1], mathPos[2], mathPos[3]];
    }
    public void setPieceOnBoard(int[] mathPos, Piece piece)
    {
        MathBoard[mathPos[0], mathPos[1], mathPos[2], mathPos[3]] = piece;
    }

    public Plane getPlaneFromBoard(int[] mathPos)
    {
        return planes[0, 0, mathPos[2], mathPos[3]];
    }


    public void setPlaneFromBoard(int[] mathPos, Plane plane)
    {
        planes[0, 0, mathPos[2], mathPos[3]] = plane;
    }

    public Vector3 mathPosToUnityPos(int[] mathPos)
    {
        Plane plane = getPlaneFromBoard(mathPos);
        Vector3 pos = plane.transform.position + new Vector3(mathPos[0], 0, mathPos[1]) + unityPiecePosOffset;

        return pos;
    }



    private List<int[]> getPawnGhostPiecePos(int[] mathPos, int color)
    {

        // white = 0
        // black = 1
        Debug.Log("getting gosth of"+ mathPos[0]+""+ mathPos[1]);

        


        List<int[]> mathGhostPos = new List<int[]>();



        // Look for none-attaking positions

        for(int n = 2; n <= dimentions; n++) {
            if( n % 2 == 0)
            {
                int [] tempPos =  (int[])mathPos.Clone();
                tempPos[n - 1] += (color == 1)? 1:-1 ;
                Debug.Log(" gosth of" + tempPos[1]);
                if (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos,-1))
                {
                    mathGhostPos.Add((int[])tempPos.Clone());
                }
            }
            else
            {
                int[] tempPos = (int[])mathPos.Clone();
                tempPos[n - 1] += 1;
                Debug.Log(" gosth of" + tempPos[1]);
                if (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos,-1))
                {
                    mathGhostPos.Add((int[])tempPos.Clone());
                }
                tempPos[n - 1] -= 2;
                Debug.Log(" gosth of" + tempPos[1]);
                if (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos,-1))
                {
                    mathGhostPos.Add((int[])tempPos.Clone());
                }
            }
        }

        // Look for attaking positions



        return mathGhostPos;

    }

    private List<int[]> getKnightGhostPiecePos(int[] mathPos, int color)
    {

        // white = 0
        // black = 1
        Debug.Log("getting gosth of" + mathPos[0] + "" + mathPos[1]);




        List<int[]> mathGhostPos = new List<int[]>();



        for(int dimention1 = 1; dimention1 <= dimentions; dimention1++)
        {
            for (int dimention2 = 1; dimention2 <= dimentions; dimention2++)
            {
                if (dimention1 == dimention2) continue;

                for (int offset1 = -1; offset1 <= 1; offset1 += 2)
                {
                    for (int offset2 = -2; offset2 <= 2; offset2 += 4)
                    {

                        int[] tempPos = (int[])mathPos.Clone();
                        tempPos[dimention1 - 1] += offset1;
                        tempPos[dimention2 - 1] += offset2;

                        if (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
                        {
                            mathGhostPos.Add((int[])tempPos.Clone());
                        }




                    }
                }

            }
        }

       



        return mathGhostPos;

    }



    private List<int[]> getRookGhostPiecePos(int[] mathPos, int color)
    {

        // white = 0
        // black = 1
        Debug.Log("getting gosth of" + mathPos[0] + "" + mathPos[1]);




        List<int[]> mathGhostPos = new List<int[]>();



        for (int dimention1 = 1; dimention1 <= dimentions; dimention1++)
        {
            int[] tempPos = (int[])mathPos.Clone();
            tempPos[dimention1 - 1] += 1;

            while (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
            {
                mathGhostPos.Add((int[])tempPos.Clone());
                tempPos[dimention1 - 1] += 1;
            }


            tempPos = (int[])mathPos.Clone();
            tempPos[dimention1 - 1] -= 1;
            while (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
            {
                mathGhostPos.Add((int[])tempPos.Clone());
                tempPos[dimention1 - 1] -= 1;
            }


        }





        return mathGhostPos;

    }

    private List<int[]> getBishopGhostPiecePos(int[] mathPos, int color)
    {

        // white = 0
        // black = 1
        Debug.Log("getting gosth of" + mathPos[0] + "" + mathPos[1]);




        List<int[]> mathGhostPos = new List<int[]>();



        for (int dimention1 = 1; dimention1 <= dimentions; dimention1++)
        {
            for (int dimention2 = dimention1+1; dimention2 <= dimentions; dimention2++)
            {
                if (dimention1 == dimention2) continue;

                for (int offset1 = -1; offset1 <= 1; offset1 += 2)
                {
                    for (int offset2 = -1; offset2 <= 1; offset2 += 2)
                    {

                        int[] tempPos = (int[])mathPos.Clone();
                        tempPos[dimention1 - 1] += offset1;
                        tempPos[dimention2 - 1] += offset2;


                        while (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
                        {
                            mathGhostPos.Add((int[])tempPos.Clone());
                            tempPos[dimention1 - 1] += offset1;
                            tempPos[dimention2 - 1] += offset2;
                        }
                        

                    }
                }

            }
        }


        





        return mathGhostPos;

    }


    private List<int[]> getQueenGhostPiecePos(int[] mathPos, int color)
    {

        // white = 0
        // black = 1
        Debug.Log("getting gosth of" + mathPos[0] + "" + mathPos[1]);




        List<int[]> mathGhostPos = new List<int[]>();



        for (int dimention1 = 1; dimention1 <= dimentions; dimention1++)
        {
            for (int dimention2 = dimention1 + 1; dimention2 <= dimentions; dimention2++)
            {
                if (dimention1 == dimention2) continue;

                for (int offset1 = -1; offset1 <= 1; offset1 += 2)
                {
                    for (int offset2 = -1; offset2 <= 1; offset2 += 2)
                    {

                        int[] tempPos = (int[])mathPos.Clone();
                        tempPos[dimention1 - 1] += offset1;
                        tempPos[dimention2 - 1] += offset2;


                        while (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
                        {
                            mathGhostPos.Add((int[])tempPos.Clone());
                            tempPos[dimention1 - 1] += offset1;
                            tempPos[dimention2 - 1] += offset2;
                        }


                    }
                }

            }
        }


        for (int dimention1 = 1; dimention1 <= dimentions; dimention1++)
        {
            int[] tempPos = (int[])mathPos.Clone();
            tempPos[dimention1 - 1] += 1;

            while (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
            {
                mathGhostPos.Add((int[])tempPos.Clone());
                tempPos[dimention1 - 1] += 1;
            }


            tempPos = (int[])mathPos.Clone();
            tempPos[dimention1 - 1] -= 1;
            while (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
            {
                mathGhostPos.Add((int[])tempPos.Clone());
                tempPos[dimention1 - 1] -= 1;
            }


        }





        return mathGhostPos;

    }


    private List<int[]> getKingGhostPiecePos(int[] mathPos, int color)
    {

        // white = 0
        // black = 1
        Debug.Log("getting gosth of" + mathPos[0] + "" + mathPos[1]);




        List<int[]> mathGhostPos = new List<int[]>();



        for (int dimention1 = 1; dimention1 <= dimentions; dimention1++)
        {
            for (int dimention2 = dimention1 + 1; dimention2 <= dimentions; dimention2++)
            {
                if (dimention1 == dimention2) continue;

                for (int offset1 = -1; offset1 <= 1; offset1 += 2)
                {
                    for (int offset2 = -1; offset2 <= 1; offset2 += 2)
                    {

                        int[] tempPos = (int[])mathPos.Clone();
                        tempPos[dimention1 - 1] += offset1;
                        tempPos[dimention2 - 1] += offset2;


                        if(isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
                        {
                            mathGhostPos.Add((int[])tempPos.Clone());
                            
                        }


                    }
                }

            }
        }


        for (int dimention1 = 1; dimention1 <= dimentions; dimention1++)
        {
            int[] tempPos = (int[])mathPos.Clone();
            tempPos[dimention1 - 1] += 1;

            if (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
            {
                mathGhostPos.Add((int[])tempPos.Clone());

            }

            tempPos = (int[])mathPos.Clone();
            tempPos[dimention1 - 1] -= 1;
            if (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
            {
                mathGhostPos.Add((int[])tempPos.Clone());

            }


        }





        return mathGhostPos;

    }



    public bool isPosOccupied(int[] mathPos, int color)
    {
        // all = -1
        // white = 0
        // black = 1

        // Gat what is on the board
        Piece piece = getPieceFromBoard(mathPos);

        if (piece == null)
        {
            // There is noting so return false
            return false;
        }

        if( color != -1)
        {
            // There is a color specified
            // Is the piece color the same as asked?
            if (((piece.white) ? 0 : 1) != color)
            {
                // There color that was asked is not there so return false
                return false;
            }

        }
        
        return true;
    }

    public bool isMathPosOnBoard(int[] mathPos)
    {

        if (mathPos.Length > dimentions)
        {
            return false;
        }
        for (int i = 0; i < mathPos.Length; i++)
        {
            if (mathPos[i] < 0)
            {
                // If the cordinate is negative it's not on the board.
                return false;
            }

            if (mathPos[i] >= size[i])
            {
                // If the cordinate is lager that the size of that dimention it's not on the board.
                return false;
            }
        }

        return true;
    }


    /*

    public int[] getPieceMathPos(Piece piece)
    {
        int[] mathPos = CombineDimention(piece.mathPos, removeLowerDimention(piece.getPlane().originMathPos));
        return mathPos;


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

    */


}
