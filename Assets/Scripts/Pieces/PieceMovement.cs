using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private bool whiteTurn = true;

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
                    Debug.Log(hit.transform.gameObject.GetComponent<Piece>().white);
                    Debug.Log(whiteTurn);

                    if (hit.transform.gameObject.GetComponent<Piece>().white == whiteTurn)
                    {
                        currentPiece = hit.transform.gameObject.GetComponent<Piece>();
                    }
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
                        whiteTurn = !whiteTurn;
                    }
                    else
                    {
                        Debug.Log(hit.transform.gameObject.GetComponent<Piece>().white);
                        Debug.Log(whiteTurn);
                        if (hit.transform.gameObject.GetComponent<Piece>().white != whiteTurn)
                        {
                            currentPiece.transform.position = new Vector3(hit.transform.position.x, currentPiece.transform.position.y, hit.transform.position.z);
                            currentPiece = null;
                            Destroy(hit.transform.gameObject);
                            whiteTurn = !whiteTurn;
                        }
                    }
                }
            }
        }
    }
}
