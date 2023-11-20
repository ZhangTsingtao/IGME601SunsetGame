
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoosaIGM601
{
	public class Grid : MonoBehaviour
	{
		public Transform player;
		public LayerMask unwalkableMask;
		public Vector2 gridWorldSize;
		public float nodeRadius;
		public NextNode[,] grid;

		float nodeDiameter;
		int gridSizeX, gridSizeY;

		//Tsingtao Grid Setup
		bool isFurniture = false;

		void Awake() 
		{
			isFurniture = false;
            nodeDiameter = nodeRadius*2;
			gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
			gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);

            //Tsingtao Grid Setup
            FurnitureGridSetup();

            CreateGrid();
		}
		public void FurnitureGridSetup()
		{
            //Tsingtao Grid Setup
            BoxCollider boxCollider = GetComponent<BoxCollider>();
			if (boxCollider == null) return;

			isFurniture = true;
            gridWorldSize.x = boxCollider.bounds.extents.x * 2;
            gridWorldSize.y = boxCollider.bounds.extents.z * 2;
            nodeRadius = 0.1f;
            nodeDiameter = nodeRadius * 2;

            gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
            gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
            Debug.Log(gridSizeX + ", " + gridSizeY);

            unwalkableMask = LayerMask.GetMask("Unwalkable");
            player = GameObject.Find("CatCapsule (1)").transform;
            //Tsingtao Grid Setup End
        }
        public void CreateGrid() {
			Debug.Log("Hello from Create Grid");
			grid = new NextNode[gridSizeX,gridSizeY];

			Vector3 worldBottomLeft = Vector3.zero;

            if (isFurniture)
			{
                BoxCollider boxCollider = GetComponent<BoxCollider>();
                worldBottomLeft = boxCollider.bounds.center - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
                worldBottomLeft += Vector3.up * (boxCollider.bounds.extents.y + 1 - nodeRadius);
            }
			else
			{
                worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2 + (new Vector3(0, 1.5f - nodeDiameter / 2, 0));
            }

            //worldBottomLeft += new Vector3(0, ( gridWorldSize.y / 2), 0);

            for (int x = 0; x < gridSizeX; x ++) {
				for (int y = 0; y < gridSizeY; y ++) {
					Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
					bool openPath = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
					//Debug.Log(openPath);
					grid[x,y] = new NextNode(openPath,worldPoint, x, y);
				}
			}
		}

		public List<NextNode> GetNeighbours(NextNode node){
		List<NextNode> neighbour = new List<NextNode>();
		for (int x=-1; x<=1; x++){
			for(int y=-1; y<=1; y++){
				if(x==0 && y==0){
					continue;
				}

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY){
					neighbour.Add(grid[checkX, checkY]);
				}
			}
		}

		return neighbour;
	}

		public NextNode NodeFromWorldPoint(Vector3 worldPosition) {
			float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
			float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
			percentX = Mathf.Clamp01(percentX);
			percentY = Mathf.Clamp01(percentY);

			int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
			int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
			return grid[x,y];
		}

		public List<NextNode> path;
		void OnDrawGizmos() {
			Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));

		
			if (grid != null) {
				NextNode playerNode = NodeFromWorldPoint(player.position);
				
				foreach (NextNode n in grid) {
					//Debug.Log(n.openPath);
					Gizmos.color = (n.openPath)?Color.yellow:Color.red;
					if(playerNode == n){
						Gizmos.color = Color.cyan;
					}
					if(path != null){
						if(path.Contains(n)){
							Gizmos.color = Color.black;
						}
					}
					Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
				}
			}
		}
	}
}

// public class Node1 {
	
// 	public bool walkable;
// 	public Vector3 worldPosition;
	
// 	public Node(bool _walkable, Vector3 _worldPos) {
// 		walkable = _walkable;
// 		worldPosition = _worldPos;
// 	}
// }

