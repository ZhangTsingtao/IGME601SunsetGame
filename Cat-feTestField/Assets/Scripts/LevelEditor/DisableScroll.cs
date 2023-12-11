using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisableScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CameraRotateScript cameraRotateScript;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ASdasdasdsa");
        cameraRotateScript.canScroll = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cameraRotateScript.canScroll = true;
    }
}
