using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
namespace TsingIGME601
{
    [System.Serializable]
    public class AbleToBuiltOn : MonoBehaviour
    {
        //This is just a mark
        //attach this script to the furniture you want to be able to be built upon.
        [SerializeField]
        Vector2 gridWorldSize;
        float nodeRadius;
        RoosaIGM601.NextNode[,] grid;
        int gridSizeX, gridSizeY;
        float nodeDiameter;
        LayerMask unwalkableMask;
        BoxCollider boxCollider;

        //[SerializeField] private List<NextNode> nodes;
        private void Start()
        {
            boxCollider = GetComponent<BoxCollider>();
            gridWorldSize.x = boxCollider.bounds.extents.x * 2;
            gridWorldSize.y = boxCollider.bounds.extents.z * 2;
            nodeRadius = 0.1f;
            nodeDiameter = nodeRadius * 2;

            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            Debug.Log(gridSizeX + ", " + gridSizeY);
            
            unwalkableMask = LayerMask.NameToLayer("Unwalkable");

            if (!GetComponent<PreviewFollowMouse>())
            {
                CreateGrid();

                ArrangeBuildGrid();
            }
        }
        public void CreateGrid()
        {
            int count = 0;
            Debug.Log("Hello from Create Grid");
            grid = new RoosaIGM601.NextNode[gridSizeX, gridSizeY];
            Vector3 worldBottomLeft = boxCollider.bounds.center - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
            worldBottomLeft += Vector3.up * (boxCollider.bounds.extents.y + 1 - nodeRadius);

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    //bool openPath = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                    //Debug.Log(openPath);
                    grid[x, y] = new RoosaIGM601.NextNode(true, worldPoint, x, y);
                    Debug.Log(grid[x,y] + "    " + count++);
                }
            }
        }
        void OnDrawGizmos()
        {
            
            Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

            if (grid != null)
            {
                //Vector3 boundsSize = boxCollider.bounds.size;
                //Vector3 boundsMax = boxCollider.bounds.max;
                //Vector3 boundsExtents = boxCollider.bounds.extents;
                //Debug.Log(boundsSize + "     " + boundsMax + "     " + boundsExtents + "     " + boxCollider.bounds.center);


                //NextNode playerNode = NodeFromWorldPoint(player.position);

                foreach (RoosaIGM601.NextNode n in grid)
                {
                    //Debug.Log("Draw");
                    //Debug.Log(n.openPath);
                    Gizmos.color = Color.yellow;
                    Gizmos.color = (n.openPath) ? Color.yellow : Color.red;
                    //if (playerNode == n)
                    //{
                    //    Gizmos.color = Color.cyan;
                    //}
                    //if (path != null)
                    //{
                    //    if (path.Contains(n))
                    //    {
                    //        Gizmos.color = Color.black;
                    //    }
                    //}
                    Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
                    
                }
            }
        }

        void ArrangeBuildGrid()
        {
            GameObject gridObject = new GameObject("GridChild");
            Grid grid = gridObject.AddComponent<Grid>();

            gridObject.transform.position = boxCollider.bounds.center + Vector3.up * (boxCollider.bounds.extents.y);
            gridObject.transform.parent = transform;

            
        }
    }

}
