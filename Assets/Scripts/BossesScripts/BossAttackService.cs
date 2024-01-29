using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackService : MonoBehaviour
{
    [SerializeField] AttackConfig[] attacks;
    Dictionary<string, AttackConfig> attacksDict;

    public static BossAttackService instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }

        DontDestroyOnLoad(instance);
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        attacksDict = new Dictionary<string, AttackConfig>();
        foreach (var attack in attacks) attacksDict[attack.index] = attack;
    }

    public IAttackable GetAttackable(string index) => attacksDict[index].attackableObject.GetComponent<IAttackable>();
}
