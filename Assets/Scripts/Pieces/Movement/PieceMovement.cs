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

    private void Start()
    {
        cam = GetComponent<Camera>();
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
                    if (hit.transform.gameObject.GetComponent<Piece>().white == whiteTurn)
                    {
                        Deselect();
                        currentPiece = hit.transform.gameObject.GetComponent<Piece>();
                        currentMaterial = currentPiece.GetComponentInChildren<MeshRenderer>().material;
                        currentPiece.GetComponentInChildren<MeshRenderer>().material = selectedMaterial;
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
                        if (!hit.transform.gameObject.GetComponent<Piece>())
                        {
                            currentPiece.transform.position = new Vector3(hit.transform.position.x, currentPiece.transform.position.y, hit.transform.position.z);
                            Deselect();
                            whiteTurn = !whiteTurn;
                        }
                        else
                        {
                            if (hit.transform.gameObject.GetComponent<Piece>().white != whiteTurn)
                            {
                                currentPiece.transform.position = new Vector3(hit.transform.position.x, currentPiece.transform.position.y, hit.transform.position.z);
                                Deselect();
                                Destroy(hit.transform.gameObject);
                                whiteTurn = !whiteTurn;
                            }
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
