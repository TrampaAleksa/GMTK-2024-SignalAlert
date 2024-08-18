using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Button launchButton;
    
    public Rocket rocket;
    public int launchAttempts = 3;
    public bool randomizeTarget;
    
    private void Start()
    {
        if (randomizeTarget)
            SpawnTargetAtRandomSpot();
        
        RocketCollisionEvents.Instance.AddOnCollidedWithObstacle(OnRocketHitObstacle);
        RocketCollisionEvents.Instance.AddOnCollidedWithTarget(OnRocketHitTarget);
        
        launchButton.onClick.AddListener(() =>
        {
            if (launchAttempts <= 0)
            {
                Debug.Log("Can't launch no attempts left");
                return;
            }

            launchAttempts--;
            rocket.Launch();
        });
    }

    private void OnRocketHitObstacle(Rocket r)
    {
        if (launchAttempts <= 0)
        {
            GameOver();
        }
    }

    private void OnRocketHitTarget(Rocket r)
    {
        Debug.Log("Level passed");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload scene
    }

    private void SpawnTargetAtRandomSpot()
    {
        var target = FindObjectOfType<RocketTarget>();
        
        if (target != null)
            target.transform.position = new Vector3(Random.Range(30f, 90f), Random.Range(10f, 35f), 0f);
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload scene
    }
}