using System.Collections;
using System.Collections.Generic;
using TsingIGME601;
using UnityEngine;

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

    //communicate with leveleditor
    private bool canNavigate = true;

	void Start() {
        target = start.position;
		PathManager.RequestPath(start.position,target, OnPathFound);

        LevelEditorManager.FurnitureUnderConstruction += ToggleNavigation;

        completingAction = false;
        previousLocation = start.position;

        catHeight = 1;
    }
    private void OnDestroy()
    {
        LevelEditorManager.FurnitureUnderConstruction -= ToggleNavigation;
    }

    private void ToggleNavigation(bool isBuilding)
    {
        canNavigate = !isBuilding;
    }

    private void Update()
    {
        //Wander();
        // Check for user input to set a new target position
        if (Input.GetMouseButtonDown(0) && canNavigate)
        {
            ClickedLocation();
        }
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
            MoveTo(target);
            //PathManager.RequestPath(start.position, target, OnPathFound); // Recalculate the path to the new target
        }
    }

    private void MoveTo(Vector3 goalLocation)
    {

        PathManager.RequestPath(start.position, goalLocation, OnPathFound); // Recalculate the path to the new target
    }

	public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (pathSuccessful) {
			path = newPath;
			targetIndex = 0;
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