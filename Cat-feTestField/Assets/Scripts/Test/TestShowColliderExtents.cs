using System.Collections;
using System.Collections.Generic;
using TsingIGME601;
using UnityEngine;

public class TestShowColliderExtents : MonoBehaviour
{
    BoxCollider _col;
    private void Start()
    {
        _col = GetComponent<BoxCollider>();
    }
    private void Update()
    {
        Utility.DisplayBox
            (
                transform.TransformPoint(_col.center),
                _col.bounds.extents,
                Quaternion.identity
            ) ;
    }
   
}
