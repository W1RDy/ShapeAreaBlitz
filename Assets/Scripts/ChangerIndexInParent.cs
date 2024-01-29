using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerIndexInParent : MonoBehaviour
{
    [SerializeField] int index;

    private void Start()
    {
        transform.SetSiblingIndex(index);
    }
}
