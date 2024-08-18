using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketStageEvents : MonoBehaviour
{
    [FormerlySerializedAs("rocketLaunch")] public Rocket rocket;
    public Vector2 position;

    private void Awake()
    {
        rocket = GetComponent<Rocket>();
    }
    
    
    public void Stage1Start(StageModel stage)
    {
        position = Vector2.zero;
    }
    public void Stage1Update(StageModel stage)
    {
        Vector2 stageLaunchDirection = rocket.GetFlightDirection(stage);
        float speed = rocket.CalculateSpeed(stage);
        
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
        Vector2 stageLaunchDirection = rocket.GetFlightDirection(stage);
        float speed = rocket.CalculateSpeed(stage);
        
        position += stageLaunchDirection * (speed * Time.fixedDeltaTime);
        transform.position = position;
    }
    public void Stage2End(StageModel stage)
    {
        _currentStage3Speed = rocket.CalculateSpeed(stage);
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
        Vector2 stageLaunchDirection = rocket.GetFlightDirection(stage);
        
        _elapsedTimeGravity += Time.fixedDeltaTime;
        _currentGravity = Mathf.Lerp(0, rocket.gravityStrength, _elapsedTimeGravity / gravityAccelerationTime);
        
        position += stageLaunchDirection * (_currentStage3Speed * Time.fixedDeltaTime); 
        position += Vector2.down * (_currentGravity * Time.fixedDeltaTime);
        
        transform.position = position;
    }
    public void Stage3End(StageModel stage)
    { 
        Debug.Log("Stage 3 ended");
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