using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    private Camera cam;

    private Piece currentPiece;

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
                    currentPiece = hit.transform.gameObject.GetComponent<Piece>();
                }
            }
        } else if(Input.GetMouseButtonUp(0))
        {
            if (currentPiece != null)
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (!hit.transform.gameObject.GetComponent<Piece>())
                    {
                        currentPiece.transform.position = new Vector3(hit.transform.position.x, currentPiece.transform.position.y, hit.transform.position.z);
                        currentPiece = null;
                    } else
                    {
                        // IF ENEMY PIECE
                            // KILL PIECE
                        
                    }
                }
            }
        }
    }
}
