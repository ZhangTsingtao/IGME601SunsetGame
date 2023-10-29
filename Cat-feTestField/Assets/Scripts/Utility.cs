using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.RuleTile.TilingRuleOutput;
namespace TsingIGME601
{
    public static class Utility
    {
        //Take a RaycastHit, then see if there's a grid component
        //if there is, then return the grid position
        //if not, just return the original hit position
        public static Vector3 GetGridPosition(RaycastHit hit)
        {
            GridLayout gridLayout = hit.transform.GetComponentInChildren<GridLayout>();

            if (gridLayout == null)
            {
                Debug.LogWarning("Can't find any grid on Raycast hit");
                return hit.point;
            }
            
            Vector3Int cellPosition = gridLayout.WorldToCell(hit.point);
            return gridLayout.CellToWorld(cellPosition);
        }
        public static Material MaterialOpaqueToTransparent(Material material, float alphaValue)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.SetInt("_Surface", 1);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            material.SetShaderPassEnabled("DepthOnly", false);
            material.SetShaderPassEnabled("SHADOWCASTER", false);

            material.SetOverrideTag("RenderType", "Transparent");

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");

            material.color = new Color(material.color.r, material.color.g, material.color.b, alphaValue);
            
            return material;
        }

        public static Material MaterialTransparentToOpaque(Material material)
        {
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.SetInt("_Surface", 0);

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

            material.SetShaderPassEnabled("DepthOnly", true);
            material.SetShaderPassEnabled("SHADOWCASTER", true);

            material.SetOverrideTag("RenderType", "Opaque");

            material.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");

            material.color = new Color(material.color.r, material.color.g, material.color.b, 1f);

            return material;
        }
    }
}

