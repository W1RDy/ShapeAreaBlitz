using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] TutorialElementConfig[] elementsOrder;
    [SerializeField] TutorialObjectsService tutorialObjectsService;
    [SerializeField] GameService gameService;
    [SerializeField] Button[] menuButtons;
    TutorialDelayer delayer;
    int currentElementInOrder = 0;
    bool isTutorial;
    int countElementWithStoppedDialog;

    private void Awake()
    {
        isTutorial = DataContainer.Instance.playerData.isNeedTutorial;
        delayer = GetComponent<TutorialDelayer>();
    }

    private void Start()
    {
        if (isTutorial) StartTutorial();
    }

    private void StartTutorial() => ShowTutorialElement();

    public void ShowTutorialElement()
    {
        TutorialElementConfig tutorialElement;
        if (currentElementInOrder < elementsOrder.Length) tutorialElement = elementsOrder[currentElementInOrder];
        else tutorialElement = null;

        if (tutorialElement != null) ActivateObject(tutorialElement);
        DeactivateObject(tutorialElement);
    }

    private void ActivateObject(TutorialElementConfig tutorialElement)
    {
        foreach (var type in tutorialElement.objectsTypes)
        {
            var activateAction = tutorialObjectsService.GetEnabledAction(type);
            activateAction?.Invoke(tutorialElement);
        }
    }

    private void DeactivateObject(TutorialElementConfig tutorialElement)
    {
        if (tutorialElement == null || !tutorialElement.IsSameType(TutorialActivatedObjectsType.Pointer))
           if (tutorialObjectsService.currentPointer != null) tutorialObjectsService.currentPointer.StopPoint();
        if (tutorialElement == null || (!tutorialElement.IsSameType(TutorialActivatedObjectsType.Outline) && !tutorialElement.IsSameType(TutorialActivatedObjectsType.Dialog))) 
           OutlineManager.instance.HideOutline();
    }

    public void ShowNextElement()
    {
        bool dialogIsStopped = DialogManager.instance.DialogIsStopped();
        if ((dialogIsStopped && countElementWithStoppedDialog == 0) || !dialogIsStopped)
        {
            currentElementInOrder++;
            ShowTutorialElement();
        }
        else
        {
            countElementWithStoppedDialog = 0;
            DialogManager.instance.ShowNextMessage();
            delayer.DelayForTutorialElement(null, TutorialActivatedObjectsType.Dialog);
            return;
        }

        if (dialogIsStopped) countElementWithStoppedDialog++;
    }
}
