using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateScript : MonoBehaviour
{
    public bool canScroll = true;

    private float x;
    private float y;
    public float sensitivity = -1f;
    private Vector3 rotate;

    private Vector3 initialCamPos;
    private Camera cam;
    private float scrollSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        initialCamPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(2))
        {
            RotateCamera();
        }
        if (cam.orthographic && canScroll)
        {
            cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        }
    }
    private void RotateCamera()
    {
        y = Input.GetAxis("Mouse X");
        x = Input.GetAxis("Mouse Y");
        rotate = new Vector3(x, y * sensitivity, 0);
        transform.eulerAngles = transform.eulerAngles - rotate;
    }
}