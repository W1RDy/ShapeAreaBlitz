using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightArea : MonoBehaviour
{
    IMaterialable materialable;

    private void Awake()
    {
        materialable = GetComponent<IMaterialable>();
        var spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.materials = new Material[2] { spriteRenderer.material, MaterialManager.instance.GetMaterial("SpriteOutline").material};
        materialable.SetMaterialInt(MaterialType.OutlineMaterial, "_OutlineEnabled", 0);
        var color = MaterialManager.instance.GetMaterial("Outline").material.GetColor("_OutlineColor");
        materialable.SetColor(MaterialType.OutlineMaterial, "_OutlineColor", color);
    }

    public void ActivateArea()
    {
        materialable.SetMaterialInt(MaterialType.OutlineMaterial, "_OutlineEnabled", 1);
    }
}
