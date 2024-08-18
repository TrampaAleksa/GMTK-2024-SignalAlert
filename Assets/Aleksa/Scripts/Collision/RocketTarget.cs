using UnityEngine;

public class RocketTarget : MonoBehaviour
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
            RocketCollisionEvents.Instance.OnCollidedWithTargetEvent(_rocketLaunch, this);
        }
    }
}