using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RetractableObjActivator : MonoBehaviour
{
    [SerializeField] GameObject IRetractableObject;

    [SerializeField] bool isManySpawnPlaces = true;
    [SerializeField, ShowIf(nameof(isManySpawnPlaces))] PointService blocksPointService;
    [SerializeField, HideIf(nameof(isManySpawnPlaces))] public Transform spawnPoint;

    [SerializeField] bool isBlockForHitActivator;
    [SerializeField, ShowIf(nameof(isBlockForHitActivator))] BlockForHitService blockForHitService;

    Transform RetractableObjPlace;
    IRetractable retractable;
    (GameObject _object, Transform point) currentObj;
    bool isActivated;
    bool isTutorial;
    PlayerMove player;

    private void Awake()
    {
        try { IRetractableObject.GetComponentInChildren<IRetractable>(); }
        catch { throw new System.InvalidOperationException("Object doesn't realize IRetractable interface!"); }
        RetractableObjPlace = GameObject.Find("Objects/Room/RetractableObjects").transform;
        isTutorial = GameObject.Find("GameService").GetComponent<GameService>().isTutorial;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }

    private void ActivateRetractableObj(float duration, float moveDelay, bool activateClosestToPlayer)
    {
        isActivated = true;
        var retractableObj = SpawnRetractableObject(activateClosestToPlayer);
        StartCoroutine(WaitActivatingAndDeactivating(duration, moveDelay, retractableObj));
    }

    public void ActivateClosestRetractableObj(float duration)
    {
        ActivateRetractableObj(duration, 0f, true);
    }

    public void ActivateRetractableObj(float duration, float moveDelay)
    {
        ActivateRetractableObj(duration, moveDelay, false);
    }

    public void ActivateRetractableObj(float duration)
    {
        ActivateRetractableObj(duration, 0f);
    }

    public void ChangeCurrentBlockForHit(BossType blockForHitType)
    {
        IRetractableObject = blockForHitService.GetBlockForHitObj(blockForHitType);
    }

    private GameObject SpawnRetractableObject(bool isSpawnClosestToPlayer)
    {
        if (isManySpawnPlaces)
            spawnPoint = isSpawnClosestToPlayer ? blocksPointService.GetClosestPoint(player.transform) : blocksPointService.GetRandomPoint();

        currentObj = (Instantiate(IRetractableObject, spawnPoint.position, spawnPoint.rotation), spawnPoint);
        currentObj._object.transform.SetParent(RetractableObjPlace);
        currentObj._object.transform.localScale = IRetractableObject.transform.localScale;
        retractable = currentObj._object.GetComponentInChildren<IRetractable>();
        return currentObj._object;
    }

    public void DestroyRetractableObj()
    {
        Destroy(currentObj._object);
        blocksPointService.DestroyPointConfig(currentObj.point.transform);
    }

    private IEnumerator WaitActivatingAndDeactivating(float duration, float moveDelay, GameObject retractableObj)
    {
        var retractable = retractableObj.GetComponentInChildren<IRetractable>();
        yield return new WaitForSeconds(moveDelay);
        retractable.ActivateDeactivateObject();
        yield return new WaitUntil(() => retractable == null || retractable.IsOnTargetPosition());
        yield return new WaitForSeconds(duration);
        if (retractableObj != null && !isTutorial)
        {
            retractable.ActivateDeactivateObject();
            yield return new WaitUntil(() => retractable == null || retractable.IsOnTargetPosition());
            Destroy(retractableObj);
        }
        isActivated = false;
    }

    public bool IsActivated() => isActivated;

    public Vector2 GetTargetPos()
    {
        if (retractable != null) return retractable.GetTargetPos();
        throw new System.NullReferenceException("Retractable object is destroyed or didn't spawn!");
    }

    public GameObject GetCurrentRetractableObject() => currentObj._object;
}
