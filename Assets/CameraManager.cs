using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private float minZoom;
    [SerializeField] private List<float> zoomHeights;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float offsetToMove;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveZoomSpeed;
    [SerializeField] private List<KeyCode> fastZoomKeys;

    private float sideOffset;
    private float useZoom;

    private void Start()
    {
        useZoom = minZoom;
        float ratio = Screen.width / Screen.height;
        sideOffset = ratio * offsetToMove;
    }

    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            useZoom -= zoomSpeed;
            if(useZoom < minZoom)
            {
                useZoom = minZoom;
            }
        } else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            useZoom += zoomSpeed;
        }

        foreach(KeyCode fastZoomKey in fastZoomKeys)
        {
            if(Input.GetKeyDown(fastZoomKey))
            {
                useZoom = zoomHeights[fastZoomKeys.IndexOf(fastZoomKey)];
            }
        }
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, useZoom, transform.position.z), Time.deltaTime * moveZoomSpeed);

        float mouseX = Input.mousePosition.x / Screen.width;
        float mouseY = Input.mousePosition.y / Screen.height;

        float useSpeed = moveSpeed * ((1f / 20f) * transform.position.y + (1f / 2f)); 

        if(mouseX < sideOffset)
        {
            transform.position -= Vector3.right * useSpeed * Time.deltaTime;
        } else if(mouseX > 1-sideOffset)
        {
            transform.position += Vector3.right * useSpeed * Time.deltaTime;
        }
        if (mouseY < offsetToMove)
        {
            transform.position -= Vector3.forward * useSpeed * Time.deltaTime;
        }
        else if (mouseY > 1 - offsetToMove)
        {
            transform.position += Vector3.forward * useSpeed * Time.deltaTime;
        }
    }
}
