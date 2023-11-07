using System.Collections;
using System.Collections.Generic;
using TsingIGME601;
using UnityEngine;

namespace RoosaIGM601
{
    public class Unit : MonoBehaviour
    {
        
        //private Transform target;
        public Transform start;
        private Vector3 target; 
        float speed = 5;
        Vector3[] path;
        int targetIndex;

        public float catHeight;

        bool completingAction;
        Vector3 targetLocation;
        Vector3 previousLocation;

        private float rotSpeed = 30f;
        //private float targetRadius = 0.5f;

        //communicate with leveleditor
        private bool canNavigate = true;

        void Start() {
            target = start.position;
            PathManager.RequestPath(start.position,target, OnPathFound);

            LevelEditorManager.FurnitureBuilding += ToggleNavigation;

            completingAction = false;
            previousLocation = start.position;

            catHeight = 1;
        }
        private void OnDestroy()
        {
            LevelEditorManager.FurnitureBuilding -= ToggleNavigation;
        }

        private void ToggleNavigation(bool isBuilding)
        {
            canNavigate = !isBuilding;
        }

        private void Update()
        {
            Wander();
            // Check for user input to set a new target position
            // if (Input.GetMouseButtonDown(0) && canNavigate)
            // {
            //     ClickedLocation();
            //     //CatRotation();
            // }
        }

        public void InvestigateObject()
        {

        }

        public void Wander()
        {
            if (!completingAction)
            {
                //Debug.Log("Finding new location");
                //Find new random location to wander to
                targetLocation = new Vector3(Random.Range(-4.0f, 4.0f), 0f, Random.Range(-4.0f, 4.0f));

                //Speed of how fast the cat will move to this location
                speed = Random.Range(1f, 3f);

                //Debug.Log(targetLocation);

                //Move to new location and set completing action to true
                MoveTo(targetLocation);
                completingAction = true;
            }
            else
            {
                //Checks to see if it stops moving
                if(start.position == previousLocation)
                {
                    completingAction = false;
                }

                //Update the previous location with the current one
                previousLocation = start.position;
            }
        }

        public void ClickedLocation()
        {
            //Debug.Log("Roosa");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            //Debug.Log("Ray origin: " + ray.origin);
            //Debug.Log("Ray direction: " + ray.direction);

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log("Hello Roosa");
                target = hit.point;
                //Debug.Log("New target position: " + target);
                //CatRotation(target);
                MoveTo(target);
                
                //PathManager.RequestPath(start.position, target, OnPathFound); // Recalculate the path to the new target
            }
        }

        public void CatRotation(Vector3 targetPos){
        // //     // Vector3 dir = target- transform.position;
        // //     // Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        // //     // rot.y = 0;
        // //     // rot.z = 0;
        // //     // transform.rotation = rot;

        // //     // float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed;
        // //     // transform.Rotate(new Vector3(0, 0,mouseY));

        // //     Vector3 direction = target.position - transform.position;
        // //     float distance = direction.magnitude;
        // //     Vector2 velocity = direction.normalized * (distance / targetRadius);
        // //     float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        // //     transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        //         Debug.Log("Hello Cat");
        //         Vector3 direction = targetPos - transform.position;
        //         Quaternion rotation = Quaternion.LookRotation(direction);

        //         // Smoothly rotate towards the direction
        //         //float rotationSpeed = 5f; // Adjust the rotation speed as needed
        //         transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
            Vector3 direction = targetPos - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
       
        }

        private void MoveTo(Vector3 goalLocation)
        {

            PathManager.RequestPath(start.position, goalLocation, OnPathFound); // Recalculate the path to the new target
        }

        public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
            if (pathSuccessful) {
                path = newPath;
                targetIndex = 0;
                //CatRotation();
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        // IEnumerator FollowPath() {
        // 	// Vector3 currentWaypoint = path[0];
        // 	// while (true) {
        // 	// 	if (transform.position == currentWaypoint) {
        // 	// 		targetIndex ++;
        // 	// 		if (targetIndex >= path.Length) {
        // 	// 			yield break;
        // 	// 		}
        // 	// 		currentWaypoint = path[targetIndex];
        // 	// 	}

        // 	// 	transform.position = Vector3.MoveTowards(transform.position,currentWaypoint,speed * Time.deltaTime);
        // 	// 	yield return null;

        // 	// }

        //     Vector3 currentWaypoint = path[0];
        //     float waypointProximity = 0.1f; // Adjust as needed
        //     while (true) {
        //         if (Vector3.Distance(transform.position, currentWaypoint) < waypointProximity) {
        //             targetIndex++;
        //             if (targetIndex >= path.Length) {
        //                 yield break;
        //             }
        //             currentWaypoint = path[targetIndex];
        //         }

        //         transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
        //         yield return null;
        //     }
        // }

        IEnumerator FollowPath() {
        if (path == null || path.Length == 0) {
            yield break; // No path to follow, exit the coroutine
        }

        Vector3 currentWaypoint = path[0];
        float waypointProximity = 0.1f; // Adjust as needed
        int pathLength = path.Length;

        for (int i = 0; i < pathLength; i++) {
            currentWaypoint = path[i];
            currentWaypoint.y = catHeight;

            while (Vector3.Distance(transform.position, currentWaypoint) > waypointProximity) {
                transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                    transform.LookAt(currentWaypoint);
                yield return null;
            }
        }
    }

        public void OnDrawGizmos() {
            if (path != null) {
                for (int i = targetIndex; i < path.Length; i ++) {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex) {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else {
                        Gizmos.DrawLine(path[i-1],path[i]);
                    }
                }
            }
        }
    }
}