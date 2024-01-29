using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangerPosByDialog : MonoBehaviour
{
    [SerializeField] Transform UIElement;
    [SerializeField] Vector3 newPos;
    Vector3 startPos;

    void Start()
    {
        startPos = UIElement.localPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Dialog") ChangePosition(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Dialog") ChangePosition(false);
    }

    private void ChangePosition(bool isActivateDialog)
    {
        if (isActivateDialog) UIElement.localPosition = newPos;
        else UIElement.localPosition = startPos;
    }
}
