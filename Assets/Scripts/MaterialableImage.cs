using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialableImage : Image, IMaterialable
{
    public void SetMaterial(MaterialConfig materialConfig)
    {
        if (materialConfig == null) material = null;
        else material = materialConfig.material;
    }

    public Material GetMaterial(MaterialType materialType)
    {
        return material;
    }

    public void SetColor(MaterialType materialType, string colorIndex, Color color)
    {
        material.SetColor(colorIndex, color);
    }

    public void SetMaterialFloat(MaterialType materialType, string materialIndex, float value)
    {
        material.SetFloat(materialIndex, value);
    }

    public void SetMaterialInt(MaterialType materialType, string materialIndex, int value)
    {
        material.SetInt(materialIndex, value);
    }
}
