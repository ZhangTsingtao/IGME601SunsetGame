using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TsingIGME601 
{
    public class RemoveItem : MonoBehaviour
    {
        public int ID;
        private LevelEditorManager _editor;

        void Start()
        {
            _editor = GameObject.FindGameObjectWithTag("LevelEditorManager").
                GetComponent<LevelEditorManager>();
        }

        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Destroy(this.gameObject);
                _editor.ItemButtons[ID].quantity++;
                _editor.ItemButtons[ID].quantityText.text = _editor.ItemButtons[ID].quantity.ToString();
            }
        }
    }
}

