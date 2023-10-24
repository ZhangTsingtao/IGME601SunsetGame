using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShowColliderExtents : MonoBehaviour
{
    Collider _col;
    private void Start()
    {
        _col = GetComponent<Collider>();
    }
    private void Update()
    {
        Debug.Log("extents : " + _col.bounds.extents);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.TransformPoint(Vector3.up * _col.bounds.extents.y / transform.localScale.y), _col.bounds.extents * 2);
        //Vector3.up * _col.bounds.extents.y/transform.localScale.y
    }
}
