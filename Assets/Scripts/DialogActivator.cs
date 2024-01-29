using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{
    public void ActivateDialog(string messageIndex)
    {
        DialogManager.instance.StartDialogBranch(messageIndex);
    }
}
