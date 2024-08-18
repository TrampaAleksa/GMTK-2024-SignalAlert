using UnityEngine;

public class RocketObstacle : MonoBehaviour
{
    private RocketLaunch _rocketLaunch;
    private void Awake()
    {
        _rocketLaunch = FindObjectOfType<RocketLaunch>();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Rocket")
        {
            RocketCollisionEvents.Instance.OnCollidedWithObstacleEvent(_rocketLaunch, this);
        }
    }
}