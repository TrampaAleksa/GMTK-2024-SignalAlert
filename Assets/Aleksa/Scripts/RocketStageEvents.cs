using System;
using UnityEngine;

public class RocketStageEvents : MonoBehaviour
{
    public RocketLaunch rocketLaunch;
    public Vector2 position;

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
        
        position += stageLaunchDirection * (speed * Time.fixedDeltaTime);
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
        
        position += stageLaunchDirection * (speed * Time.fixedDeltaTime);
        transform.position = position;
    }
    public void Stage2End(StageModel stage)
    {
        _currentStage3Speed = rocketLaunch.CalculateSpeed(stage);
    }


    
    // The speed which stage 3 should initially have if expected to reach 0 in reference time
    public float referenceStage3Speed = 5.5f;  
    // The time in seconds for referenceSpeed to reach 0
    public float referenceTime = 5f;   
    private float _currentStage3Speed;
 
    public float gravityAccelerationTime = 5f;
    private float _currentGravity;  
    private float _elapsedTimeGravity;  
    
    public void Stage3Start(StageModel stage)
    {
        _elapsedTimeGravity = 0f;
        _currentGravity = 0f;
    }
    public void Stage3Update(StageModel stage)
    {
        DecreaseSpeed(stage);
        Vector2 stageLaunchDirection = rocketLaunch.GetFlightDirection(stage);
        
        _elapsedTimeGravity += Time.fixedDeltaTime;
        _currentGravity = Mathf.Lerp(0, rocketLaunch.gravityStrength, _elapsedTimeGravity / gravityAccelerationTime);
        
        position += stageLaunchDirection * (_currentStage3Speed * Time.fixedDeltaTime); 
        position += Vector2.down * (_currentGravity * Time.fixedDeltaTime);
        
        transform.position = position;
    }
    public void Stage3End(StageModel stage)
    { 
    }
    


    private void DecreaseSpeed(StageModel stage)
    {
        float referenceDecrementPerSecond = referenceStage3Speed / referenceTime;
        float adjustedDecrementPerSecond = referenceDecrementPerSecond * (stage.referenceStageMass / stage.mass);

        float speedDecrementPerFrame = adjustedDecrementPerSecond * Time.fixedDeltaTime;
        
        _currentStage3Speed -= speedDecrementPerFrame;
        _currentStage3Speed = Mathf.Max(_currentStage3Speed, 0f);
    }
}