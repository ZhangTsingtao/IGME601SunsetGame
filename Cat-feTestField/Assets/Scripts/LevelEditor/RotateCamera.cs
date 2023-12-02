using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{

    public Camera camera;

    public float rotateSpeedModifier = 0.1f;

    private bool isRotating = false;

    private float startMousePositionX, startMousePositionY;

    private Transform rotatePoint;
    public Vector3 initialCamPos;
    public Vector3 camPos = Vector3.zero;
    
    private void Start()
    {
        camPos = camera.transform.position;
        rotatePoint = transform;
        initialCamPos = camera.transform.position - rotatePoint.position;

        startMousePositionX = 0;
        startMousePositionY = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            isRotating = true;
            startMousePositionX = Input.mousePosition.x;
            startMousePositionY = Input.mousePosition.y;
        }
        else if (Input.GetMouseButtonUp(2))
        {
            isRotating = false;
            startMousePositionX = 0;
            startMousePositionY = 0;
        }

        if (isRotating)
        {
            float currentMousePositionX = Input.mousePosition.x;
            float currentMousePositionY = Input.mousePosition.y;

            float mouseMovementX = currentMousePositionX - startMousePositionX;
            float mouseMovementY = currentMousePositionY - startMousePositionY;

            startMousePositionX = currentMousePositionX;
            startMousePositionY = currentMousePositionY;

            rotatePoint.rotation = Quaternion.AngleAxis(mouseMovementX * rotateSpeedModifier, transform.up) * rotatePoint.rotation;
            rotatePoint.rotation = Quaternion.AngleAxis(-mouseMovementY * rotateSpeedModifier, transform.right) * rotatePoint.rotation;
            camPos = Quaternion.AngleAxis(mouseMovementX * rotateSpeedModifier, rotatePoint.transform.up) * camPos;
            camPos = Quaternion.AngleAxis(-mouseMovementY * rotateSpeedModifier, rotatePoint.transform.right) * camPos;
            camera.transform.position = rotatePoint.position + camPos;

            camera.transform.LookAt(rotatePoint);
        }
    }
    public void ResetRotation()
    {
        camera.transform.position = rotatePoint.position + initialCamPos;
        camera.transform.LookAt(rotatePoint);
        camPos = initialCamPos;
    }
}
