using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UnavailableDirectionsManager : MonoBehaviour
{
    public static UnavailableDirectionsManager instance;

    [SerializeField] List<DirectionType> unavailableDirections;
    [SerializeField] Dictionary<string, Vector2> normals;
    Transform room;
    bool isActivated = true;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(instance);

        instance.unavailableDirections = new List<DirectionType>();
        instance.normals = new Dictionary<string, Vector2>();
        instance.room = GameObject.Find("Objects/Room").transform;
    }

    private void AddRemoveUnavailableDirections(DirectionType type, bool isAdd)
    {
        if (isActivated)
        {
            if (isAdd) unavailableDirections.Add(type);
            else unavailableDirections.Remove(type);
        }
    }

    public void UpdateUnavailableDirections()
    {
        if (unavailableDirections.Count > 0 && isActivated)
        {
            ClearAllUnavailableDirections();
            foreach (var normal in normals.Values)
                SetRemoveDirectionType(normal, true, true);
        }
    }

    private void SetRemoveDirectionType(Vector2 direction, bool isSet, bool isUpdate)
    {
        direction = new Vector2(-direction.x, -direction.y);
        if (!isSet || isUpdate) direction = room.TransformDirection(direction);
        var directionType = DirectionService.GetDirectionType(direction);
        if (directionType == null) throw new NullReferenceException("Side is incorrect!");
        AddRemoveUnavailableDirections(directionType.Value, isSet);
    }

    public void ClearAllUnavailableDirections()
    {
        unavailableDirections.Clear();
    }

    private void AddCurrentUnavailableDirections()
    {
        foreach (var normal in normals.Values)
            SetRemoveDirectionType(normal, true, false);
    }

    public void SetCollision(Collision2D collision)
    {
        var contact = collision.GetContact(0);
        var normal = room.InverseTransformDirection(contact.normal);
        normals.Add(collision.gameObject.name, normal);
        if (isActivated) SetRemoveDirectionType(contact.normal, true, false);
    }

    public void RemoveCollision(Collision2D collision)
    {
        var direction = normals[collision.gameObject.name];
        if (isActivated) SetRemoveDirectionType(direction, false, false);
        normals.Remove(collision.gameObject.name);
    }

    public bool IsUnavailableDirection(DirectionType directionType)
    {
        return unavailableDirections.Contains(directionType);
    }

    public void ActivateDeactivateUnavailableDirections(bool isActivate)
    {
        isActivated = isActivate;
        if (!isActivated) ClearAllUnavailableDirections();
        else AddCurrentUnavailableDirections();
    }

    public void AddAllDirectionsExcept(DirectionType? directionType)
    {
        if (directionType != null && IsUnavailableDirection(directionType.Value)) unavailableDirections.Remove(directionType.Value);
        if (unavailableDirections.Count <= 3)
        {
            for (int i = 0; i < (int)DirectionType.Down + 1; i++)
            {
                if (!IsUnavailableDirection((DirectionType)i) && (directionType == null || i != (int)directionType))
                    AddRemoveUnavailableDirections((DirectionType)i, true);
            }
        }
    }
}
