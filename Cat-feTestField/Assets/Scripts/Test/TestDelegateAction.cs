using System.Collections;
using System.Collections.Generic;
using TsingIGME601;
using UnityEngine;

public class TestDelegateAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LevelEditorManager.FurnitureUpdated += TestAction;
    }
    private void OnDisable()
    {
        LevelEditorManager.FurnitureUpdated -= TestAction;
    }

    private void TestAction() 
    {
        Debug.Log("Action recieved, the position is: ");
    }
}
