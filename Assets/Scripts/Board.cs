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

    /*public static int dimensions = 5;
    public static int[] size = { 8, 8, 2 ,2,2};

    public Plane[,,,,] planes = new Plane[1, 1, size[2], size[3], size[4]];
    private Piece[,,,,] MathBoard = new Piece[size[0], size[1], size[2], size[3], size[4]];*/
    // The amount of dimensions being used
    public static int dimensions = 10;
    public static int[] size = { 8, 8, 6, 6,  /*e5:*/ 1,
                                              /*e6:*/ 1,
                                              /*e7:*/ 1,
                                              /*e8:*/ 1,
                                              /*e9:*/ 1,
                                              /*e10:*/ 1 };

    public Plane[,,,,,,,,,] planes = new Plane[1, 1, size[2], size[3], size[4], size[5], size[6], size[7], size[8], size[9]];
    private Piece[,,,,,,,,,] MathBoard = new Piece[size[0], size[1], size[2], size[3], size[4], size[5], size[6], size[7], size[8], size[9]];

    private List<GameObject> ghosts = new List<GameObject>();

    [SerializeField] public Vector3 unityPiecePosOffset = new Vector3(0, 0, 0);


    [SerializeField] public GameObject ghostRook;
    [SerializeField] public GameObject ghostPawn;
    [SerializeField] public GameObject ghostQueen;
    [SerializeField] public GameObject ghostKing;
    [SerializeField] public GameObject ghostKnight;
    [SerializeField] public GameObject ghostBishop;

    [SerializeField] public GameObject defDecorPref;
    [SerializeField] public GameObject planePrefSetUp;
    [SerializeField] public GameObject planePref;

    [SerializeField] public GameObject Decor;


    [SerializeField] public GameObject Rook;
    [SerializeField] public GameObject Pawn;
    [SerializeField] public GameObject Queen;
    [SerializeField] public GameObject King;
    [SerializeField] public GameObject Knight;
    [SerializeField] public GameObject Bishop;


    //private List<int[], GameObject> places = new List< int[], GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        initializeBoard();

        makeBoard();
    }
    public void initializeBoard()
    {
        int planeAmount = size[2] * size[3] * size[4] * size[5] * size[6] * size[7] * size[8] * size[9];

        for(int n3 = 0; n3 < size[2]; n3++)
        {
            for (int n4 = 0; n4 < size[3]; n4++)
            {
                for (int n5 = 0; n5 < size[4]; n5++)
                {
                    for (int n6 = 0; n6 < size[5]; n6++)
                    {


                        for (int n7 = 0; n7 < size[6]; n7++)
                        {

                            for (int n8 = 0; n8 < size[7]; n8++)
                            {

                                for (int n9 = 0; n9 < size[8]; n9++)
                                {

                                    for (int n10 = 0; n10 < size[9]; n10++)
                                    {
                                        Vector3 pos = new Vector3(n3 * 10
                                                                   + ((n5 == 0) ? 0 : 1) *n5* size[2] * 10 + n5 * 1.5f
                                                                   + ((n7 == 0) ? 0 : 1) *n7* size[2] * size[4] * 10 + n7 * 10
                                                                   + ((n9 == 0) ? 0 : 1) *n9* size[2] * size[4] * size[6] * 10 + n9 * 20
                                            , 0,
                                                                   n4 * 10
                                                                   + ((n6 == 0) ? 0 : 1)*n6 * size[3] * 10 + n6 * 1.5f
                                                                   + ((n8 == 0) ? 0 : 1) *n8* size[3]  * size[5] * 10 + n8 * 10
                                                                   + ((n10 == 0) ? 0 : 1) *n10* size[3] * size[5]  * size[7] * 10 + n10 * 20
                                                                   );
                                        GameObject plane = Instantiate(planePrefSetUp, pos, Quaternion.identity, this.transform);
                                        int[] originMathPos = { 0, 0, n3, n4, n5, n6, n7, n8, n9, n10 };
                                        plane.GetComponent<Plane>().originMathPos = originMathPos;

                                        GameObject decorOfPlane = Instantiate(defDecorPref, pos, Quaternion.identity, Decor.transform);


                                        decorOfPlane.transform.GetChild(0).GetComponent<TextMesh>().text = HighMath.mathPosToString(originMathPos );


                                       GameObject Piece = Instantiate(Knight, pos + new Vector3(1+0.5f, 0.5f, 2+0.5f), Quaternion.identity, plane.transform);
                                        //GameObject Piece2 = Instantiate(Pawn, pos+ new Vector3(0.5f,0.5f,0.5f)*2, Quaternion.identity, plane.transform);
                                        //Instantiate(Knight, pos + new Vector3(1.5f, 0.5f, 1.5f) * 2, Quaternion.identity, plane.transform);





                                    }
                                }
                            }
                        }


                    }
                }
            }
        }



        /*
          Vector3 pos = new Vector3(n3 * 10 + ((n5==0)?0:1)*size[2]*10+ n5* 1.5f, 0, n4 * 10 + ((n6 == 0) ? 0 : 1) * size[3] * 10 + n6 * 1.5f);
                        GameObject plane = Instantiate(planePref, pos, Quaternion.identity, this.transform);
                        int[] originMathPos = { 0, 0, n3, n4, n5, n6, 0, 0, 0, 0 };
                        plane.GetComponent<Plane>().originMathPos = originMathPos;

                        GameObject decorOfPlane = Instantiate(defDecorPref, pos, Quaternion.identity, Decor.transform);

         * */

    }
    public void makeBoard()
    {


        foreach (Plane plane in FindObjectsOfType<Plane>())
        {
            //Debug.Log("Plane co:");
            //Debug.Log(HighMath.mathPosToString(plane.originMathPos));
            setPlaneFromBoard(plane.originMathPos, plane);

        }
       // Debug.Log("------");


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

            setPieceOnBoard(piece.getPos(), piece);
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
           // Debug.LogError("Moving piece on  " + startPos + " failed. There is no piece there.");
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
            setPieceOnBoard(endPos, null);  // Maybe unnecessary
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
                //Debug.Log("Gettin ghost of pawn");
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
                //Debug.Log("Gettin ghost of Rook");
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

                //Debug.Log("Gettin ghost of Knight");
                mathGhostPos = getKnightGhostPiecePos(piece.getPos(), (piece.white) ? 0 : 1);

                foreach (int[] ghostPos in mathGhostPos)
                {
                    Debug.Log(" -----");
                    Debug.Log(ghostPos[0]);
                    Debug.Log(ghostPos[1]);
                    Debug.Log(ghostPos[2]);

                    GameObject ghost = Instantiate(ghostKnight, mathPosToUnityPos(ghostPos), Quaternion.identity, getPlaneFromBoard(ghostPos).gameObject.transform);
                    ghosts.Add(ghost);
                    ghost.GetComponent<Piece>().updatePos();

                }
                break;
            case 4:



                //Debug.Log("Gettin gosth of Bishop");
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

                //Debug.Log("Gettin ghost of Queen");
                mathGhostPos = getQueenGhostPiecePos(piece.getPos(), (piece.white) ? 0 : 1);

                foreach (int[] ghostPos in mathGhostPos)
                {
                    Debug.Log(" -----");
                    Debug.Log(ghostPos[0]);
                    Debug.Log(ghostPos[1]);
                    Debug.Log(ghostPos[2]);

                    GameObject ghost = Instantiate(ghostQueen, mathPosToUnityPos(ghostPos), Quaternion.identity, getPlaneFromBoard(ghostPos).gameObject.transform);
                    ghosts.Add(ghost);
                    ghost.GetComponent<Piece>().updatePos();

                }


                break;
            case 6:

                //Debug.Log("Gettin ghost of King");
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
        return MathBoard[mathPos[0], mathPos[1], mathPos[2], mathPos[3], mathPos[4], mathPos[5], mathPos[6], mathPos[7], mathPos[8], mathPos[9]];
    }
    public void setPieceOnBoard(int[] mathPos, Piece piece)
    {

        MathBoard[mathPos[0], mathPos[1], mathPos[2], mathPos[3], mathPos[4], mathPos[5], mathPos[6], mathPos[7], mathPos[8], mathPos[9]] = piece;
    }

    public Plane getPlaneFromBoard(int[] mathPos)
    {
        return planes[0, 0, mathPos[2], mathPos[3], mathPos[4], mathPos[5], mathPos[6], mathPos[7], mathPos[8], mathPos[9]];
    }


    public void setPlaneFromBoard(int[] mathPos, Plane plane)
    {
        planes[0, 0, mathPos[2], mathPos[3], mathPos[4], mathPos[5], mathPos[6], mathPos[7], mathPos[8], mathPos[9]] = plane;
    }

    public Vector3 mathPosToUnityPos(int[] mathPos)
    {
        Plane plane = getPlaneFromBoard(mathPos);
        //Debug.Log(HighMath.mathPosToString(mathPos)+"   :"+plane);
        Vector3 pos = plane.transform.position + new Vector3(mathPos[0], 0, mathPos[1]) + unityPiecePosOffset;

        return pos;
    }



    private List<int[]> getPawnGhostPiecePos(int[] mathPos, int color)
    {

        // white = 0
        // black = 1
       // Debug.Log("--------------------");
        //Debug.Log("getting gosth of "+ HighMath.mathPosToString(mathPos));




        List<int[]> mathGhostPos = new List<int[]>();



        // Look for none-attacking positions

        for(int n = 2; n <= dimensions; n++) {
            //Debug.Log("Looking at the " + n + " 'th dimension");
            if( n % 2 == 0)
            {

                int [] tempPos =  (int[])mathPos.Clone();
                tempPos[n - 1] += (color == 1)? -1:1 ;
                //Debug.Log("attacking " + HighMath.mathPosToString(tempPos));
                //Debug.Log("on Board? " + isMathPosOnBoard(tempPos));
                if (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos,-1))
                {
                    //Debug.Log("occupied? " +  "No");
                    //Debug.Log("adding "+HighMath.mathPosToString(tempPos));
                    mathGhostPos.Add((int[])tempPos.Clone());
                }
                else
                {
                    //Debug.Log("occupied? " + (isMathPosOnBoard(tempPos)?"Yes":"No"));
                }
            }
            else
            {
                int[] tempPos = (int[])mathPos.Clone();
                tempPos[n - 1] += 1;

               // Debug.Log("cheking " + HighMath.mathPosToString(tempPos));
                //Debug.Log("on Board? " + isMathPosOnBoard(tempPos));
                if (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, -1))
                {
                    //Debug.Log("occupied? " + "No");
                    //Debug.Log("adding " + HighMath.mathPosToString(tempPos));
                    mathGhostPos.Add((int[])tempPos.Clone());
                }
                else
                {
                    //Debug.Log("occupied? " + (isMathPosOnBoard(tempPos) ? "Yes" : "No"));
                }
                tempPos[n - 1] -= 2;

                //Debug.Log("cheking " + HighMath.mathPosToString(tempPos));
                //Debug.Log("on Board? " + isMathPosOnBoard(tempPos));
                if (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, -1))
                {
                    //Debug.Log("occupied? " + "No");
                    //Debug.Log("adding " + HighMath.mathPosToString(tempPos));
                    mathGhostPos.Add((int[])tempPos.Clone());
                }
                else
                {
                    //Debug.Log("occupied? " + (isMathPosOnBoard(tempPos) ? "Yes" : "No"));
                }
            }
        }

        // Look for attaking positions

        for(int n1 = 1; n1 <= dimensions; n1++)
        {
            for (int n2 = 1; n2 <= dimensions; n2++)
            {
                if (n1 % 2 != 0 && n2 % 2 != 0) continue;
                if (n1 == n2 ) continue;

                for (int offset1 = (n1%2==0)?-1:1; offset1 <= 1; offset1+=2)
                {

                    for (int offset2 = (n1 % 2 == 0) ? -1 : 1; offset2 <= 1; offset2 += 2)
                    {

                        int[] tempPos = (int[])mathPos.Clone();
                        tempPos[n1 - 1] += offset1;
                        tempPos[n2 - 1] += offset2;

                        // Debug.Log("cheking " + HighMath.mathPosToString(tempPos));
                        //Debug.Log("on Board? " + isMathPosOnBoard(tempPos));
                        if (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color) && isPosOccupied(tempPos, -1))
                        {
                            //Debug.Log("occupied? " + "No");
                            //Debug.Log("adding " + HighMath.mathPosToString(tempPos));
                            mathGhostPos.Add((int[])tempPos.Clone());
                        }

                    }

                }


            }


        }

        return mathGhostPos;

    }

    private List<int[]> getKnightGhostPiecePos(int[] mathPos, int color)
    {

        // white = 0
        // black = 1
        Debug.Log("getting gosth of" + mathPos[0] + "" + mathPos[1]);




        List<int[]> mathGhostPos = new List<int[]>();



        for(int dimension1 = 1; dimension1 <= dimensions; dimension1++)
        {
            for (int dimension2 = 1; dimension2 <= dimensions; dimension2++)
            {
                if (dimension1 == dimension2) continue;

                for (int offset1 = -1; offset1 <= 1; offset1 += 2)
                {
                    for (int offset2 = -2; offset2 <= 2; offset2 += 4)
                    {

                        int[] tempPos = (int[])mathPos.Clone();
                        tempPos[dimension1 - 1] += offset1;
                        tempPos[dimension2 - 1] += offset2;

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



        for (int dimension1 = 1; dimension1 <= dimensions; dimension1++)
        {
            int[] tempPos = (int[])mathPos.Clone();
            tempPos[dimension1 - 1] += 1;

            while (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
            {

                mathGhostPos.Add((int[])tempPos.Clone());
                if(isPosOccupied(tempPos, -1))
                {
                    //Is there an piece, (the enemy) stop while loop
                    break;
                }
                tempPos[dimension1 - 1] += 1;
            }




            tempPos = (int[])mathPos.Clone();
            tempPos[dimension1 - 1] -= 1;
            while (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
            {
                mathGhostPos.Add((int[])tempPos.Clone());
                if (isPosOccupied(tempPos, -1))
                {

                    break;
                }
                tempPos[dimension1 - 1] -= 1;
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



        for (int dimension1 = 1; dimension1 <= dimensions; dimension1++)
        {
            for (int dimension2 = dimension1+1; dimension2 <= dimensions; dimension2++)
            {
                if (dimension1 == dimension2) continue;

                for (int offset1 = -1; offset1 <= 1; offset1 += 2)
                {
                    for (int offset2 = -1; offset2 <= 1; offset2 += 2)
                    {

                        int[] tempPos = (int[])mathPos.Clone();
                        tempPos[dimension1 - 1] += offset1;
                        tempPos[dimension2 - 1] += offset2;


                        while (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
                        {
                            mathGhostPos.Add((int[])tempPos.Clone());
                            if (isPosOccupied(tempPos, -1))
                            {
                                //Is there an piece, (the enemy) stop while loop
                                break;
                            }
                            tempPos[dimension1 - 1] += offset1;
                            tempPos[dimension2 - 1] += offset2;
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



        for (int dimension1 = 1; dimension1 <= dimensions; dimension1++)
        {
            for (int dimension2 = dimension1 + 1; dimension2 <= dimensions; dimension2++)
            {
                if (dimension1 == dimension2) continue;

                for (int offset1 = -1; offset1 <= 1; offset1 += 2)
                {
                    for (int offset2 = -1; offset2 <= 1; offset2 += 2)
                    {

                        int[] tempPos = (int[])mathPos.Clone();
                        tempPos[dimension1 - 1] += offset1;
                        tempPos[dimension2 - 1] += offset2;


                        while (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
                        {
                            mathGhostPos.Add((int[])tempPos.Clone());
                            if (isPosOccupied(tempPos, -1))
                            {
                                //Is there an piece, (the enemy) stop while loop
                                break;
                            }
                            tempPos[dimension1 - 1] += offset1;
                            tempPos[dimension2 - 1] += offset2;
                        }


                    }
                }

            }
        }


        for (int dimension1 = 1; dimension1 <= dimensions; dimension1++)
        {
            int[] tempPos = (int[])mathPos.Clone();
            tempPos[dimension1 - 1] += 1;

            while (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
            {
                mathGhostPos.Add((int[])tempPos.Clone());
                tempPos[dimension1 - 1] += 1;
            }


            tempPos = (int[])mathPos.Clone();
            tempPos[dimension1 - 1] -= 1;
            while (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
            {
                mathGhostPos.Add((int[])tempPos.Clone());
                if (isPosOccupied(tempPos, -1))
                {
                    //Is there an piece, (the enemy) stop while loop
                    break;
                }
                tempPos[dimension1 - 1] -= 1;
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



        for (int dimension1 = 1; dimension1 <= dimensions; dimension1++)
        {
            for (int dimension2 = dimension1 + 1; dimension2 <= dimensions; dimension2++)
            {
                if (dimension1 == dimension2) continue;

                for (int offset1 = -1; offset1 <= 1; offset1 += 2)
                {
                    for (int offset2 = -1; offset2 <= 1; offset2 += 2)
                    {

                        int[] tempPos = (int[])mathPos.Clone();
                        tempPos[dimension1 - 1] += offset1;
                        tempPos[dimension2 - 1] += offset2;


                        if(isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
                        {
                            mathGhostPos.Add((int[])tempPos.Clone());

                        }


                    }
                }

            }
        }


        for (int dimension1 = 1; dimension1 <= dimensions; dimension1++)
        {
            int[] tempPos = (int[])mathPos.Clone();
            tempPos[dimension1 - 1] += 1;

            if (isMathPosOnBoard(tempPos) && !isPosOccupied(tempPos, color))
            {
                mathGhostPos.Add((int[])tempPos.Clone());

            }

            tempPos = (int[])mathPos.Clone();
            tempPos[dimension1 - 1] -= 1;
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

        if (mathPos.Length > dimensions)
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
                // If the cordinate is lager that the size of that dimension it's not on the board.
                return false;
            }
        }

        return true;
    }


    /*

    public int[] getPieceMathPos(Piece piece)
    {
        int[] mathPos = Combinedimension(piece.mathPos, removeLowerDimension(piece.getPlane().originMathPos));
        return mathPos;


    }

    /// <summary>
    /// Combine the lower 2D part and higher dimenstion part of Posistions
    /// </summary>
    /// <param name="LowerMathPos"> list of 2D Posistions</param>
    /// <param name="HighMathPos"><list of Posistions above 2D</param>
    /// <returns>The combined Posistions</returns>
    public int[] CombineDimenstion(int[] LowerMathPos, int[] HighMathPos)
    {
        int[] FullMathPos = { LowerMathPos[0], LowerMathPos[1], HighMathPos[0] };
        return FullMathPos;
    }


    public int[] removeHigherDimenstion(int[] FullMathPos)
    {
        int[] LowerMathPos = { FullMathPos[0], FullMathPos[0] };
        return LowerMathPos;
    }

    public int[] removeLowerDimenstion(int[] FullMathPos)
    {
        int[] HighMathPos = { FullMathPos[2] };
        return HighMathPos;
    }

    */


}
