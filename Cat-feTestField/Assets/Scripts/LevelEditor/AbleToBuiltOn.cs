using System;
using System.Collections;
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
            //gridWorldSize.x = boxCollider.bounds.extents.x * 2;
            //gridWorldSize.y = boxCollider.bounds.extents.z * 2;
            //nodeRadius = 0.1f;
            //nodeDiameter = nodeRadius * 2;

            //gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            //gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            //Debug.Log(gridSizeX + ", " + gridSizeY);
            
            //unwalkableMask = LayerMask.NameToLayer("Unwalkable");

            if (!GetComponent<PreviewFollowMouse>())
            {
                //CreateGrid();
                //GetNavigationGrid();
                transform.AddComponent<RoosaIGM601.Grid>();
                ArrangeBuildGrid();
            }
        }

        public void GetNavigationGrid()
        {
            RoosaIGM601.Grid grid = transform.AddComponent<RoosaIGM601.Grid>();
            grid.gridWorldSize = gridWorldSize;
            grid.nodeRadius = nodeRadius;
            grid.unwalkableMask = LayerMask.GetMask("Unwalkable");
            grid.player = GameObject.Find("CatCapsule (1)").transform;
            

            Vector3 worldBottomLeft = boxCollider.bounds.center - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
            worldBottomLeft += Vector3.up * (boxCollider.bounds.extents.y + 1 - nodeRadius);

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    //bool openPath = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                    //Debug.Log(openPath);
                    grid.grid[x, y] = new RoosaIGM601.NextNode(true, worldPoint, x, y);
                    Debug.Log(grid.grid[x, y]);
                }
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
       

        void ArrangeBuildGrid()
        {
            GameObject gridObject = new GameObject("GridChild");
            UnityEngine.Grid grid = gridObject.AddComponent<UnityEngine.Grid>();

            gridObject.transform.position = boxCollider.bounds.center + Vector3.up * (boxCollider.bounds.extents.y);
            gridObject.transform.parent = transform;     
        }
    }

}
