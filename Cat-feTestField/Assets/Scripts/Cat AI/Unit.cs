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

        //What action is happening?
        private bool completingAction;
        private bool isIdle;
        private bool isPurring;

        //Behavior Meters
        private int tiredMeter;
        private int boredMeter;
        private int mischeviousMeter;

        //Movement
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

        private float walkBuffer;

        //Animator
        private Animator catAnimator;

        public GameObject catModel;

        void Start() {
            catAnimator = catModel.GetComponent<Animator>();

            target = start.position;
            PathManager.RequestPath(start.position, target, OnPathFound);

            LevelEditorManager.FurnitureBuilding += ToggleNavigation;

            completingAction = false;
            isIdle = false;
            previousLocation = start.position;
            walkBuffer = 0.1f;

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
                //ClickedLocation();
            }
        }

        void FixedUpdate()
        {
            //Debug.Log(isRotating);
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

        private void Wander()
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

                //Activate Animation
                if(catAnimator != null)
                {
                    Debug.Log("Cat should be walking");
                    catAnimator.SetTrigger("Walk");
                }

                walkBuffer = 0.1f;
            }
            else
            {
                //Checks to see if it stops moving
                if (start.position == previousLocation && walkBuffer <= 0)
                {
                    Debug.Log("Position has been found");
                    completingAction = false;
                    actionDuration = Random.Range(5f, 15f);
                    isIdle = true;
                }

                //Update the previous location with the current one
                walkBuffer = walkBuffer - Time.deltaTime;
                previousLocation = start.position;
            }
        }

        private void Idle()
        {
            //Debug.Log("Is idle");
            if (!completingAction)
            {
                //Activate Cat Animation - Placeholder
                completingAction = true;

                //Debug.Log("Cat should be idling");
                int idleType = Random.Range(1, 4);
                //Debug.Log("Idle Type" + idleType);
                if(idleType == 1 || idleType == 2)
                {
                    catAnimator.SetTrigger("Idle1");
                }
                else if(idleType == 3)
                {
                    catAnimator.SetTrigger("Idle3");
                }
            }
            else
            {


                if (actionDuration <= 0)
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

        private void Purr()
        {
            if (!completingAction)
            {
                completingAction = true;

                //TODO: Start purring animation
            }
            else
            {
                if (actionDuration <= 0)
                {
                    completingAction = false;
                    isPurring = false;
                    //TODO: Stop animating?
                    return;
                }

                //Count down the timer
                actionDuration = actionDuration - Time.deltaTime;
            }
        }

        private void Nap()
        {
            //Debug.Log("Is idle");
            if (!completingAction)
            {
                //Activate Cat Animation - Placeholder
                completingAction = true;

                //TODO: Implement pathfinding for a comfortable spot
                //Check for any comfortable spots
                //If there is, find the position of the nearest spot or comfiest spot
                //If not find a random location

                //MoveTo the specified location
                //Start Nap Animation

               
            }
            else
            {
                if (tiredMeter <= 0)
                {
                    completingAction = false;
                    //isIdle = false;
                    //Stop napping and stop animation

                    return;
                }

                //Count down the timer
                //After certain amount of time 
                actionDuration = actionDuration - Time.deltaTime;
                if(actionDuration < 0)
                {
                    tiredMeter--;
                    //How many seconds till it gets less tired again
                    actionDuration = 15;
                }
            }
        }

        private void PokeMouse()
        {
            //TODO: Implement function

            //Find the raycasted position of the mouse

            //Use the MoveTo function to move to the mouse location
            //If it cannot access this, stop trying
            //If it can, procede

            //Once at the position, start the poke animation
            //Play for random amount of time
            //Meow at player

            //Stop when player moves the mouse
            //Stop after a certain amount of time ~15-30 seconds
        }

        private void PushObject()
        {
            //TODO: Implement push object function
            
            //Find a movable object
            //Check to see if this movable object is ontop of a surface

            //MoveTo function to move to the object

            //Timer to stare at object and wait
            
            //Once ready, start animation to move object

            //Finish action
        }

        private void PickedUp()
        {
            //TODO: Implement state for being picked up

            //Turn model into rag doll
            //Wait till mouse is released

            //Activate falling and then landing animation
            //Return back to normal
        }

        private void ChooseAction()
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