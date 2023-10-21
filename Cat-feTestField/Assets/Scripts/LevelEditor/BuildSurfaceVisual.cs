using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSurfaceVisual : MonoBehaviour, IVisualGrid
{
    public GameObject VisualGrid;
    private void Awake()
    {
        VisualGrid = new GameObject();
    }
    public void TryShowVisualGrid()
    {
        if (VisualGrid != null)
        {
            if (!VisualGrid.activeSelf)
            {
                VisualGrid.SetActive(true);
            }
        }
    }
    public void TryHideVisualGrid()
    {
        if(VisualGrid != null)
        {
            if (VisualGrid.activeSelf)
            {
                VisualGrid.SetActive(false);
            }
        }
    }
}
