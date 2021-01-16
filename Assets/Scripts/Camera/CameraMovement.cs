using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private float ySpeed;
    [SerializeField] private float xSpeed;

    [SerializeField] private float zoomSpeed;
    [SerializeField] private float maxZoom;

    private bool rotating = false;

    private Vector3 previousRotation;

    private void Start()
    {
        cam = GetComponent<Camera>();
        previousRotation = transform.eulerAngles;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit))
            {
                rotating = true;
            } else
            {
                if(!hit.transform.gameObject.GetComponent<Piece>())
                {
                    if ((transform.eulerAngles - previousRotation).magnitude > 1)
                    {
                        GetComponent<PieceMovement>().Deselect();
                    }
                    rotating = true;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            rotating = false;
            previousRotation = transform.eulerAngles;
        }
        if (transform.localPosition.z + Input.mouseScrollDelta.y * zoomSpeed * zoomSpeed <= maxZoom)
        {
            transform.localPosition += new Vector3(0, 0, Input.mouseScrollDelta.y * zoomSpeed);
        }
        if (rotating)
        {
            if (transform.parent.eulerAngles.x - Input.GetAxis("Mouse Y") * xSpeed > 271f)
            {
                transform.parent.rotation = Quaternion.Euler(transform.parent.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y") * xSpeed, 0, 0));
            }
            else if (transform.parent.eulerAngles.x - Input.GetAxis("Mouse Y") * xSpeed < 89f)
            {
                transform.parent.rotation = Quaternion.Euler(transform.parent.eulerAngles + new Vector3(-Input.GetAxis("Mouse Y") * xSpeed, 0, 0));
            }

            transform.parent.rotation = Quaternion.Euler(transform.parent.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X") * ySpeed, 0));
        }
    }

    private void FixedUpdate()
    {
        if((transform.eulerAngles - previousRotation).magnitude > 1)
        {
            GetComponent<PieceMovement>().Cancel();
        }
    }
}
