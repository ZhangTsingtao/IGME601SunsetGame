using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TsingIGME601
{
    public class ItemController : MonoBehaviour
    {
        public GameObject ItemPrefab;
        public int quantity;
        private TextMeshProUGUI quantityText;
        public bool Clicked = false;

        private Button _button;
        
        void Start()
        {
            quantityText = GetComponentInChildren<TextMeshProUGUI>();
            quantityText.text = quantity.ToString();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ButtonClicked);//Don't need to add event in inspector
            
            //Get the LevelEditorManager
            //Maybe need change from finding by tag, but let's keep it for now

        }

        public void ButtonClicked()
        {
            //check anything left, and if the manager is free (no other assets being clicked)
            if (quantity <= 0) return;
            if (LevelEditorManager.Instance.HaveButtonPressed == true) 
            {
                LevelEditorManager.Instance.CancelBuild();
            }
            
            //not clickable until placed, and change quantity left
            Clicked = true;
            MinusQuantity();

            //couple with LevelEditorManager
            //let manager get the right one
            LevelEditorManager.Instance.HaveButtonPressed = true;
            LevelEditorManager.Instance.SetController(this);
            //Let manager instantiate a preview that follows the mouse
            LevelEditorManager.Instance.SpawnPreview();

            //This event is to toggle the navigation on/off
            LevelEditorManager.FurnitureBuilding?.Invoke(true);

        }
        public void AddQuantity()
        {
            quantity++;
            quantityText.text = quantity.ToString();
        }
        public void MinusQuantity()
        {
            quantity--;
            quantityText.text = quantity.ToString();
        }
    }
}

