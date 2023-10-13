using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TsingIGME601
{
    public class ItemController : MonoBehaviour
    {
        public int ID;
        public int quantity;
        public TextMeshProUGUI quantityText;
        public bool Clicked = false;

        private Button _button;
        private LevelEditorManager _editor;
        void Start()
        {
            quantityText.text = quantity.ToString();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ButtonClicked);//Don't need to add event in inspector
            
            //Get the LevelEditorManager
            //Maybe need change from finding by tag, but let's keep it for now
            _editor = GameObject.FindGameObjectWithTag("LevelEditorManager").
                GetComponent<LevelEditorManager>();
        }

        public void ButtonClicked()
        {
            //check anything left, and if the manager is free (no other assets being clicked)
            if (quantity > 0 && _editor.CurrentButtonPressed == -1)
            {
                //not clickable until placed, and change quantity left
                Clicked = true;
                quantity--;
                quantityText.text = quantity.ToString();

                //couple with LevelEditorManager
                //let manager get the right one
                _editor.CurrentButtonPressed = ID;

                //Let manager instantiate a preview that follows the mouse
                _editor.SpawnPreview(ID);
            }
        }

    }
}

