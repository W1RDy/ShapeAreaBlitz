using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OutlineManager : MonoBehaviour
{
    private List<(IMaterialable materialable, string type)> outlinedObjects;
    public static OutlineManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            outlinedObjects = new List<(IMaterialable materialable, string type)>();
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(instance);
    }

    public void OutlineObject(GameObject[] objectViews, bool isEnemy)
    {
        foreach (var objectView in objectViews)
        {
            var materialable = objectView.GetComponent<IMaterialable>();
            Vector3 scaleReference;
            float outlineScale = 5;
            try
            {
                var sprite = objectView.GetComponent<SpriteRenderer>();
                if (sprite == null) throw new System.Exception();

                if (isEnemy)
                {
                    scaleReference = objectView.transform.parent.GetComponent<MovableEnemy>().startScale;
                    outlineScale = 12 * Mathf.Min(1.3f / Mathf.Abs(scaleReference.x), 1.3f / Mathf.Abs(scaleReference.y)) * (sprite.sprite.pixelsPerUnit / 100);
                    materialable.SetMaterialInt(MaterialType.OutlineMaterial, "_OutlineEnabled", 1);
                }
                else
                {
                    objectView.GetComponent<HighlightArea>().ActivateArea();
                    outlineScale = 44;
                    outlinedObjects.Add((materialable, "Sprite"));
                }
            }
            catch
            {
                var outlineMaterial = MaterialManager.instance.GetMaterial("Outline");
                scaleReference = objectView.GetComponent<Image>().rectTransform.sizeDelta;
                outlineScale = 5 * Mathf.Max(180 / scaleReference.x, 133 / scaleReference.y);
                materialable.SetMaterial(outlineMaterial);
                outlinedObjects.Add((materialable, "Image"));
            }
            materialable.SetMaterialFloat(MaterialType.OutlineMaterial, "_OutlineThickness", outlineScale);
        }
    }

    public void HideOutline()
    {
        if (outlinedObjects.Count > 0)
        {
            foreach (var outlinedObject in outlinedObjects)
            {
                if (outlinedObject.type == "Image") outlinedObject.materialable.SetMaterial(null);
                else outlinedObject.materialable.SetMaterialInt(MaterialType.OutlineMaterial, "_OutlineEnabled", 0);
            }
            outlinedObjects.Clear();
        }
    }

    public bool ObjectsIsOutlined(IMaterialable materialable)
    {
        if (outlinedObjects.Count == 0) return false;
        return outlinedObjects[0].materialable == materialable;
    }
}
