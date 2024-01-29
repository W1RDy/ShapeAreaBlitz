using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyrighterInRoom : MonoBehaviour
{
    Transform room;
    GameObject copy;

    private void Awake()
    {
        room = GameObject.Find("Objects/Room").transform;
        if (transform.parent != room) CopyInRoom();
    }

    private void CopyInRoom()
    {
        copy = Instantiate(gameObject, room);
        Destroy(copy.GetComponent<CopyrighterInRoom>());
    }

    public GameObject GetCopy() => copy;
}
