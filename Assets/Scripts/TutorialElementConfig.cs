using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TutorialElementConfig 
{   
    [GUIColor(0, 1, 0)] public string elementName;
    public bool isNeedActivateSomething;
    [ShowIf(nameof(isNeedActivateSomething))] public TutorialActivatedObjectsType[] objectsTypes;

    [ShowIf(nameof(IsSameTypeDialog))] public string branchName;

    [ShowIf(nameof(IsSameTypeOutline))] public GameObject[] outlinedObjects;

    [ShowIfGroup(nameof(IsSameTypePointer))] public bool isPointerByDirection;
    [ShowIfGroup(nameof(IsSameTypePointer)), ShowIf(nameof(isPointerByDirection))] public DirectionType[] directionTypes;
    [ShowIfGroup(nameof(IsSameTypePointer)), HideIf(nameof(isPointerByDirection))] public string[] connectedObjectsIndexes;
    [ShowIfGroup(nameof(IsSameTypePointer)), HideIf(nameof(isPointerByDirection))] public bool isDodgeConnectedObject;

    [ShowIf(nameof(IsSameTypeCameZone))] public Transform[] camePlaces;

    [ShowIf(nameof(IsSameTypeButton))] public Button button;

    private bool IsSameTypeButton() => IsSameType(TutorialActivatedObjectsType.Button);

    private bool IsSameTypeCameZone() => IsSameType(TutorialActivatedObjectsType.CameZone);

    private bool IsSameTypeDialog() => IsSameType(TutorialActivatedObjectsType.Dialog);

    private bool IsSameTypePointer() => IsSameType(TutorialActivatedObjectsType.Pointer);

    private bool IsSameTypeOutline() => IsSameType(TutorialActivatedObjectsType.Outline);

    public bool IsSameType(TutorialActivatedObjectsType other)
    {
        if (objectsTypes == null) return false; 
        return objectsTypes.Contains(other);
    }
}
