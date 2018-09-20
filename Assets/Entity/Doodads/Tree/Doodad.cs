using UnityEngine;

public abstract class Doodad : MonoBehaviour
{
    public float DefaultOutlineWidth = 1.05f;
    public Color DefaultOutlineColor = Color.black;



    public void Awake()
    {
        ResetOutline();
    }

    public void DisableHighLight()
    {
        ResetOutline();
    }

    public void HighLight(Color color)
    {
        foreach (var ren in GameHelpers.GetAllRenderersForObject(gameObject))
        {
            if (ren.material.shader.name == "Custom/Outline")
            {
                ren.material.SetFloat("_OutlineWidth", 1.1f);
                ren.material.SetColor("_OutlineColor", color);
            }
        }
    }

    private void ResetOutline()
    {
        foreach (var ren in GameHelpers.GetAllRenderersForObject(gameObject))
        {
            if (ren.material.shader.name == "Custom/Outline")
            {
                ren.material.SetFloat("_OutlineWidth", DefaultOutlineWidth);
                ren.material.SetColor("_OutlineColor", DefaultOutlineColor);
            }
        }
    }
}