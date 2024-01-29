using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsEffect : MonoBehaviour
{
    ParticleSystem _particleSystem;
    CustomText[] coinsCount;

    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        coinsCount = GetComponentsInChildren<CustomText>();
    }

    private void OnEnable()
    {
        _particleSystem.Play();
        StartCoroutine(DestroyingCoroutine(1));
    }

    public void ChangeCoinsText(float value)
    {
        foreach (var coinCount in coinsCount) coinCount.SetMessage("", "+" + value);
    }

    public void SetLimitText()
    {
        foreach (var coinCount in coinsCount) coinCount.SetMessage("Limit");
    }

    private IEnumerator DestroyingCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
