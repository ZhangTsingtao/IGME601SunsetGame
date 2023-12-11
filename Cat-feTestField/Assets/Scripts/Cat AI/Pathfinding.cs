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

        // void Update() {
        // 	FindPath (seeker.position, target.position);
        // }

        // IEnumerator CatRotation(Vector3 targetPos){
        // //     // Vector3 dir = target- transform.position;
        // //     // Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);
        // //     // rot.y = 0;
        // //     // rot.z = 0;
        // //     // transform.rotation = rot;

        // //     // float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed;
        // //     // transform.Rotate(new Vector3(0, 0,mouseY));

        // //     Vector3 direction = target.position - transform.position;
        // //     float distance = direction.magnitude;
        // //     Vector2 velocity = direction.normalized * (distance / targetRadius);
        // //     float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        // //     transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        //         Debug.Log("Hello Cat");
        //         Vector3 direction = targetPos - transform.position;
        //         Quaternion rotation = Quaternion.LookRotation(direction);

        //         // Smoothly rotate towards the direction
        //         //float rotationSpeed = 5f;
        //         transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
        // 		yield return null;

        // }

        public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
			StartCoroutine(FindPath(startPos,targetPos));
		}

		public void StartFindJumpPath(Vector3 startPos, Vector3 targetPos)
        {
			StartCoroutine(FindJumpPath(startPos, targetPos));
		}

		IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
			//CatRotation(targetPos);
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

