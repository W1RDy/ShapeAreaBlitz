using System.Collections;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    [SerializeField] HeartUI[] hearts;

    public void ChangeHealthIndicator(int health)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health) hearts[i].ChangeHeart(false);
            else hearts[i].ChangeHeart(true);
        }
    }
}
