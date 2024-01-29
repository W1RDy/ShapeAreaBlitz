using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DialogBranch
{
    public string branchName;
    public MessageConfig[] messages;
    int messageIndexInQueue;

    public MessageConfig GetFirstMessage()
    {
        messageIndexInQueue = 0;
        return messages[messageIndexInQueue];
    }

    public MessageConfig GetNextMessage()
    {
        messageIndexInQueue++;
        if (messageIndexInQueue < messages.Length) return messages[messageIndexInQueue];
        return null;
    }
}
