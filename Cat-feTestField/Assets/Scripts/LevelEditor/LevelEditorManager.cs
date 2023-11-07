using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

namespace TsingIGME601
{
    public class LevelEditorManager : MonoBehaviour
    {
        [Header("Assign these fields")]
        public Material _PreviewMatDenied;
        [SerializeField] private GameObject VisualGrid;
        [Header("Assign this game object, to store all build surfaces and furniture")]
        public GameObject RoomLayout;
        [Header("The rest is generated")]
        public ItemController itemController;
        
        public bool HaveButtonPressed = false;

        [SerializeField] private GameObject ItemPreview;
        
        [SerializeField] private List<GameObject> BuildSurfaces;
        private BuildSurfaceVisual _BSVBuffer;

        //Communicate with Pathfinding
        public static Action FurnitureUpdated;//called when a furniture is built/removed
        public static Action<bool> FurnitureBuilding;//This event is to toggle the navigation on/off


#region Singleton
        private static LevelEditorManager _instance;
        public static LevelEditorManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }
#endregion
        private void Start()
        {
            HaveButtonPressed = false;
            
            AddVisualGrid();
            _BSVBuffer = BuildSurfaces[0].GetComponent<BuildSurfaceVisual>();
        }
        private void Update()
        {
            if (HaveButtonPressed) //if a button is pressed
            {
                //Actually build on surfaces
                if (Input.GetMouseButtonDown(0))
                {
                    BuildFurniture();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    CancelBuild();
                }
            }
        }
        //This method only works when the BuildSurfaces are along the world XYZ axis,
        //if they have other rotation, the method can't handle it
        private void AddVisualGrid()
        {
            Collider[] cols = Physics.OverlapSphere(Vector3.zero, 1000f, LayerMask.GetMask("Build Surface"));
            BuildSurfaces = new();
            for (int i = 0; i < cols.Length; i++)
            {
                //Add visual grid to the build surface as a child
                BuildSurfaces.Add(cols[i].gameObject);
               
                AddOneVisualGridForSurface(i);
            }
        }
        private void AddOneVisualGridForSurface(int i)
        {
            GameObject vg = Instantiate(VisualGrid);
            vg.transform.parent = BuildSurfaces[i].transform.parent.transform;

            BuildSurfaceVisual build = BuildSurfaces[i].AddComponent<BuildSurfaceVisual>();
            build.VisualGrid = vg;

            //choose the thinnest axis of the BuildSurface's collider
            BoxCollider bCol = BuildSurfaces[i].GetComponent<BoxCollider>();
            int axis = 0;
            if (bCol == null) return;

            float[] boundSizes = {bCol.bounds.size.x,
                                      bCol.bounds.size.y,
                                      bCol.bounds.size.z};

            float min = boundSizes[0];
            for (int j = 0; j < boundSizes.Length; j++)
            {
                Debug.Log(j + "  " + BuildSurfaces[i] + "  " + boundSizes[j]);
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
            Vector3 visualOffset = new Vector3();
            switch (axis)
            {
                case 0:
                    visualOffset = new Vector3(normalLength, 0, 0);
                    SetVGPos(vg, visualOffset, toCamDir, surfacePos);

                    vg.transform.rotation = Quaternion.Euler(0, 0, 90);
                    if (Vector3.Dot(vg.transform.up, toCamDir) < 0)
                        vg.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case 1:
                    visualOffset = new Vector3(0, normalLength, 0);
                    SetVGPos(vg, visualOffset, toCamDir, surfacePos);
                    break;
                case 2:
                    visualOffset = new Vector3(0, 0, normalLength);
                    SetVGPos(vg, visualOffset, toCamDir, surfacePos);

                    vg.transform.rotation = Quaternion.Euler(90, 0, 0);
                    if (Vector3.Dot(vg.transform.up, toCamDir) < 0)
                        vg.transform.rotation = Quaternion.Euler(-90, 0, 0);
                    break;
            }
            vg.SetActive(false);
        }
        private void AddOneVisualGridForFurniture(int i)
        {
            BuildSurfaces[i].AddComponent<Grid>();

            GameObject vg = Instantiate(VisualGrid);
            vg.transform.parent = BuildSurfaces[i].transform.parent.transform;

            BuildSurfaceVisual build = BuildSurfaces[i].AddComponent<BuildSurfaceVisual>();
            build.VisualGrid = vg;

            //find y axis of the furniture's collider
            BoxCollider bCol = BuildSurfaces[i].GetComponent<BoxCollider>();
            
            vg.transform.position = bCol.transform.position;
            vg.transform.position += bCol.transform.up * bCol.size.y;
            vg.transform.up = bCol.transform.up;

            vg.SetActive(false);
        }

        //For build surfaces, not furniture
        private void SetVGPos(GameObject vg, Vector3 visualOffset, Vector3 toCamDir, Vector3 surfacePos)
        {
            vg.transform.position = Vector3.Dot(visualOffset, toCamDir) >= 0 ? visualOffset : -visualOffset;
            vg.transform.position += surfacePos;
            Debug.Log(Vector3.Dot(visualOffset, toCamDir));
        }

        public void BuildFurniture()
        {
            if (ItemPreview == null) return;
            if (ItemPreview.GetComponent<PreviewFollowMouse>()._placeable == false) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layer_mask = LayerMask.GetMask("Build Surface");
            layer_mask += LayerMask.GetMask("Unwalkable");
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer_mask))
            {
                //Build
                itemController.Clicked = false;
                GameObject item = Instantiate(itemController.ItemPrefab, ItemPreview.transform.position, ItemPreview.transform.rotation);
                RemoveItem removeItem = item.AddComponent<RemoveItem>();
                removeItem.SetController(itemController);
                item.transform.parent = RoomLayout.transform;

                //change collider to isTrigger
                item.GetComponent<BoxCollider>().isTrigger = true;

                FurnitureUpdated?.Invoke();
                FurnitureBuilding?.Invoke(false);
                HaveButtonPressed = false;

                Destroy(ItemPreview);
                itemController = null;

                //Build on top of furniture
                if(item.TryGetComponent<AbleToBuiltOn>(out AbleToBuiltOn component))
                {
                    BuildSurfaces.Add(item);
                    
                    AddOneVisualGridForFurniture(BuildSurfaces.Count - 1);
                }
            }
            //Disable Visual Grid
            _BSVBuffer.VisualGrid.SetActive(false);

        }

        //called by controller (the button with the image)
        public void SpawnPreview()
        {
            //instantiate a preview that follows the mouse
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ItemPreview = Instantiate(itemController.ItemPrefab, worldPos, Quaternion.identity);
            ItemPreview.SetActive(true);
            var previewScript = ItemPreview.AddComponent<PreviewFollowMouse>();
            
            //assign the preview denied material
            previewScript._previewDeniedMaterial = _PreviewMatDenied;

            //materials are changed to transparent at the Start of PreviewFollowMouse

            //make the preview ignored by raycast,
            //so that PreviewFollowMouse will avoid raycast from camera,
            //not constantly changing its position
            int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
            ItemPreview.layer = LayerIgnoreRaycast;
        }
        
        public void CancelBuild()
        {
            Destroy(ItemPreview);
            HaveButtonPressed = false;

            itemController.AddQuantity();
            itemController.Clicked = false;
            itemController = null;

            FurnitureBuilding?.Invoke(false);

            //Disable Visual Grid
            _BSVBuffer.VisualGrid.SetActive(false);
        }
        public void SetController(ItemController controller)
        {
            if (itemController == null)
            {
                itemController = controller;
            }
        }
        
        public void ShowVisualGrid(BuildSurfaceVisual buildSurface)
        {
            if (buildSurface.VisualGrid == null)
            {
                Debug.LogWarning("No visual grid found!");
                return;
            }
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
        public void RemoveFromBuildSurfaces(GameObject obj)
        {
            BuildSurfaces.Remove(obj);
        }
    }
}

