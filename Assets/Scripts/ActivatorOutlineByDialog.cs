using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorOutlineByDialog : MonoBehaviour
{
    [SerializeField] MessageWithOutline[] messages;
    Dictionary<string, MessageWithOutline> messagesDict;

    private void Awake()
    {
        InitializeMessagesDictionary();
    }

    private void InitializeMessagesDictionary()
    {
        messagesDict = new Dictionary<string, MessageWithOutline>();

        foreach (var message in messages) messagesDict[message.messageIndex] = message;
    }

    public void OutlineObjects(string messageIndex)
    {
        if (messagesDict.ContainsKey(messageIndex))
        {
            var message = messagesDict[messageIndex];
            if (!OutlineManager.instance.ObjectsIsOutlined(message.outlinedObjects[0].GetComponent<IMaterialable>()))
            {
                OutlineManager.instance.HideOutline();
                OutlineManager.instance.OutlineObject(message.outlinedObjects, false);
            }
        }
        else OutlineManager.instance.HideOutline();
    }
}
