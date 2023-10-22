using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ObjectChangeEventStream;


namespace TsingIGME601
{
    public class LevelEditorManager : MonoBehaviour
    {
        public ItemController[] ItemButtons;
        public GameObject[] ItemPrefabs;
        public int CurrentButtonPressed = -1;

        public GameObject ItemPreview;
        public Material _previewMaterial;
        public Material _PreviewDeniedMaterial;

        public Grid[] Grids;

        [SerializeField] private GameObject VisualGrid;
        [SerializeField] private GameObject[] BuildSurfaces;
        BuildSurfaceVisual _BSVBuffer;

        private void Start()
        {
            CurrentButtonPressed = -1;

            addVisualGrid();
            _BSVBuffer = BuildSurfaces[0].GetComponent<BuildSurfaceVisual>();
        }
        private void Update()
        {
            if (CurrentButtonPressed != -1) //if a button is pressed
            {
                //Actually build on surfaces
                if (Input.GetMouseButtonDown(0) && CurrentButtonPressed != -1)
                {
                    BuildFurniture();
                }
                if (Input.GetMouseButtonDown(1) && CurrentButtonPressed != -1)
                {
                    CancelBuild(CurrentButtonPressed);
                }
            }

        }
        //This method only works when the BuildSurfaces are along the world XYZ axis,
        //if they have other rotation, the method can't handle it
        private void addVisualGrid()
        {
            Collider[] cols = Physics.OverlapSphere(Vector3.zero, 1000f, LayerMask.GetMask("Build Surface"));
            BuildSurfaces = new GameObject[cols.Length];
            for (int i = 0; i < cols.Length; i++)
            {
                //Add visual grid to the build surface as a child
                BuildSurfaces[i] = cols[i].gameObject;
                
                GameObject vg = Instantiate(VisualGrid);
                vg.transform.parent = BuildSurfaces[i].transform.parent.transform;

                BuildSurfaceVisual build = BuildSurfaces[i].AddComponent<BuildSurfaceVisual>();
                build.VisualGrid = vg;
                

                //choose the thinnest axis of the BuildSurface's collider
                BoxCollider bCol = BuildSurfaces[i].GetComponent<BoxCollider>();
                int axis = 0;
                if (bCol != null)
                {
                    float[] boundSizes = {bCol.bounds.size.x,
                                      bCol.bounds.size.y,
                                      bCol.bounds.size.z};
                    
                    float min = boundSizes[0];
                    for (int j = 0; j < boundSizes.Length; j++)
                    {
                        Debug.Log(j + "  " +BuildSurfaces[i] + "  "+ boundSizes[j]);
                        if (boundSizes[j] < min)
                        {
                            min = boundSizes[j];
                            axis = j;
                        }
                    }

                    Debug.Log(axis.ToString());

                    //set position, rotation based on the BuildSurface's collider bounding box
                    //the visual grid snaps to the thinnest side of the collider
                    float normalLength = boundSizes[axis] / 2 + 0.01f;
                    Vector3 surfacePos = BuildSurfaces[i].transform.position;
                    Vector3 toCamDir = Camera.main.transform.position - BuildSurfaces[i].transform.position;
                    Vector3 visualOffset = new();
                    switch (axis)
                    {
                        case 0:
                            visualOffset = new Vector3(normalLength, 0, 0);
                            setVGPos(vg, visualOffset, toCamDir, surfacePos);

                            vg.transform.rotation = Quaternion.Euler(0, 0, 90);
                            if(Vector3.Dot(vg.transform.up,toCamDir) < 0)
                                vg.transform.rotation = Quaternion.Euler(0, 0, -90);
                            break;
                        case 1:
                            visualOffset = new Vector3(0, normalLength, 0);
                            setVGPos(vg, visualOffset, toCamDir, surfacePos);
                            break;
                        case 2:
                            visualOffset = new Vector3(0, 0, normalLength);
                            setVGPos(vg, visualOffset, toCamDir, surfacePos);

                            vg.transform.rotation = Quaternion.Euler(90, 0, 0);
                            if (Vector3.Dot(vg.transform.up, toCamDir) < 0)
                                vg.transform.rotation = Quaternion.Euler(-90, 0, 0);
                            break;
                    }
                    
                    vg.SetActive(false);
                }
                else continue;
            }
        }
        private void setVGPos(GameObject vg, Vector3 visualOffset, Vector3 toCamDir, Vector3 surfacePos)
        {
            vg.transform.position = Vector3.Dot(visualOffset, toCamDir) >= 0 ? visualOffset : -visualOffset;
            vg.transform.position += surfacePos;
            Debug.Log(Vector3.Dot(visualOffset, toCamDir));
        }

        public void BuildFurniture()
        {
            if(ItemPreview != null && ItemPreview.GetComponent<PreviewFollowMouse>()._placeable)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layer_mask = LayerMask.GetMask("Build Surface");
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer_mask))
                {
                    //Build
                    ItemButtons[CurrentButtonPressed].Clicked = false;
                    Instantiate(ItemPrefabs[CurrentButtonPressed], Utility.GetGridPosition(hit), hit.transform.rotation);
                    CurrentButtonPressed = -1;

                    Destroy(ItemPreview);
                }
                //Disable Visual Grid
                _BSVBuffer.VisualGrid.SetActive(false);
            }
            
            
        }

        //called by controller (the button with the image)
        public void SpawnPreview(int id)
        {
            //instantiate a preview that follows the mouse
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ItemPreview = Instantiate(ItemPrefabs[id], worldPos, Quaternion.identity);
            ItemPreview.SetActive(true);
            var previewScript = ItemPreview.AddComponent<PreviewFollowMouse>();
            previewScript._editor = this;

            //change material to transparent
            Color _color = ItemPreview.GetComponent<MeshRenderer>().material.color;
            _previewMaterial.color = new Color(_color.r, _color.g, _color.b, 0.5f);
            ItemPreview.GetComponent<MeshRenderer>().material = _previewMaterial;
            
            //make the preview ignored by raycast,
            //so that PreviewFollowMouse will behave as expected
            int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
            ItemPreview.layer = LayerIgnoreRaycast;

            //Assign this manager script to ItemPreview,
            //so that it has reference to all grids
            ItemPreview.GetComponent<PreviewFollowMouse>()._editor = this;

        }
        
        public void CancelBuild(int id)
        {
            Destroy(ItemPreview);
            ItemButtons[id].AddQuantity();
            CurrentButtonPressed = -1;
        }

        
        public void ShowVisualGrid(BuildSurfaceVisual buildSurface)
        {
            if (_BSVBuffer != buildSurface)
            {
                buildSurface.VisualGrid.SetActive(true);
                _BSVBuffer.VisualGrid.SetActive(false);
                _BSVBuffer = buildSurface;
            }
            else
            {
                if (!_BSVBuffer.VisualGrid.activeSelf)
                {
                    _BSVBuffer.VisualGrid.SetActive(true);
                }
            } 
        }
        public void ClearVisualGrid()
        {
            _BSVBuffer.VisualGrid.SetActive(false);
        }
    }
}

