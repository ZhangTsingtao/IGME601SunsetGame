using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleActive : MonoBehaviour
{
    public Button toggleButton;
    public GameObject toggleUIElement;
    private bool toggle;
    // Start is called before the first frame update
    void Start()
    {
        toggle = true;
        toggleButton = GetComponent<Button>();
        toggleButton.onClick.AddListener(ToggleGameObjectActive);
    }

    private void ToggleGameObjectActive()
    {
        toggleUIElement.SetActive(toggle);
        toggle = !toggle;
    }
}
