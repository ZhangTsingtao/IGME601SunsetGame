using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatRotation : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = 5f;
    private Vector3 target;

    private float rotSpeed = 10f;
    bool isRotating = false;
    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                    target = hit.point;
                    Vector3 directionToClick = target - transform.position;
                    directionToClick.y=0;
                    directionToClick.Normalize();
                    Quaternion toRotation = Quaternion.LookRotation(directionToClick, Vector3.up);
                    StartCoroutine(RotateAndMove(toRotation));
                    
            }

        }

        IEnumerator RotateAndMove(Quaternion toRotation)
        {
            isRotating = true;
            float rotationTime = 0f;

            while (!transform.rotation.Equals(toRotation))
            {
                rotationTime += rotSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationTime);
                Debug.Log("Rotated");
                Debug.Log("Current Rotation" + transform.rotation);
                Debug.Log("To Rotate" + toRotation);
                yield return null;
            }

            // Rotation complete, now move towards the target position
            isRotating = false;
        }

        
        // float horizontalInput = Input.GetAxis("Horizontal");
        // float verticalInput = Input.GetAxis("Vertical");
         

        // Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        // movementDirection.Normalize();
        // //transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);
        // transform.position = Vector3.MoveTowards(transform.position, target, speed* Time.deltaTime);

        // if(movementDirection != Vector3.zero){
        //     Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        //     transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotSpeed*Time.deltaTime);
        // }
    }

    void FixedUpdate()
        {
            Debug.Log("Can you move");
            if (!isRotating)
            {
                // Move towards the target position
                Debug.Log("Can you move");
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
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
        // Vector3 dir = targetPos- transform.position;
        // Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        // rot.y = 0;
        // rot.z = 0;
        // transform.rotation = rot;
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
