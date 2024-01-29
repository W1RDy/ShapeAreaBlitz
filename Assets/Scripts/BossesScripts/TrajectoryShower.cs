using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryShower : MonoBehaviour
{
    public static TrajectoryShower instance = null;

    [SerializeField] Trajectory trajectory;
    bool isShow;

    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            instance.isShow = false;
            Destroy(gameObject);
        }

        DontDestroyOnLoad(instance);
    }

    public void ShowTrajectory(Vector2 startPos,  Vector2 endPos, float duration)
    {
        isShow = true;
        var currentTrajectory = Instantiate(trajectory.gameObject).GetComponent<Trajectory>();
        currentTrajectory.SetTrajectoryPoints(startPos, endPos);
        StartCoroutine(WaitTrajectory(duration, currentTrajectory));
    }

    private IEnumerator WaitTrajectory(float duration, Trajectory trajectory)
    {
        yield return new WaitForSeconds(duration);
        Destroy(trajectory.gameObject);
        isShow = false;
    }

    public bool TrajectoryIsShowed() => isShow;
}
