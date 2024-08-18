using UnityEngine;

public class RocketTarget : MonoBehaviour
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
            RocketCollisionEvents.Instance.OnCollidedWithTargetEvent(_rocketLaunch, this);
        }
    }
}