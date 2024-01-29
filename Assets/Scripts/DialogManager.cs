using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DialogManager : MonoBehaviour
{
    [SerializeField] DialogBranch[] dialogBranches;
    [SerializeField] MessageField messageField;

    public static DialogManager instance;
    MessageConfig currentMessage;
    Transform canvas;
    Timer timer;
    DialogBranch currentDialogBranch;
    MessageField currentMessageField;
    TouchReader touchReader;
    ActivatorOutlineByDialog outlineActivator;
    bool isFinished;
    bool isStopped;

    Dictionary<string, DialogBranch> branchesDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitializeBranchesDictionary();
        }
        else Destroy(gameObject);

        DontDestroyOnLoad(instance);

        instance.canvas = GameObject.Find("Canvas").transform;
        instance.touchReader = GameObject.Find("Canvas/TouchReader").GetComponent<TouchReader>();
        try { instance.timer = instance.canvas.Find("UIPanel/TimerPanel/Timer").GetComponent<Timer>(); }
        catch { }

        try { instance.outlineActivator = GameObject.Find("ActivatorOutlineByDialog").GetComponent<ActivatorOutlineByDialog>(); }
        catch { }
    }

    private void InitializeBranchesDictionary()
    {
        branchesDictionary = new Dictionary<string, DialogBranch>();

        foreach (var dialogBranch in dialogBranches) branchesDictionary[dialogBranch.branchName] = dialogBranch;
    }

    public void StartDialogBranch(string dialogBranchName)
    {
        isFinished = false;
        isStopped = false;
        touchReader.EnableTouchZone();
        if (timer) timer.StopTimer();

        currentDialogBranch = branchesDictionary[dialogBranchName];
        currentMessageField = Instantiate(messageField.gameObject, canvas).GetComponent<MessageField>();
        currentMessageField.transform.SetSiblingIndex(canvas.childCount - 3);
        currentMessage = currentDialogBranch.GetFirstMessage();
        currentMessageField.SetMessage(currentMessage);

        OutlineObjects();
        if (UnavailableDirectionsManager.instance) UnavailableDirectionsManager.instance.AddAllDirectionsExcept(null);
    }

    public void ShowNextMessage()
    {
        isStopped = false;
        touchReader.EnableTouchZone();
        currentMessage = currentDialogBranch.GetNextMessage();
        if (currentMessage == null)
        {
            HideDialogField();
            return;
        }
        else
        {
            currentMessageField.SetMessage(currentMessage);
            OutlineObjects();
        }

        if (currentMessage.isMessageForAction)
        {
            touchReader.DisableTouchZone();
            isStopped = true;
        }
    }

    private void OutlineObjects()
    {
        if (outlineActivator) outlineActivator.OutlineObjects(currentMessage.index);
    }

    public void HideDialogField()
    {
        if (currentMessageField)
        {
            isFinished = true;
            if (UnavailableDirectionsManager.instance) UnavailableDirectionsManager.instance.ClearAllUnavailableDirections();
            touchReader.DisableTouchZone();
            if (timer) timer.StartTimer();
            Destroy(currentMessageField.gameObject);
        }
    }

    public bool DialogIsFinished() => isFinished;

    public bool DialogIsStopped() => isStopped;

    public string GetCurrentMessageIndex() => currentMessage.index;
}
