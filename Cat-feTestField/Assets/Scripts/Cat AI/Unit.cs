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

        public static Unit instance;

        public float catHeight;

        //What action is happening?
        public bool completingAction;
        private bool isIdle;
        private bool isPurring;
        private bool isFalling;
        public bool isPickedUp;
        private bool justPickedUp;
        public bool isHeld;

        private bool isWalking;

        private bool isSitting;
        private bool sittingTrigger;

        //Behavior Meters
        private int tiredMeter;
        private int boredMeter;
        private int mischeviousMeter;

        //Movement
        Vector3 targetLocation;
        Vector3 previousLocation;


        //Jumping Behavior
        private bool startJump;
        private bool endJump;

        //Current Grid
        private Grid currentGrid;
        private Grid targetGrid;

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

        private void Awake()
        {
            instance = this;
        }

        void Start() {
            catAnimator = catModel.GetComponent<Animator>();
            justPickedUp = false;

            target = start.position;
            //PathManager.RequestPath(start.position, currentGrid, target, targetGrid, OnPathFound);

            LevelEditorManager.FurnitureBuilding += ToggleNavigation;

            completingAction = false;
            isIdle = false;
            previousLocation = start.position;
            walkBuffer = 0.1f;

            catHeight = 1;
            tiredMeter = Random.Range(6, 15);

            //currentGrid = PathManager.instance.grids[0];
        }
        private void OnDestroy()
        {
            LevelEditorManager.FurnitureBuilding -= ToggleNavigation;
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("Collision Detection");
            if (other.CompareTag("Floor") && !isHeld)
            {
                isFalling = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {

/*            if (other.CompareTag("Floor") && isWalking)
            {
                isFalling = true;
            }*/
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
                    //Debug.Log("Inside Wander");
                    MoveTo(targetLocation);
                }
                else if(!isFalling){
                    MoveTo(target);
                    //transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                    //transform.LookAt(currentWaypoint);
                }
            }
        }

        public void InvestigateObject()
        {
            //Not Implemented
        }

        private void Wander()
        {         
            if (!completingAction)
            {               
                if (isSitting)
                {                  
                    if (!sittingTrigger)
                    {
                        actionDuration = 2.1f;
                        catAnimator.SetTrigger("FinishSitting");
                        sittingTrigger = true;
                    }
                    else
                    {
                        actionDuration = actionDuration - Time.deltaTime;
                    }       
                    if(actionDuration <= 0)
                    {
                        isSitting = false;
                        sittingTrigger = false;
                    }
                }
                else
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
                    StartCoroutine("FollowPath");

                    //Activate Animation
                    catAnimator.SetTrigger("Walk");

                    walkBuffer = 0.1f;
                }
            }
            else
            {
                //Checks to see if it stops moving
                if (start.position == previousLocation && walkBuffer <= 0)
                {
                    //Debug.Log("Position has been found");
                    completingAction = false;
                    actionDuration = Random.Range(5f, 15f);
                    isIdle = true;
                    tiredMeter--;
                    isWalking = false;
                }

                //Update the previous location with the current one
                walkBuffer = walkBuffer - Time.deltaTime;
                previousLocation = start.position;
            }
        }

        private void Idle()
        {
            if (!completingAction)
            {
                //Activate Cat Animation - Placeholder
                completingAction = true;

                int idleType = Random.Range(1, 5);
                if (idleType == 1)
                {
                    catAnimator.SetTrigger("Idle1");
                    actionDuration = Random.Range(4, 16);
                }
                if (idleType == 2)
                {
                    catAnimator.SetTrigger("Idle2");
                    actionDuration = Random.Range(5, 16);
                }
                else if(idleType == 3)
                {
                    catAnimator.SetTrigger("Idle3");
                    actionDuration = 5;
                }
                else if (idleType == 4)
                {
                    catAnimator.SetTrigger("Idle4");
                    isSitting = true;
                    actionDuration = Random.Range(5, 15);
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
            if (!completingAction)
            {
                //Activate Cat Animation - Placeholder
                completingAction = true;

                //TODO: Implement pathfinding for a comfortable spot
                //Check for any comfortable spots
                //If there is, find the position of the nearest spot or comfiest spot
                //If not find a random location

                //MoveTo the specified location
                //Start Nap Animation and duration
                catAnimator.SetTrigger("Nap");
                actionDuration = Random.Range(20,45);
            }
            else
            {
                //Count down the timer
                //After certain amount of time it will regain its energy
                actionDuration = actionDuration - Time.deltaTime;
                if(actionDuration < 0)
                {
                    tiredMeter = Random.Range(6, 15);

                    completingAction = false;
                    //Stop napping and stop animation
                    return;
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
            //Turn model into rag doll??
            //StopCoroutine("FollowPath");
            //Wait till mouse is released
            if (!justPickedUp)
            {
                isHeld = true;
                //this.GetComponent<Rigidbody>().isKinematic = false;            
                StopCoroutine("FollowPath");
                path = null;
                catAnimator.SetTrigger("Falling");
                //Activate falling 
                isFalling = true;
                isHeld = true;
                completingAction = true;
                justPickedUp = true;
            }

            //Falling animation
            if (isFalling && !isHeld)
            {
                Debug.Log("This falling method is running");
                //start.position = new Vector3(0, 1, 0) * Time.deltaTime;
                transform.Translate(Vector3.down * 10 * Time.deltaTime);

                previousLocation = start.position;

/*                if (transform.position.y < 3 && !isHeld)
                {
                    Debug.Log("This is running 3");
                    catAnimator.SetTrigger("Landing");
                }*/
            }
            else if (!isHeld)
            {
                //Return back to normal
                isFalling = false;
                completingAction = false;
                justPickedUp = false;
                isPickedUp = false;
                canNavigate = true;
                isIdle = true;            
                if (transform.position.y > 3)
                {
                    catAnimator.SetTrigger("Landing");
                }                  
            }
        }

        private void ChooseAction()
        {
            if (isPickedUp)
            {
                PickedUp();
            }
            else if(tiredMeter <= 0)
            {
                Nap();
            }
            else if (isIdle)
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
/*          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                target = hit.point;
                //MoveTo(target);
                CatRotation(target);
            }*/
        }

        public void Jump()
        {
            //Choose some grid
            //From here choose a spot on this grid and return the node and grid index

            //Request path from here
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
            PathManager.RequestPath(start.position, currentGrid, goalLocation, targetGrid, OnPathFound); // Recalculate the path to the new target                    
        }

        public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
        {
            if (pathSuccessful)
            {
                path = newPath;
                targetIndex = 0;
                if (transform.position.y > 2)
                {
                    Debug.Log("On path found is runing");
                    StopCoroutine("FollowPath");                 
                    StartCoroutine("FollowPath");
                }
                else
                {
                    StopCoroutine("FollowPath");
                    StartCoroutine("FollowPath");
                }

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

                while (Vector3.Distance(transform.position, currentWaypoint) > waypointProximity && !isPickedUp)
                {
                    if (!isFalling)
                    {
                        //CatRotation(currentWaypoint);
                        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
                        transform.LookAt(currentWaypoint);
                        yield return null;
                    }
                }
            }
            //isWalking = false;
        }

        IEnumerator FollowJumpPath()
        {
            Debug.Log("Jump path is running");
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

                //Debug.Log("Should be walking now");
                while (Vector3.Distance(transform.position, currentWaypoint) > waypointProximity && !isPickedUp)
                {
                    if (isFalling)
                    {
                        Vector3 tempCurrentWaypoint = currentWaypoint;
                        tempCurrentWaypoint.y = 1;
                        //start.position = new Vector3(0, 1, 0) * Time.deltaTime;
                        //transform.position = Vector3.MoveTowards(transform.position, tempCurrentWaypoint, 10 * Time.deltaTime);
                        transform.Translate(Vector3.down * 10 * Time.deltaTime);


                        previousLocation = start.position;
                        //catAnimator.SetTrigger("Falling");
                        if(transform.position.y < 1.1f)
                        {
                            isFalling = false;
                        }
                    }
                    else if(!isFalling)
                    {
                        //CatRotation(currentWaypoint);
                        Vector3 tempCurrentWaypoint = currentWaypoint;
                        if(currentWaypoint.y != previousLocation.y)
                        {
                            tempCurrentWaypoint.y = previousLocation.y;
                        }
                        transform.position = Vector3.MoveTowards(transform.position, tempCurrentWaypoint, speed * Time.deltaTime);                  
                        previousLocation = start.position;
                        transform.LookAt(currentWaypoint);
                        yield return null;
                    }

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