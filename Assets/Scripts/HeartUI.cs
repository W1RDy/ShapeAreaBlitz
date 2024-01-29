using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeHeart(bool isTakeDamage)
    {
        animator.SetBool("Hit", isTakeDamage);
    }
}
