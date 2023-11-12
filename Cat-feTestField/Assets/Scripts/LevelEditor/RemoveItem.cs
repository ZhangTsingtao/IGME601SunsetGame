using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsingIGME601 
{
    public class RemoveItem : MonoBehaviour
    {
        public ItemController _controller;

        public void SetController(ItemController controller)
        {
            _controller = controller;
        }

        
        private void OnMouseOver()
        {
            if (LevelEditorManager.Instance.HaveButtonPressed)
                return;
            if (Input.GetMouseButtonDown(1))
            {
                _controller.AddQuantity();
                Destroy(this.gameObject);
            }
        }
        private void OnDestroy()
        {
            if (transform.TryGetComponent<AbleToBuiltOn>(out AbleToBuiltOn component))
            {
                LevelEditorManager.Instance.RemoveFromBuildSurfaces(transform.gameObject);
            }
            //Debug.Log("Furniture Destroyed, the grid should be updated");
            LevelEditorManager.FurnitureUpdated?.Invoke();
        }
    }
}

