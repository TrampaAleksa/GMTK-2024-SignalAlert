using UnityEngine;

public class RocketObstacle : MonoBehaviour
{
    private RocketLaunch _rocketLaunch;
    private void Awake()
    {
        _rocketLaunch = FindObjectOfType<RocketLaunch>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter");
        if (other.gameObject.CompareTag("Rocket"))
        {
            RocketCollisionEvents.Instance.OnCollidedWithObstacleEvent(_rocketLaunch, this);
        }
    }
}