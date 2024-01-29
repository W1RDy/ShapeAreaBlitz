using UnityEngine;

public interface IMaterialable 
{
    public void SetMaterial(MaterialConfig materialConfig);
    public Material GetMaterial(MaterialType materialType);

    public void SetColor(MaterialType materialType, string colorIndex, Color color);
    public void SetMaterialFloat(MaterialType materialType, string materialIndex, float value);
    public void SetMaterialInt(MaterialType materialType, string materialIndex, int value);
}
