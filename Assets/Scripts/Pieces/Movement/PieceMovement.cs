using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private bool whiteTurn = true;

    private Piece currentPiece;
    private bool canceling = false;

    private Material currentMaterial;

    [SerializeField] private Material selectedMaterial;

    private Board Board;
    private TCPJoin network;

    [SerializeField] public bool white = false;

    [SerializeField] private bool local = false;

    [SerializeField] private bool Debuging = true;
    [SerializeField] public int[] selectPlacePos = { 0,0,1,0,0,0,0,0,0,0};
    //[SerializeField] public GameObject selecter ;


    private void Start()
    {
        network = FindObjectOfType<TCPJoin>();
        cam = GetComponent<Camera>();
        Board = FindObjectOfType<Board>();
    }
    

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.GetComponent<Piece>())
                {
                    if ((local || whiteTurn == white) )
                    {
                        if (hit.transform.gameObject.GetComponent<Piece>().white == whiteTurn && !hit.transform.gameObject.GetComponent<Piece>().isGhost)
                        {
                            Deselect();
                            currentPiece = hit.transform.gameObject.GetComponent<Piece>();
                            currentMaterial = currentPiece.GetComponentInChildren<MeshRenderer>().material;
                            currentPiece.GetComponentInChildren<MeshRenderer>().material = selectedMaterial;
                            Board.deleteGhosts();
                            Board.placeGhostPiece(currentPiece);
                            Debug.Log("selected " + HighMath.mathPosToString(currentPiece.getPos()));

                        }
                    }
                }
            }
        } else if(Input.GetMouseButtonUp(0))
        {
            if (currentPiece != null)
            {
                if (!canceling)
                {
                    RaycastHit hit;
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out hit))
                    {
                        // TODO: if hit is ghost piece or enemy within legal distance
                        /*if (!hit.transform.gameObject.GetComponent<Piece>())
                        {
                            //currentPiece.transform.position = new Vector3(hit.transform.position.x, currentPiece.transform.position.y, hit.transform.position.z);
                            Board.GetComponent<Board>().move(currentPiece, new Vector3(hit.transform.position.x, currentPiece.transform.position.y, hit.transform.position.z));
                            Deselect();
                            whiteTurn = !whiteTurn;
                        }
                        else
                        {
                            if (hit.transform.gameObject.GetComponent<Piece>().white != whiteTurn)
                            {
                                //currentPiece.transform.position = new Vector3(hit.transform.position.x, currentPiece.transform.position.y, hit.transform.position.z);
                                Board.GetComponent<Board>().move(currentPiece, new Vector3(hit.transform.position.x, currentPiece.transform.position.y, hit.transform.position.z));
                                Deselect();
                                Destroy(hit.transform.gameObject);
                                whiteTurn = !whiteTurn;
                            }
                        }*/

                        int[] startMathPos = HighMath.getPieceMathPos(currentPiece);
                        int[] endMathPos = new int[3];

                        if (hit.transform.gameObject.GetComponent<Piece>())
                        {
                            if (hit.transform.gameObject.GetComponent<Piece>().isGhost)
                            {


                                //endMathPos[0] = hit.transform.gameObject.GetComponent<Piece>().mathPos[0];
                                //endMathPos[1] = hit.transform.gameObject.GetComponent<Piece>().mathPos[1];
                                //endMathPos[2] = 0;

                                endMathPos = HighMath.getPieceMathPos(hit.transform.gameObject.GetComponent<Piece>());

                                if (move(startMathPos, endMathPos))
                                {
                                    if (!local)
                                    {
                                        network.SendMove(startMathPos, endMathPos);
                                    }
                                    Deselect();
                                    whiteTurn = !whiteTurn;
                                    Board.deleteGhosts();
                                }
                            }
                            else
                            {
                                /*endMathPos[0] = (int)(Mathf.Floor(hit.transform.localPosition.x));
                                endMathPos[1] = (int)(Mathf.Floor(hit.transform.localPosition.z));
                                endMathPos[2] = 0;*/
                            }
                        }
                        else
                        {
                            /*endMathPos[0] = (int)(Mathf.Floor(hit.transform.localPosition.x));
                            endMathPos[1] = (int)(Mathf.Floor(hit.transform.localPosition.z));
                            endMathPos[2] = 0;*/
                        }


                        //Board.deleteGhosts();


                        
                        

                    }
                    else
                    {
                        Deselect();
                    }
                } else
                {
                    canceling = false;
                }
            }
        }




        // Cheats to make testing easyer, (Jannes)
        if (Debuging)
        {
            /*if (Input.GetMouseButtonUp(1))
            {
                Debug.Log("Yes 1");
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    Debug.Log(hit.transform.gameObject);
                    if (hit.transform.gameObject.GetComponent<Plane>())
                    {
                        Board.placePiece(hit.transform.position, hit.transform.gameObject.GetComponent<Plane>(), Board.PieceType.Pawn, true);
                    }

                    
                }
             }*/

            if (Input.GetKeyDown(KeyCode.LeftShift) || true)
            {
                
                if (Input.GetKeyUp(KeyCode.UpArrow))
                {
                    
                    selectPlacePos[1] +=1;

                    
                }
                else if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    selectPlacePos[1] -= 1;
                }


                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    selectPlacePos[0] -= 1;
                }
                else if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    selectPlacePos[0] += 1;
                }

                selectPlacePos[0] =  Math.Max(0,  Math.Min(selectPlacePos[0], Board.size[0]-1));
                selectPlacePos[1] = Math.Max(0, Math.Min(selectPlacePos[1], Board.size[1]-1));

                //if (selecter != null)
                //{
                //    Debug.Log("jndfsqf");
                //    selecter.transform.position = Board.mathPosToUnityPos(selectPlacePos) + Vector3.up;
                //    Debug.Log((Board.mathPosToUnityPos(selectPlacePos) + Vector3.up).x + " " + (Board.mathPosToUnityPos(selectPlacePos) + Vector3.up).z);
                //}

            }

            if (Input.GetKeyUp(KeyCode.Return))
            {
                Debug.Log(HighMath.mathPosToString(selectPlacePos));

                Board.PieceType piecType = Board.PieceType.Empty;

                if (Input.GetKey(KeyCode.A))
                {
                    
                    piecType = Board.PieceType.Pawn;

                }else if (Input.GetKey(KeyCode.Z))
                {
                    
                    piecType = Board.PieceType.Rook;
                }
                else if (Input.GetKey(KeyCode.E))
                {
                    piecType = Board.PieceType.Knight;

                }
                else if (Input.GetKey(KeyCode.R))
                {
                    piecType = Board.PieceType.Bishop;
                }
                else if (Input.GetKey(KeyCode.T))
                {
                    piecType = Board.PieceType.Queen;

                }
                else if (Input.GetKey(KeyCode.Y))
                {
                    piecType = Board.PieceType.King;
                }

                Debug.Log(piecType);
                

                Board.placePiece(selectPlacePos, piecType, Input.GetKey(KeyCode.Tab));
            }

            
            if (Input.GetKeyUp(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
            {
                System.DateTime theTime = System.DateTime.Now;
                string date = theTime.Year + "-" + theTime.Month + "-" + theTime.Day;
                string time = date + "T" + theTime.Hour + ":" + theTime.Minute + ":" + theTime.Second;
                if (Board.safeBoardold("Assets/Resources/sevedBoard" + date + time + ".txt"))
                {
                    Debug.Log("sucees fully saved  Assets/Resources/sevedBoard" + time + ".txt the old way");
                }
            }
            else if(Input.GetKeyUp(KeyCode.S))
            {
                
                if (Board.safeBoard("Assets/Resources/newtest.txt"))
                {
                    Debug.Log("sucees fully saved  Assets/Resources/newtest.txt");
                }

            }
            if (Input.GetKeyUp(KeyCode.U))
            {

                Board.makePecesOnBoard();

            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                Board.clearBoardPieces();
            }

            if (Input.GetKeyUp(KeyCode.L) && Input.GetKey(KeyCode.LeftShift))
            {
                Board.loadBoardold();
            }
            else if(Input.GetKeyUp(KeyCode.L))
            {
                Board.loadBoard();
            }
        }
        


    }




    public bool move(int[] startPos, int[] endPos)
    {
        return Board.move(startPos, endPos);
    }
    public void requestedMove(int[] startPos, int[] endPos)
    {
        try
        {
            Board.move(startPos, endPos);
            whiteTurn = !whiteTurn;
        }
        catch (Exception er)
        {
            Debug.Log(er);
            throw;
        }
    }


    public void Cancel()
    {
        canceling = true;
    }

    public void Deselect()
    {
        if (currentPiece != null)
        {
            currentPiece.GetComponentInChildren<MeshRenderer>().material = currentMaterial;
            currentPiece = null;
            
        }
    }
}
