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


        if (Input.GetKeyUp(KeyCode.S) && Debuging)
        {
            System.DateTime theTime = System.DateTime.Now;
            string date = theTime.Year + "-" + theTime.Month + "-" + theTime.Day;
            string time = date + "T" + theTime.Hour + ":" + theTime.Minute + ":" + theTime.Second;
            if (Board.safeBoard("Assets/Resources/sevedBoard" + date + time + ".txt"))
            {
                Debug.Log("sucees fully saved  Assets/Resources/sevedBoard" + time + ".txt");
            }
        }
        if (Input.GetKeyUp(KeyCode.U) && Debuging)
        {

            Board.makePecesOnBoard();
            
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
