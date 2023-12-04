

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RoosaIGM601
{
    public class PathManager : MonoBehaviour
    {
        Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
        PathRequest currentPathRequest;

        public static PathManager instance;
        Pathfinding pathfinding;

        bool isProcessingPath;

        public List<Grid> grids = new List<Grid>();

        public Grid currentGrid;
        public Grid targetGrid;

        void Awake() {
            instance = this;
            pathfinding = GetComponent<Pathfinding>();
        }

        private void Start()
        {
            currentGrid = grids[0];
            targetGrid = grids[0];
            Debug.Log(grids);
        }

        public static void RequestPath(Vector3 pathStart, Grid _currentGrid, Vector3 pathEnd, Grid _targetGrid, Action<Vector3[], bool> callback) {
            if(_currentGrid == _targetGrid)
            {
                Debug.Log("Step 7--> Inside Request Path");
                PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
                instance.pathRequestQueue.Enqueue(newRequest);
                instance.TryProcessNext();
            }
            //Here is the location for jumping algorithm
            else
            {

            }


            instance.targetGrid = _targetGrid;
        }

        void TryProcessNext() {
            if (!isProcessingPath && pathRequestQueue.Count > 0) {
				Debug.Log("Step 8 --> Processing path is false");
                currentPathRequest = pathRequestQueue.Dequeue();
                isProcessingPath = true;
                pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
            }
        }

        void TryProcessNextJump()
        {
            if (!isProcessingPath && pathRequestQueue.Count > 0)
            {
                currentPathRequest = pathRequestQueue.Dequeue();
                isProcessingPath = true;
                pathfinding.StartFindJumpPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
            }
        }

        public void FinishedProcessingPath(Vector3[] path, bool success) {
			Debug.Log("Step 12 --> Inside FinishedProcessingPath" + path +" -- "+ success);
            currentPathRequest.callback(path,success);
            currentGrid = targetGrid;
            isProcessingPath = false;
            TryProcessNext();
        }

        struct PathRequest {
            public Vector3 pathStart;
            public Vector3 pathEnd;
            public Action<Vector3[], bool> callback;

            public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback) {
                pathStart = _start;
                pathEnd = _end;
                callback = _callback;
            }

        }
    }
}
