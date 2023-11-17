using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatRotation : MonoBehaviour
{
    // Start is called before the first frame update
    private float rotSpeed = 0.1f;
    private Vector3 target; 
    //private float targetRadius = 0.5f;
    Quaternion rotGoal;
    Vector3 direction;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Debug.Log("Ray origin: " + ray.origin);
            //Debug.Log("Ray direction: " + ray.direction);

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log("Hello Roosa");
                target = hit.point;
                //Debug.Log("New target position: " + target);
                CatRot(target);
                //MoveTo(target);
                
                //PathManager.RequestPath(start.position, target, OnPathFound); // Recalculate the path to the new target
            }
        }
    }

    public void CatRot(Vector3 targetPos)
    {
        // Vector3 direction = targetPos - transform.position;
        // Quaternion rotation = Quaternion.LookRotation(direction);

        // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
        ///////////////
        //transform.LookAt(targetPos);
        //////////////////
        ///
        Vector3 dir = targetPos- transform.position;
        Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        rot.y = 0;
        rot.z = 0;
        transform.rotation = rot;
        ///////////////////////
        // float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed;
        // transform.Rotate(new Vector3(0, 0,mouseY));
        ///////////////////////
        // Vector3 direction = targetPos - transform.position;
        // float distance = direction.magnitude;
        // Vector2 velocity = direction.normalized * (distance / targetRadius);
        // float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        /////////////////
        // direction = (target - transform.position).normalized;
        // rotGoal = Quaternion.LookRotation(direction);
        // transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, rotSpeed);


    }
}
