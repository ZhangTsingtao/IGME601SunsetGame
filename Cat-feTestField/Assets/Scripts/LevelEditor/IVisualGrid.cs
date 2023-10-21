using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisualGrid 
{
    public void TryShowVisualGrid();
    //PreviewFollowMouse provokes BuildSurfaceVisual, to set the visual grid active;
    public void TryHideVisualGrid();
}
