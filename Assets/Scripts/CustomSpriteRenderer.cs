using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CustomSpriteRenderer : MonoBehaviour, IMaterialable
{
    [HideInInspector] public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetMaterial(MaterialConfig materialConfig)
    {
        if (materialConfig == null) spriteRenderer.material = MaterialManager.instance.GetMaterial("Default").material;
        else spriteRenderer.material = materialConfig.material;
    }

    public Material GetMaterial(MaterialType materialType)
    {
        var index = TransformMaterialTypeToInt(materialType);
        return spriteRenderer.materials[index];
    }

    public void SetColor(MaterialType materialType, string colorIndex, Color color)
    {
        var index = TransformMaterialTypeToInt(materialType);
        spriteRenderer.materials[index].SetColor(colorIndex, color);
    }

    public void SetMaterialFloat(MaterialType materialType, string materialIndex, float value)
    {
        var index = TransformMaterialTypeToInt(materialType);
        spriteRenderer.materials[index].SetFloat(materialIndex, value);
    }

    public void SetMaterialInt(MaterialType materialType, string materialIndex, int value)
    {
        var index = TransformMaterialTypeToInt(materialType);
        spriteRenderer.materials[index].SetInt(materialIndex, value);
    }

    private int TransformMaterialTypeToInt(MaterialType materialType)
    {
        var index = (int)materialType;
        if (index > spriteRenderer.materials.Length - 1) return 0;
        return index;
    }
}
