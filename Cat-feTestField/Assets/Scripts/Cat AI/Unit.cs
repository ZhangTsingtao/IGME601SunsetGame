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
        float speed = 5f;
        Vector3[] path;
        int targetIndex;

        public float catHeight;

        private bool completingAction;
        private bool isIdle;
        Vector3 targetLocation;
        Vector3 previousLocation;

        private float rotSpeed = 10f;
        bool isRotating = true;
        private string callFrom;
        private Vector3 currentWaypoint;
        //private float targetRadius = 0.5f;
        

        //communicate with leveleditor
        private bool canNavigate = true;

        private float actionDuration;

        void Start()
        {

            target = start.position;
            PathManager.RequestPath(start.position, target, OnPathFound);

            LevelEditorManager.FurnitureBuilding += ToggleNavigation;

            completingAction = false;
            isIdle = false;
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
            //Wander();
            ChooseAction();
            // Check for user input to set a new target position
            if (Input.GetMouseButtonDown(0) && canNavigate)
            {
                ClickedLocation();
            }
        }

        void FixedUpdate()
        {
            Debug.Log(isRotating);
            if (!isRotating)
            {
                if(callFrom == "Wandering"){
                    Debug.Log("Inside Wander");
                    MoveTo(targetLocation);
                }
                else{
                    MoveTo(target);
                    //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                    //transform.LookAt(currentWaypoint);
                }
            }
        }

        public void InvestigateObject()
        {

        }

        public void Wander()
        {
            //Debug.Log("Is wandering");
            if (!completingAction)
            {
                //Find new random location to wander to
                targetLocation = new Vector3(Random.Range(-4.0f, 4.0f), 0f, Random.Range(-4.0f, 4.0f));

                //Speed of how fast the cat will move to this location
                speed = Random.Range(1f, 3f);

                //Move to new location and set completing action to true
                MoveTo(targetLocation);
                callFrom = "Wandering";
                //CatRotation(targetLocation);
                completingAction = true;
            }
            else
            {
                //Checks to see if it stops moving
                if (start.position == previousLocation)
                {
                    completingAction = false;
                    actionDuration = Random.Range(5f, 15f);
                    isIdle = true;
              
                }

                //Update the previous location with the current one
                previousLocation = start.position;
            }
        }

        public void ClickedLocation()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                target = hit.point;
                //MoveTo(target);
                CatRotation(target);
            }
        }

        public void Idle()
        {
            //Debug.Log("Is idle");
            if (!completingAction)
            {
                //Activate Cat Animation - Placeholder
                completingAction = true;
            }
            else
            {


                if(actionDuration <= 0)
                {
                    completingAction = false;
                    isIdle = false;
                    //Stop animating?
                    return;
                }

                //Count down the timer
                actionDuration = actionDuration - Time.deltaTime;
            }
        }

        public void ChooseAction()
        {
            //Debug.Log("Is idle" + isIdle);
            if (isIdle)
            {
                Idle();
            }
            else
            {
                Wander();
            }
        }

        public void CatRotation(Vector3 targetPos)
        {
            Vector3 directionToClick = targetPos - transform.position;
            directionToClick.y=0;
            directionToClick.Normalize();
            Quaternion toRotation = Quaternion.LookRotation(directionToClick, Vector3.up);
            //transform.LookAt(targetPos);
            StartCoroutine(RotateAndMove(toRotation));
        }

        IEnumerator RotateAndMove(Quaternion toRotation)
        {
            isRotating = true;
            float rotationTime = 0f;

            while (!transform.rotation.Equals(toRotation))
            {
                
                rotationTime += rotSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationTime);
                
                yield return null;
            }

            // Rotation complete, now move towards the target position
            isRotating = false;
        }

        private void MoveTo(Vector3 goalLocation)
        {

            PathManager.RequestPath(start.position, goalLocation, OnPathFound); // Recalculate the path to the new target
        }

        public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                path = newPath;
                targetIndex = 0;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        IEnumerator FollowPath()
        {
            if (path == null || path.Length == 0) 
            {
                yield break; // No path to follow, exit the coroutine
            }

            currentWaypoint = path[0];
            float waypointProximity = 0.1f; // Adjust as needed
            int pathLength = path.Length;

            for (int i = 0; i < pathLength; i++) 
            {
                currentWaypoint = path[i];
                currentWaypoint.y = catHeight;

                while (Vector3.Distance(transform.position, currentWaypoint) > waypointProximity) 
                {
                    //CatRotation(currentWaypoint);
                    transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                    transform.LookAt(currentWaypoint);
                    yield return null;
                }
            }
        }

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                    {
                        Gizmos.DrawLine(path[i - 1], path[i]);
                    }
                }
            }
        }
    }
}