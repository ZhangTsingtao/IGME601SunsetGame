using System.Collections;
using System.Collections.Generic;
using TsingIGME601;
using UnityEngine;

public class TestDelegateAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LevelEditorManager.NewFurnitureAdded += TestAction;
    }
    private void OnDisable()
    {
        LevelEditorManager.NewFurnitureAdded -= TestAction;
    }

    private void TestAction(Vector3 position) 
    {
        Debug.Log("Action recieved, the position is: " + position);
    }
}
