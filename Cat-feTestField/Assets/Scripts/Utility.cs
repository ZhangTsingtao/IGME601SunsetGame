using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class Utility 
{
    //Take a RaycastHit, then see if there's a grid component
    //if there is, then return the grid position
    //if not, just return the original hit position
    public static Vector3 GetGridPosition(RaycastHit hit)
    {
        Grid grid = hit.collider.GetComponentInChildren<Grid>();
        if (grid != null)
        {
            GridLayout gridLayout = grid.GetComponentInParent<GridLayout>();
            Vector3Int cellPosition = gridLayout.WorldToCell(hit.point);
            return gridLayout.CellToWorld(cellPosition);
        }
        else return hit.point;
    }
}
