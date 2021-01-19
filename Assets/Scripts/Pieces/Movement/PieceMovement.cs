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

    [SerializeField] private bool white = true;

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
                    if (whiteTurn == white)
                    {
                        if (hit.transform.gameObject.GetComponent<Piece>().white == whiteTurn)
                        {
                            Deselect();
                            currentPiece = hit.transform.gameObject.GetComponent<Piece>();
                            currentMaterial = currentPiece.GetComponentInChildren<MeshRenderer>().material;
                            currentPiece.GetComponentInChildren<MeshRenderer>().material = selectedMaterial;

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


                        int[] startMathPos = Board.getPieceMathPos(currentPiece);
                           
                        int[] endMathPos = { (int)(Mathf.Floor(hit.transform.localPosition.x)), (int)(Mathf.Floor(hit.transform.localPosition.z)),0 };

                        if (move(startMathPos, endMathPos))
                        {
                            network.SendMove(startMathPos, endMathPos);
                            Deselect();
                            whiteTurn = !whiteTurn;
                        }
                        

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
