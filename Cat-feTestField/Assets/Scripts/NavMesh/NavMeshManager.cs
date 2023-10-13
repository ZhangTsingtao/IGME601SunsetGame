using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using Unity.AI.Navigation;


namespace TsingIGME601
{
    public class NavMeshManager : MonoBehaviour
    {
        [SerializeField] public GameObject[] Obstacles;

        [SerializeField] public NavMeshSurface[] Surfaces;

        private bool _click = true;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                PopObstacles();
            }
        }

        private void PopObstacles()
        {
            _click = !_click;
            foreach (GameObject obstacle in Obstacles)
            {
                obstacle.SetActive(_click);
            }

            foreach (NavMeshSurface surface in Surfaces)
            {
                surface.UpdateNavMesh(surface.navMeshData);
            }
        }
    }

}
