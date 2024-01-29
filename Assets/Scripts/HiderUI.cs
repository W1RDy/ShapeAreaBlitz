using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiderUI : MonoBehaviour
{
    List<Collider2D> _colliders;
    bool isHided;
    public event Action<float> HideObject;

    private void Awake()
    {
        _colliders = new List<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _colliders.Add(collision);
        HideShowUI(true);
        StartCoroutine(CheckCollider());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _colliders.Remove(collision); ;
        if (_colliders.Count == 0) HideShowUI(false);
    }

    private IEnumerator CheckCollider()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (!isHided) break;

            if (!IsHaveCollidedObj())
            {
                _colliders = new List<Collider2D>();
                HideShowUI(false);
                break;
            }
        }
    }

    private bool IsHaveCollidedObj()
    {
        foreach (var collider in _colliders)
            if (collider.gameObject.activeInHierarchy) return true;
        return false;
    }

    private void HideShowUI(bool isHide)
    {
        isHided = isHide;
        float alphaValue = isHide ? 0.3f : 1f;
        HideObject?.Invoke(alphaValue);
    }
}
