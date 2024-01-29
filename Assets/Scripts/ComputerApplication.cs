using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerApplication : MonoBehaviour
{
    [SerializeField] GameObject highlightFrame;
    GameObject highlightFrameObj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mouse") highlightFrameObj = Instantiate(highlightFrame, transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Mouse") Destroy(highlightFrameObj);
    }
}
