using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChangeOpaqueToTransparent : MonoBehaviour
{
    public Material material;
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        material.color = Color.white;
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

        material.color = new Color(material.color.r, material.color.g, material.color.b,0.5f);
        GetComponent<MeshRenderer>().material = material;
    }

}
