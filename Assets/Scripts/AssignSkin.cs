using UnityEngine;

public class AssignSkin : MonoBehaviour
{
    private void Awake()
    {
        var skin = SkinManager.instance.GetEquippedSkin();
        GameObject player = Instantiate(skin.skinPrefab);
        player.transform.SetParent(transform);
    }
}
