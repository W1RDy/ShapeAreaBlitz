using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarningSignActivator : MonoBehaviour
{
    public static WarningSignActivator instance = null;
    [SerializeField] WarningSign warningSign;
    Transform objects;
    Transform container;
    Pool warningSignsPool;
    Dictionary<string, GameObject> warningSignsDictionary;

    private void Awake()
    {
        if (instance == null) instance = this;

        DontDestroyOnLoad(instance);
        instance.warningSignsPool = GameObject.Find("Pool").GetComponent<Pool>();
        instance.warningSignsDictionary = new Dictionary<string, GameObject>();
        instance.objects = GameObject.Find("Objects").transform;
    }

    private void Start()
    {
        instance.container = instance.warningSignsPool.transform.Find("WarningSignContainer").transform;
        if (instance != this) Destroy(this);
    }

    public void ActivateWarningSign(Vector3 position, float duration)
    {
        AudioManager.instance.PlaySound("Warning");
        var sign = warningSignsPool.GetPool(warningSign.GetType()).GetFreeElement();
        sign.transform.position = position;
        StartCoroutine(DestroyingCoroutine(duration, sign.gameObject));
    }

    private IEnumerator DestroyingCoroutine(float duration, GameObject warningSign)
    {
        yield return new WaitForSeconds(duration);
        warningSign.gameObject.SetActive(false);
    }

    public void ActivateWarningSign(Vector3 spawnPoint, string index)
    {
        AudioManager.instance.PlaySound("Warning");
        var sign = warningSignsPool.GetPool(warningSign.GetType()).GetFreeElement(); 
        sign.transform.position = spawnPoint;
        sign.transform.SetParent(objects);
        if (!warningSignsDictionary.ContainsKey(index)) warningSignsDictionary.Add(index, sign.gameObject);
    }

    public void ChangeWarningSignPos(string index, Vector3 newPos)
    {
        var sign = warningSignsDictionary[index].transform;
        if (sign.position != newPos) sign.position = newPos;
    }

    public void DeactivateWarningSign(string index)
    {
        if (warningSignsDictionary.ContainsKey(index))
        {
            var sign = warningSignsDictionary[index];
            sign.transform.SetParent(container);
            sign.transform.rotation = Quaternion.identity;
            warningSignsDictionary[index].gameObject.SetActive(false);
            warningSignsDictionary.Remove(index);
        }
    }
}
