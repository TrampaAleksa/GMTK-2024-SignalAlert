using System;
using UnityEngine;

public class RocketStageEvents : MonoBehaviour
{
    public RocketLaunch rocketLaunch;
    private Vector2 position;

    private void Awake()
    {
        rocketLaunch = GetComponent<RocketLaunch>();
    }
    
    
    public void Stage1Start(StageModel stage)
    {
        position = Vector2.zero;
    }
    public void Stage1Update(StageModel stage)
    {
        Vector2 stageLaunchDirection = rocketLaunch.GetFlightDirection(stage);
        float speed = rocketLaunch.CalculateSpeed(stage);
        
        position += stageLaunchDirection * (speed * Time.deltaTime);
        transform.position = position;
    }
    public void Stage1End(StageModel stage)
    {
        
    }
    
    
    
    public void Stage2Start(StageModel stage)
    {
        
    }
    public void Stage2Update(StageModel stage)
    {
        Vector2 stageLaunchDirection = rocketLaunch.GetFlightDirection(stage);
        float speed = rocketLaunch.CalculateSpeed(stage);
        
        position += stageLaunchDirection * (speed * Time.deltaTime);
        transform.position = position;
    }
    public void Stage2End(StageModel stage)
    {
        currentStage3Speed = rocketLaunch.CalculateSpeed(stage);
    }


    
    private float currentStage3Speed;
    private float thrustDecayRate;
    
    public void Stage3Start(StageModel stage)
    {
        float relativeMassFactor = stage.mass / stage.referenceStageMass; 
        thrustDecayRate = (stage.referenceStageMass / currentStage3Speed) * relativeMassFactor;
        
        Debug.Log("Thrust decay rate: " + thrustDecayRate);
        Debug.Log("Current stage 3 speed: " + currentStage3Speed);
    }
    public void Stage3Update(StageModel stage)
    {
        DecreaseSpeed(stage);
        Debug.Log("Current stage 3 speed: " + currentStage3Speed);
        Vector2 stageLaunchDirection = rocketLaunch.GetFlightDirection(stage);
        
        // TODO -- Additive gravity factor for more realistic descent
        position += stageLaunchDirection * (currentStage3Speed * Time.deltaTime) + Vector2.down * (rocketLaunch.gravityStrength * Time.deltaTime);
        transform.position = position;
    }
    public void Stage3End(StageModel stage)
    { 

    }
    
    // The speed which stage 3 should initially have if expected to reach 0 in reference time
    public float referenceStage3Speed = 5.5f;  
    // The time in seconds for referenceSpeed to reach 0
    public float referenceTime = 5f;    
    private void DecreaseSpeed(StageModel stage)
    {
        float referenceDecrementPerSecond = referenceStage3Speed / referenceTime;
        float adjustedDecrementPerSecond = referenceDecrementPerSecond * (stage.referenceStageMass / stage.mass);

        float speedDecrementPerFrame = adjustedDecrementPerSecond * Time.deltaTime;

        currentStage3Speed -= speedDecrementPerFrame;
        currentStage3Speed = Mathf.Max(currentStage3Speed, 0f);
    }
}