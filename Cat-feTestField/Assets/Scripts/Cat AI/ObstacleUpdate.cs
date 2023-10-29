using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    // private void OnTriggerStay(Collider other)
    // {
    //     // Check if the other collider is the one you want to monitor
    //     Debug.Log("Hello");
    //     if (other.CompareTag("TagOfCollidersOfInterest"))
    //     {
    //         // Handle the change in position or interaction with the colliders of interest
    //         HandleColliderPositionChange();
    //     }
    // }

    // void HandleColliderPositionChange()
    // {
    //     // This function is called when the collider's position changes or interacts with other colliders of interest
    //     Debug.Log("Collider position has changed or is in contact with a collider of interest in 3D!");
    //     // Implement your desired actions or logic here when the collider interacts with the colliders of interest
    // }

    Vector3 previousPosition; // Store the previous position of the collider
    Grid grid;

    

    void Start()
    {
        // Initialize the previous position with the initial position of the collider
        previousPosition = transform.position;
        
    }

    void Update()
    {
        // Check for changes in position
        if (transform.position != previousPosition)
        {
            // If the position has changed, call the function to handle the change
            HandleColliderPositionChange();
            
            // Update the previous position to the current position
            previousPosition = transform.position;
        }
    }

    void HandleColliderPositionChange()
    {
        // This function is called when the collider's position changes
        Debug.Log("Collider position has changed!");
        GameObject objectWithMyScript = GameObject.Find("A*");
        Grid grid = objectWithMyScript.GetComponent<Grid>();
        grid.CreateGrid();
        // Implement your desired actions or logic here when the collider's position changes
    }
}
