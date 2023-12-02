using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

namespace TsingIGME601
{
    public class PreviewFollowMouse : MonoBehaviour
    {
        private BoxCollider _collider;

        public bool _placeable = false;
        public Material _previewDeniedMaterial;
        public Material[] _materials;
        public Color[] _originalColorBuffer;
        private GameObject VisualGrid;

        //rotation
        [SerializeField] private int _rotationDegree = 0;
        private void Start()
        {
            _collider = gameObject.GetComponent<BoxCollider>();

            _materials = GetComponent<MeshRenderer>().materials;
            _originalColorBuffer = new Color[_materials.Length];
            for (int i = 0; i < _materials.Length; i++)
            {
                //store original materials
                _originalColorBuffer[i] = _materials[i].color;
                _originalColorBuffer[i].a = 0.5f;

                //set material to transparent
                Material newMat = Utility.MaterialOpaqueToTransparent(_materials[i], 0.5f);
                _materials[i] = newMat;
            }

            VisualGrid = Instantiate(LevelEditorManager.Instance.VisualGrid);
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                AddRotationPerpendicular();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                MinusRotationPerpendicular();
            }
        }
        private void AddRotationPerpendicular()
        {
            _rotationDegree += 90;
            if(_rotationDegree == 360)
            {
                _rotationDegree = 0;
            }
        }
        private void MinusRotationPerpendicular()
        {
            _rotationDegree -= 90;
            if (_rotationDegree == -360)
            {
                _rotationDegree = 0;
            }
        }
        private void LateUpdate()
        {
            bool showGrid = true;
            showGrid &= PositionToGrid();
            CollisionDetection();
            showGrid &= _placeable;
            ArrangeVisualGrid(showGrid);
        }
        private bool PositionToGrid()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layer_mask = LayerMask.GetMask("Build Surface");
            layer_mask += LayerMask.GetMask("Unwalkable");
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer_mask))
            {
                //get grid position
                bool showGridCheck;
                (transform.position, showGridCheck) = Utility.GetGridPosition(hit);

                //update rotation
                SetRotation(hit.transform, hit);

                
                return showGridCheck;
            }
            else
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                transform.position = new Vector3(worldPos.x, worldPos.y, worldPos.z) + Camera.main.transform.forward * 10;

                //LevelEditorManager.Instance.ClearVisualGrid();
                return false;
            }
        }
        private void SetRotation(Transform targetTransform, RaycastHit hit)
        {
            transform.up = hit.normal;

            transform.rotation = transform.rotation * Quaternion.AngleAxis(_rotationDegree, Vector3.up);
        }
        //private void SetRotation(Transform targetTransform, RaycastHit hit)
        //{
        //    transform.up = hit.normal;
        //    if (Vector3.Angle(transform.up, targetTransform.up) > 10)
        //    {
        //        //Debug.Log("Angle greater than 10");
        //        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetTransform.rotation, 180f);
        //    }
        //    if (_rotationDegree != 0)
        //    {
        //        transform.rotation = transform.rotation * Quaternion.AngleAxis(_rotationDegree, Vector3.up);
        //        _rotationDegree = 0;
        //    }
        //}

        private Collider[] _colBuffer = new Collider[4];
        private void CollisionDetection()
        { 
            int hitAmount = 0;
            hitAmount = Physics.OverlapBoxNonAlloc
                (
                    transform.TransformPoint(_collider.center),
                    _collider.bounds.extents * 0.9f,
                    _colBuffer,
                    Quaternion.identity
                );

            //Utility.DisplayBox
            //    (
            //        transform.TransformPoint(_collider.center),
            //        _collider.bounds.extents * 0.9f, 
            //        Quaternion.identity
            //    );

            if (hitAmount > 1)//itself is also included. so start from 1 rather than 0
            {
                if (_placeable)
                {
                    _placeable = false;
                    //Debug.Log("Overlap");
                    for (int i = 0; i < _materials.Length; i++)
                    {
                        Material newMat = Utility.MaterialOpaqueToTransparent(_materials[i], 0.5f);
                        _materials[i] = newMat;
                        _materials[i].color = _previewDeniedMaterial.color;
                    }
                }
            }
            //no overlap
            else
            {
                if (!_placeable)
                {
                    _placeable = true;
                    //Debug.Log("Fine");
                    for (int i = 0; i < _materials.Length; i++)
                    {
                        _materials[i].color = _originalColorBuffer[i];
                    }
                }
            }
        }
        //private void CollisionDetection()
        //{
        //    Collider[] hitColliders = Physics.OverlapBox
        //        (transform.TransformPoint(Vector3.up * _col.bounds.extents.y / transform.localScale.y), 
        //        _col.bounds.extents / 2, 
        //        Quaternion.identity);//transform.localScale / 2

        //    //Debug.Log("extents : " + _col.bounds.extents);

        //    if (hitColliders.Length > 1)//there must be itself. so start from one
        //    {
        //        //for (int i = 0;i< hitColliders.Length; i++)
        //        //{
        //        //    Debug.Log("Collider " + i + ": " + hitColliders[i].name.ToString());
        //        //}
        //        if (_placeable)
        //        {
        //            _placeable = false;
        //            //Debug.Log("Overlap");
        //            foreach(Material mat in _materials)
        //            {
        //                mat.color = _previewDeniedMaterial.color;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (!_placeable)
        //        {
        //            _placeable = true;
        //            //Debug.Log("Fine");
        //            foreach (Material mat in _materials)
        //            {
        //                mat.SetFloat("_Mode", 3);
        //                mat.color = new Color(mat.color.r,mat.color.g,mat.color.b,0.5f);
        //            }
        //        }
        //    }
        //}

        //used for removing itself
        private void SelfCheck(Collider[] hitColliders)
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

        private void ArrangeVisualGrid(bool showGrid)
        {
            if(!showGrid)
            {
                VisualGrid.SetActive(false);
                return;
            }
            VisualGrid.SetActive(true);
            VisualGrid.transform.position = transform.position + transform.up * 0.05f;
            VisualGrid.transform.rotation = transform.rotation;
        }
        private void OnDestroy()
        {
            Destroy(VisualGrid.gameObject);
        }
    }
}

