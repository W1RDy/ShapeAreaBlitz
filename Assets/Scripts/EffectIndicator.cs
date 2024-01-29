using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EffectIndicator : MonoBehaviour
{
    public EffectType effectType;
    [SerializeField] public float duration;
    Image shieldImage;
    bool isInitialized = false;

    private void InitializeIndicator()
    {
        isInitialized = true;
        shieldImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void ActivateDeactivateIndicator(bool isActivate)
    {
        if (!isInitialized) InitializeIndicator();
        gameObject.SetActive(isActivate);
        if (isActivate) shieldImage.fillAmount = 1;
    }

    private void Update()
    {
        if (shieldImage.IsActive() && shieldImage.fillAmount > 0) 
            shieldImage.fillAmount -= 1 / duration * Time.deltaTime;
    }
}
