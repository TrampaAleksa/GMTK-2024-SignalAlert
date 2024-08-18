using UnityEngine;

public class RocketTarget : MonoBehaviour
{
    private Rocket _rocket;
    private void Awake()
    {
        _rocket = FindObjectOfType<Rocket>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter");
        if (other.gameObject.CompareTag("Rocket"))
        {
            RocketCollisionEvents.Instance.OnCollidedWithTargetEvent(_rocket, this);
        }
    }
}