using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RoosaIGM601
{
	public class Pathfinding : MonoBehaviour
	{
		// Start is called before the first frame update
		//public Transform seeker, target;

		PathManager requestManager;
		Grid grid;

		private float rotSpeed = 30f;

		void Awake() {
			requestManager = GetComponent<PathManager>();
		}

        private void Start()
        {
			grid = GetComponent<Grid>();
		}

        public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
			Debug.Log("Step 9 --> Inside Start Finding path");
			StartCoroutine(FindPath(startPos,targetPos));
		}

		public void StartFindJumpPath(Vector3 startPos, Vector3 targetPos)
        {

        }

		IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
			//CatRotation(targetPos);
			Debug.Log("Step 10 --> Finding path");
			Vector3[] waypoints = new Vector3[0];
			bool pathSuccess = false;
			
			NextNode startNode = grid.NodeFromWorldPoint(startPos);
			NextNode targetNode = grid.NodeFromWorldPoint(targetPos);
			if (startNode.openPath && targetNode.openPath) {
				List<NextNode> openSet = new List<NextNode>();
				HashSet<NextNode> closedSet = new HashSet<NextNode>();
				openSet.Add(startNode);

				while (openSet.Count > 0) {
					NextNode currentNode = openSet[0];
					for (int i = 1; i < openSet.Count; i ++) {
						if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
							currentNode = openSet[i];
						}
					}

					openSet.Remove(currentNode);
					closedSet.Add(currentNode);

					if (currentNode == targetNode) {
						pathSuccess=true;
						//RetracePath(startNode,targetNode);
						break;
					}

					foreach (NextNode neighbour in grid.GetNeighbours(currentNode)) {
						if (!neighbour.openPath || closedSet.Contains(neighbour)) {
							continue;
						}

						int newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
						if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
							neighbour.gCost = newCostToNeighbour;
							neighbour.hCost = GetDistance(neighbour, targetNode);
							neighbour.parent = currentNode;

							if (!openSet.Contains(neighbour))
								openSet.Add(neighbour);
						}
					}
				}
			}
			yield return null;
			if (pathSuccess) {
				waypoints = RetracePath(startNode,targetNode);
			}
			Debug.Log("Step 11 --> Calling Finished Processing Path");
			requestManager.FinishedProcessingPath(waypoints,pathSuccess);
		}

		IEnumerator FindJumpPath(Vector3 startPos, Vector3 targetPos)
        {
			yield return null;
        }

		Vector3[] RetracePath(NextNode startNode, NextNode endNode) {
			List<NextNode> path = new List<NextNode>();
			NextNode currentNode = endNode;

			while (currentNode != startNode) {
				path.Add(currentNode);
				currentNode = currentNode.parent;
			}
			Vector3[] waypoints = SimplifyPath(path);
			Array.Reverse(waypoints);
			return waypoints;

			//surface.path = path;

		}

		Vector3[] SimplifyPath(List<NextNode> path) {
			List<Vector3> waypoints = new List<Vector3>();
			Vector2 directionOld = Vector2.zero;
			
			for (int i = 1; i < path.Count; i ++) {
				Vector2 directionNew = new Vector2(path[i-1].gridX - path[i].gridX,path[i-1].gridY - path[i].gridY);
				if (directionNew != directionOld) {
					waypoints.Add(path[i].worldPosition);
				}
				directionOld = directionNew;
			}
			return waypoints.ToArray();
		}

		int GetDistance(NextNode nodeA, NextNode nodeB) {
			int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
			int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

			if (dstX > dstY)
				return 14*dstY + 10* (dstX-dstY);
			return 14*dstX + 10 * (dstY-dstX);
		}
	}
}

