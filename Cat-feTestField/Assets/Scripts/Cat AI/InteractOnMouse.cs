using System.Collections;
using System.Collections.Generic;
using TsingIGME601;
using UnityEngine;

public class InteractOnMouse : MonoBehaviour
{
    public GameObject plane;
    public float maxDragDistance = 20;
    public bool mouseIsHovering;
    private void Start()
    {
        plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = 10 * Vector3.one;
        plane.GetComponent<MeshRenderer>().enabled = false;
        plane.SetActive(false);
        plane.gameObject.layer = LayerMask.NameToLayer("Drag");
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseIsHovering)
        {
            GetComponent<RoosaIGM601.Unit>().completingAction = false;

            GetComponent<RoosaIGM601.Unit>().isPickedUp = true;
            GetComponent<RoosaIGM601.Unit>().isHeld = true;
        }
        else if(Input.GetMouseButtonUp(0) && mouseIsHovering)
        {
            GetComponent<RoosaIGM601.Unit>().isHeld = false;
        }
    }
    private void OnMouseEnter()
    {
        SetPlanePosRot();
        mouseIsHovering = true;
    }
    private void OnMouseDrag()
    {
        int layer_mask = LayerMask.GetMask("Drag");
        layer_mask += LayerMask.GetMask("Build Surface");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer_mask);
        transform.position = hit.point;    
    }
    private void OnMouseExit()
    {
        plane.SetActive(false);
        mouseIsHovering = false;
    }
    private void SetPlanePosRot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity);
        plane.transform.position = hit.point;
        plane.transform.up = -ray.direction;

        plane.SetActive(true);
    }
    private void LateUpdate()
    {
        if(transform.position.magnitude > maxDragDistance)
        {
            transform.position = Vector3.one;
            Debug.Log("You got me too far");
        }
    }

}
