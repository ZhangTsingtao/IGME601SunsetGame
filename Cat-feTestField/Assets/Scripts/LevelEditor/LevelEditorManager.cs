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
        public Material _previewMaterial;

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
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    //Build
                    ItemButtons[CurrentButtonPressed].Clicked = false;
                    Instantiate(ItemPrefabs[CurrentButtonPressed], hit.point, hit.transform.rotation);
                    CurrentButtonPressed = -1;

                    //Destroy the image
                    if (ItemPreview != null)
                    {
                        Destroy(ItemPreview);
                    }
                }
            }
        }

        public void SpawnPreview(int id)
        {
            //instantiate a preview that follows the mouse
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ItemPreview = Instantiate(ItemPrefabs[id], worldPos, Quaternion.identity);
            ItemPreview.SetActive(true);
            ItemPreview.AddComponent<PreviewFollowMouse>();

            //change material to transparent
            ItemPreview.GetComponent<MeshRenderer>().material = _previewMaterial;


        }
    }
}

