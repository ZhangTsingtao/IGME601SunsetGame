using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitApplication : MonoBehaviour
{
    private Button _button;
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ButtonClicked);//Don't need to add event in inspector
    }
    public void ButtonClicked()
    {
        Application.Quit();
    }
}
