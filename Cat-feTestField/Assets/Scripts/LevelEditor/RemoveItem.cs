using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsingIGME601 
{
    public class RemoveItem : MonoBehaviour
    {
        public ItemController _controller;
        private LevelEditorManager _editor;

        public void SetController(ItemController controller)
        {
            _controller = controller;
        }

        void Start()
        {
            _editor = GameObject.FindGameObjectWithTag("LevelEditorManager").
                GetComponent<LevelEditorManager>();
        }

        
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _controller.AddQuantity();
                Destroy(this.gameObject);
            }
        }
        private void OnDestroy()
        {
            Debug.Log("Furniture Destroyed, the grid should be updated");
            GameObject objectWithMyScript = GameObject.Find("A*");
            if (objectWithMyScript != null)
            {
                Grid grid = objectWithMyScript.GetComponent<Grid>();
                grid.CreateGrid();
            } 
        }
    }
}

