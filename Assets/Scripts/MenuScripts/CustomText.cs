using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomText : MonoBehaviour
{
    [SerializeField] string index;
    protected Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(index)) SetMessage(index);
    }

    public Text GetText()
    {
        return text;
    }

    public virtual void SetMessage(string index)
    {
        SetMessage(index, "");
    }

    public virtual void SetMessage(string index, string dynamicString)
    {
        string message = "" + dynamicString;
        if (index != "") message = PhraseDictionary.Instance.GetMessage(index) + dynamicString;
        if (text == null) text = GetComponent<Text>();
        text.text = message;
    }
}
