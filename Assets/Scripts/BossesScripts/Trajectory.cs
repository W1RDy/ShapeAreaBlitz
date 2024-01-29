using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    LineRenderer trajectory;

    public void SetTrajectoryPoints(Vector2 startPos, Vector2 endPos)
    {
        trajectory = GetComponent<LineRenderer>();
        trajectory.SetPosition(0, startPos);
        trajectory.SetPosition(1, endPos);
    }
}
