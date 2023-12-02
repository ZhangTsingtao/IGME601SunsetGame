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

        //public static (Vector3, bool) GetGridPosition(RaycastHit hit)
        //{
        //    Grid grid = hit.transform.GetComponentInChildren<Grid>();

        //    //Grid grid = GameObject.Find("BuildGrid").GetComponent<Grid>();

        //    if (grid == null)
        //    {
        //        Debug.Log("Can't find any grid on Raycast hit");
        //        return (hit.point, false);
        //    }
        //    Vector3Int cellPosition = grid.WorldToCell(hit.point);
        //    return (grid.GetCellCenterWorld(cellPosition), true);
        //}

        public static (Vector3, bool) GetGridPosition(RaycastHit hit)
        {
            GridLayout gridLayout = hit.transform.GetComponentInChildren<GridLayout>();
            //GridLayout gridLayout = GameObject.Find("BuildGrid").GetComponent<GridLayout>();
            if (gridLayout == null)
            {
                //Debug.LogWarning("Can't find any grid on Raycast hit");
                return (hit.point, false);
            }

            Vector3Int cellPosition = gridLayout.WorldToCell(hit.point);
            return (gridLayout.CellToWorld(cellPosition), true);
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


        public static void DisplayBox(Vector3 center, Vector3 HalfExtend, Quaternion rotation, float Duration = 0)
        {
            Vector3[] Vertices = new Vector3[8];
            int i = 0;
            for (int x = -1; x < 2; x += 2)
            {
                for (int y = -1; y < 2; y += 2)
                {
                    for (int z = -1; z < 2; z += 2)
                    {
                        Vertices[i] = center + new Vector3(HalfExtend.x * x, HalfExtend.y * y, HalfExtend.z * z);
                        i++;
                    }
                }
            }

            Vertices = RotateObject(Vertices, rotation.eulerAngles, center);

            Debug.DrawLine(Vertices[0], Vertices[1], Color.white, Duration);
            Debug.DrawLine(Vertices[1], Vertices[3], Color.white, Duration);
            Debug.DrawLine(Vertices[2], Vertices[3], Color.white, Duration);
            Debug.DrawLine(Vertices[2], Vertices[0], Color.white, Duration);
            Debug.DrawLine(Vertices[4], Vertices[0], Color.white, Duration);
            Debug.DrawLine(Vertices[4], Vertices[6], Color.white, Duration);
            Debug.DrawLine(Vertices[2], Vertices[6], Color.white, Duration);
            Debug.DrawLine(Vertices[7], Vertices[6], Color.white, Duration);
            Debug.DrawLine(Vertices[7], Vertices[3], Color.white, Duration);
            Debug.DrawLine(Vertices[7], Vertices[5], Color.white, Duration);
            Debug.DrawLine(Vertices[1], Vertices[5], Color.white, Duration);
            Debug.DrawLine(Vertices[4], Vertices[5], Color.white, Duration);
        }

        static Vector3[] RotateObject(Vector3[] ObjToRotate, Vector3 DegreesToRotate, Vector3 Around)//rotates a set of dots counterclockwise
        {
            for (int i = 0; i < ObjToRotate.Length; i++)
            {
                ObjToRotate[i] -= Around;
            }
            DegreesToRotate.z = Mathf.Deg2Rad * DegreesToRotate.z;
            DegreesToRotate.x = Mathf.Deg2Rad * DegreesToRotate.x;
            DegreesToRotate.y = -Mathf.Deg2Rad * DegreesToRotate.y;

            for (int i = 0; i < ObjToRotate.Length; i++)
            {
                float H = Vector3.Distance(Vector3.zero, ObjToRotate[i]);
                if (H != 0)
                {
                    float CosA = ObjToRotate[i].x / H;
                    float SinA = ObjToRotate[i].y / H;
                    float cosB = Mathf.Cos(DegreesToRotate.z);
                    float SinB = Mathf.Sin(DegreesToRotate.z);
                    ObjToRotate[i] = new Vector3(H * (CosA * cosB - SinA * SinB), H * (SinA * cosB + CosA * SinB), ObjToRotate[i].z);
                }
            }

            for (int i = 0; i < ObjToRotate.Length; i++)
            {
                float H = Vector3.Distance(Vector3.zero, ObjToRotate[i]);
                if (H != 0)
                {
                    float CosA = ObjToRotate[i].y / H;
                    float SinA = ObjToRotate[i].z / H;
                    float cosB = Mathf.Cos(DegreesToRotate.x);
                    float SinB = Mathf.Sin(DegreesToRotate.x);
                    ObjToRotate[i] = new Vector3(ObjToRotate[i].x, H * (CosA * cosB - SinA * SinB), H * (SinA * cosB + CosA * SinB));
                }
            }

            for (int i = 0; i < ObjToRotate.Length; i++)
            {
                float H = Vector3.Distance(Vector3.zero, ObjToRotate[i]);
                if (H != 0)
                {
                    float CosA = ObjToRotate[i].x / H;
                    float SinA = ObjToRotate[i].z / H;
                    float cosB = Mathf.Cos(DegreesToRotate.y);
                    float SinB = Mathf.Sin(DegreesToRotate.y);
                    ObjToRotate[i] = new Vector3((CosA * cosB - SinA * SinB) * H, ObjToRotate[i].y, H * (SinA * cosB + CosA * SinB));
                }
            }

            for (int i = 0; i < ObjToRotate.Length; i++)
            {
                ObjToRotate[i] += Around;
            }



            return ObjToRotate;
        }
    }
}

