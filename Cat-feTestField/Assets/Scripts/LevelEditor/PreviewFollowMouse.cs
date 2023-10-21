using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsingIGME601
{
    public class PreviewFollowMouse : MonoBehaviour
    {
        public LevelEditorManager _editor;

        private Collider _col;

        public bool _placeable = false;
        [SerializeField] private Material _material;
        [SerializeField] private Material _previewMaterial;
        [SerializeField] private Material _previewDeniedMaterial;
        private void Start()
        {
            _material = GetComponent<MeshRenderer>().material;
            _previewMaterial = _editor._previewMaterial;
            _previewDeniedMaterial = _editor._PreviewDeniedMaterial;
            _col = gameObject.GetComponent<Collider>();
        }
        void LateUpdate()
        {
            positionToGrid();
            collisionDetection();
        }
        private void positionToGrid()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layer_mask = LayerMask.GetMask("Build Surface");
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer_mask))
            {
                //get grid position
                transform.position = Utility.GetGridPosition(hit);

                //update rotation
                transform.rotation = hit.transform.rotation;

                //the surface also shows its visual grid
                _editor.ShowVisualGrid(hit.transform.GetComponent<BuildSurfaceVisual>());
            }
            else
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                transform.position = new Vector3(worldPos.x, worldPos.y, worldPos.z) + Camera.main.transform.forward * 10;

                _editor.ClearVisualGrid();
            }
        }

        private void collisionDetection()
        {
            Collider[] hitColliders = Physics.OverlapBox
                (transform.TransformPoint(Vector3.up * _col.bounds.size.y / 2), transform.localScale / 2, Quaternion.identity);

            if(hitColliders.Length > 1)//there must be itself. so start from one
            {
                if (_placeable)
                {
                    _placeable = false;
                    //Debug.Log("Overlap");
                    _material.color = _previewDeniedMaterial.color;
                }
            }
            else
            {
                if (!_placeable)
                {
                    _placeable = true;
                    //Debug.Log("Fine");
                    _material.color = _previewMaterial.color;
                }
            }
        }

        //used for removing itself
        private void selfCheck(Collider[] hitColliders)
        {
            if (hitColliders.Length > 0)
            {
                int i = 0;
                //Debug.Log(hitColliders.Length);
                for (i = 0; i < hitColliders.Length; i++)
                {
                    //Debug.Log(hitColliders[i].name);
                    if (hitColliders[i].gameObject == gameObject)
                    {
                        //Debug.Log("This is me");
                        break;
                    }
                }
                for (int j = i; j < hitColliders.Length - 1; j++)
                {
                    hitColliders[j] = hitColliders[j + 1];
                }
                int collidersLength = hitColliders.Length - 1;
                for (i = 0; i < collidersLength; i++)
                {
                    Debug.Log(hitColliders[i].name);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawCube(transform.TransformPoint(Vector3.up * _col.bounds.size.y / 2), 
                _col.bounds.size);
        }

    }
}

