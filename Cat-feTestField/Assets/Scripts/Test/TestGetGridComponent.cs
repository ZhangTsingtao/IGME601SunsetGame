using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGetGridComponent : MonoBehaviour
{
    public GameObject gridObject;
    [SerializeField] private GridLayout grid;
    [SerializeField] private BoxCollider boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        grid = gridObject.GetComponent<GridLayout>();
        boxCollider = gridObject.GetComponent<BoxCollider>();
    }

}
