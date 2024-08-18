using System;
using UnityEngine;

public class RocketCollisionEvents : MonoBehaviour
{
    public static RocketCollisionEvents Instance;

    private Action <RocketLaunch> _onCollidedWithTarget;
    private Action <RocketLaunch> _onCollidedWithObstacle;
    
    void Awake()
    {
        Instance = this;
        _onCollidedWithTarget = null;
        _onCollidedWithObstacle = null;
    }

    public void OnCollidedWithTargetEvent(RocketLaunch rocketLaunch, RocketTarget target)
    {
        Debug.Log("Rocket hit the target ! Name:" + target.name);
        _onCollidedWithTarget?.Invoke(rocketLaunch);
    }

    public void OnCollidedWithObstacleEvent(RocketLaunch rocketLaunch, RocketObstacle obstacle)
    {
        Debug.Log("Rocket hit an obstacle ! Name:" + obstacle.name);
        _onCollidedWithObstacle?.Invoke(rocketLaunch);
    }

    public void AddOnCollidedWithTarget(Action<RocketLaunch> onCollidedWithTarget)
    {
        _onCollidedWithTarget += onCollidedWithTarget;
    }

    public void AddOnCollidedWithObstacle(Action<RocketLaunch> onCollidedWithObstacle)
    {
        _onCollidedWithObstacle += onCollidedWithObstacle;
    }

    private void OnDisable()
    {
        _onCollidedWithTarget = null;
        _onCollidedWithObstacle = null;
    }
}