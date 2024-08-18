using System;
using UnityEngine;

public class RocketCollisionEvents : MonoBehaviour
{
    public static RocketCollisionEvents Instance;

    private Action <Rocket> _onCollidedWithTarget;
    private Action <Rocket> _onCollidedWithObstacle;
    
    void Awake()
    {
        Instance = this;
        _onCollidedWithTarget = null;
        _onCollidedWithObstacle = null;
    }

    public void OnCollidedWithTargetEvent(Rocket rocket, RocketTarget target)
    {
        Debug.Log("Rocket hit the target ! Name:" + target.name);
        _onCollidedWithTarget?.Invoke(rocket);
    }

    public void OnCollidedWithObstacleEvent(Rocket rocket, RocketObstacle obstacle)
    {
        Debug.Log("Rocket hit an obstacle ! Name:" + obstacle.name);
        _onCollidedWithObstacle?.Invoke(rocket);
    }

    public void AddOnCollidedWithTarget(Action<Rocket> onCollidedWithTarget)
    {
        _onCollidedWithTarget += onCollidedWithTarget;
    }

    public void AddOnCollidedWithObstacle(Action<Rocket> onCollidedWithObstacle)
    {
        _onCollidedWithObstacle += onCollidedWithObstacle;
    }

    private void OnDisable()
    {
        _onCollidedWithTarget = null;
        _onCollidedWithObstacle = null;
    }
}