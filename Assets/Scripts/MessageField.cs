using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageField : MonoBehaviour
{
    [SerializeField] Image reych;
    Text message;

    public void SetMessage(MessageConfig messageConfig)
    {
        var text = PhraseDictionary.Instance.GetMessage(messageConfig.index);
        if (message == null) message = GetComponentInChildren<Text>();
        message.text = text;
        ChangeReych(messageConfig.reychType);
    }

    private void ChangeReych(ReychType type)
    {
        reych.sprite = ReychManager.instance.GetReychSprite(type);
    }
}
