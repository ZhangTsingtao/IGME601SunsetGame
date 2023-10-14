using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsingIGME601
{
    public class PreviewFollowMouse : MonoBehaviour
    {
        public LevelEditorManager _manager;
        void LateUpdate()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layer_mask = LayerMask.GetMask("Build Surface");
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer_mask))
            {
                //get grid position
                transform.position = Utility.GetGridPosition(hit);

                //update rotation
                transform.rotation = hit.transform.rotation;
            }
            else
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                transform.position = new Vector3(worldPos.x, worldPos.y, worldPos.z) + Camera.main.transform.forward * 10;
            }
            
        }
    }
}

