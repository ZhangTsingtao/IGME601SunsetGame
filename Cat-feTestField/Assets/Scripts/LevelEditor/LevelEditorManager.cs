using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TsingIGME601
{
    public class LevelEditorManager : MonoBehaviour
    {
        public ItemController[] ItemButtons;
        public GameObject[] ItemPrefabs;
        public int CurrentButtonPressed = -1;

        public GameObject ItemPreview;
        [SerializeField] private Material _previewMaterial;

        public Grid[] Grids;

        private void Start()
        {
            CurrentButtonPressed = -1;
        }
        private void Update()
        {
            //Actually build on surfaces
            if (Input.GetMouseButtonDown(0) && CurrentButtonPressed != -1)//ItemButtons[CurrentButtonPressed].Clicked
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                int layer_mask = LayerMask.GetMask("Build Surface");
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer_mask))
                {
                    //Build
                    ItemButtons[CurrentButtonPressed].Clicked = false;
                    Instantiate(ItemPrefabs[CurrentButtonPressed], Utility.GetGridPosition(hit), hit.transform.rotation);
                    CurrentButtonPressed = -1;

                    //Destroy the image
                    if (ItemPreview != null)
                    {
                        Destroy(ItemPreview);
                    }
                }
            }
        }

        //called by controller (the button with the image)
        public void SpawnPreview(int id)
        {
            //instantiate a preview that follows the mouse
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ItemPreview = Instantiate(ItemPrefabs[id], worldPos, Quaternion.identity);
            ItemPreview.SetActive(true);
            ItemPreview.AddComponent<PreviewFollowMouse>();

            //change material to transparent
            ItemPreview.GetComponent<MeshRenderer>().material = _previewMaterial;
            
            //make the preview ignored by raycast,
            //so that PreviewFollowMouse will behave as expected
            int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
            ItemPreview.layer = LayerIgnoreRaycast;

            //Assign this manager script to ItemPreview,
            //so that it has reference to all grids
            ItemPreview.GetComponent<PreviewFollowMouse>()._manager = this;

        }

        
    }
}

