using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketStageEvents : MonoBehaviour
{
    [FormerlySerializedAs("rocketLaunch")] public Rocket rocket;
    public Vector2 position;

    private float angleIncrement;

    private void Awake()
    {
        rocket = GetComponent<Rocket>();
    }


    private float _stage1Speed;
    
    private float _stage1InitialAngle;
    private float _stage1Angle;
    private float _stage1TargetAngle;
    
    public void Stage1Start(StageModel stage)
    {
        position = Vector2.zero;
        
        _stage1InitialAngle = stage.angleAtStageStart;
        _stage1Angle = _stage1InitialAngle;
        _stage1TargetAngle = stage.CalculateAdjustedAngle();
        
        float totalAngleDifference = Mathf.DeltaAngle(_stage1InitialAngle, _stage1TargetAngle);
        angleIncrement = totalAngleDifference / stage.GetStageDuration();
    }
    public void Stage1Update(StageModel stage)
    {
        Vector2 stageLaunchDirection = GetFlightDirection(_stage1Angle);
        _stage1Angle += angleIncrement * Time.fixedDeltaTime;
        
        _stage1Speed = rocket.CalculateSpeed(stage);
        
        position += stageLaunchDirection * (_stage1Speed * Time.fixedDeltaTime);
        transform.position = position;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -90f+_stage1Angle));

    }
    public void Stage1End(StageModel stage)
    {
        
    }


    private float _stage2Speed;
    
    private float _stage2InitialAngle;
    private float _stage2Angle;
    private float _stage2TargetAngle;
    public void Stage2Start(StageModel stage)
    {
        stage.angleAtStageStart = _stage1TargetAngle;
        
        _stage2InitialAngle = _stage1TargetAngle;
        _stage2Angle = _stage2InitialAngle;
        _stage2TargetAngle = stage.CalculateAdjustedAngle();
        
        float totalAngleDifference = Mathf.DeltaAngle(_stage2InitialAngle, _stage2TargetAngle);
        angleIncrement = totalAngleDifference / stage.GetStageDuration();
    }
    public void Stage2Update(StageModel stage)
    {
        Vector2 stageLaunchDirection = GetFlightDirection(_stage2Angle);
        _stage2Angle += angleIncrement * Time.fixedDeltaTime;
        
        _stage2Speed = rocket.CalculateSpeed(stage) + _stage1Speed;
        
        position += stageLaunchDirection * (_stage2Speed  * Time.fixedDeltaTime);
        transform.position = position;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -90f+_stage2Angle));
    }
    public void Stage2End(StageModel stage)
    {
        
        
    }


    
    // The speed which stage 3 should initially have if expected to reach 0 in reference time
    public float referenceStage3Speed = 5.5f;  
    // The time in seconds for referenceSpeed to reach 0
    public float referenceTime = 5f;   
    private float _currentStage3Speed;
 
    public float gravityAccelerationTime = 5f;
    private float _currentGravity;  
    private float _elapsedTimeGravity;  
    
    private float _stage3InitialAngle;
    private float _stage3Angle;
    
    public void Stage3Start(StageModel stage)
    {
        _currentStage3Speed = rocket.CalculateSpeed(stage) + _stage2Speed;

        speedDecrement = _currentStage3Speed - speedAtStageEnd / stage.GetStageDuration();
        speedDecrement *=  stage.mass / stage.referenceStageMass;
        
        _elapsedTimeGravity = 0f;
        _currentGravity = 0f;
        
        stage.angleAtStageStart = _stage2TargetAngle;
        _stage3InitialAngle = _stage2TargetAngle;
        _stage3Angle = _stage3InitialAngle;
        
        float totalAngleDifference = Mathf.DeltaAngle(_stage3InitialAngle, rocket.stage3TargetAngle);
        angleIncrement = totalAngleDifference / stage.GetStageDuration();
    }
    public void Stage3Update(StageModel stage)
    {
        DecreaseSpeed(stage);
        
        Vector2 stageLaunchDirection = GetFlightDirection(_stage3Angle);
        _stage3Angle += angleIncrement * Time.fixedDeltaTime;
        
        position += stageLaunchDirection * (_currentStage3Speed * Time.fixedDeltaTime); 
        transform.position = position;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -90f+_stage3Angle));

    }
    public void Stage3End(StageModel stage)
    { 
        Debug.Log("Stage 3 ended");
    }

    public float speedAtStageEnd = 3f;
    private float speedDecrement;

    private void DecreaseSpeed(StageModel stage)
    {
        _currentStage3Speed = Mathf.Max(_currentStage3Speed - speedDecrement, rocket.stage3MinimumSpeed);
    }
    
    float LerpTowardsAngle(float initialAngle, float targetAngle, float lerpAmount)
    {
        return Mathf.LerpAngle(initialAngle, targetAngle, lerpAmount);
    }
    
    private Vector2 GetFlightDirection(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }
}