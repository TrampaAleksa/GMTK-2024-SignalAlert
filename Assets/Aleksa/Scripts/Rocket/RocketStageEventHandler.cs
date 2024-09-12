using UnityEngine;

public class RocketStageEventHandler : MonoBehaviour
{
    public Rocket rocket;
    public Vector2 Position { get; set; }

    private StageModel stage1 => rocket.stage1;
    private StageModel stage2 => rocket.stage2;
    private StageModel stage3 => rocket.stage3;

    public RocketStageEvent firstStageEvents;

    
    public void Init(Rocket rocketRef)
    {
        this.rocket = rocketRef;
        firstStageEvents = new RSEv_FirstStageFlight().InjectRocket(rocketRef);

        // stage1.OnStageStart = firstStageEvents.StageStart;
        // stage1.OnStageUpdate = firstStageEvents.StageUpdate;
        // stage1.OnStageEnd = firstStageEvents.StageEnd;

        stage1.OnStageStart = Stage1Start;
        stage1.OnStageUpdate = Stage1Update;
        stage1.OnStageEnd = Stage1End;
        
        stage2.OnStageStart = Stage2Start;
        stage3.OnStageStart = Stage3Start;
        stage2.OnStageUpdate = Stage2Update;
        stage3.OnStageUpdate = Stage3Update;
        stage2.OnStageEnd = Stage2End;
        stage3.OnStageEnd = Stage3End;
    }


    private float _stage1Speed;
    
    private float _angleIncrement;
    private float _stage1InitialAngle;
    private float _stage1Angle;
    private float _stage1TargetAngle;
    
    public void Stage1Start(StageModel stage)
    {
        Position = Vector2.zero;
        
        _stage1InitialAngle = stage.angleAtStageStart;
        _stage1Angle = _stage1InitialAngle;
        _stage1TargetAngle = stage.CalculateAdjustedAngle();
        
        float totalAngleDifference = Mathf.DeltaAngle(_stage1InitialAngle, _stage1TargetAngle);
        _angleIncrement = totalAngleDifference / stage.GetStageDuration();
    }
    public void Stage1Update(StageModel stage)
    {
        Vector2 stageLaunchDirection = GetFlightDirection(_stage1Angle);
        _stage1Angle += _angleIncrement * Time.fixedDeltaTime;
        
        _stage1Speed = rocket.CalculateSpeed(stage);
        
        Position += stageLaunchDirection * (_stage1Speed * Time.fixedDeltaTime);
        transform.position = Position;
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
        _angleIncrement = totalAngleDifference / stage.GetStageDuration();
    }
    public void Stage2Update(StageModel stage)
    {
        Vector2 stageLaunchDirection = GetFlightDirection(_stage2Angle);
        _stage2Angle += _angleIncrement * Time.fixedDeltaTime;
        
        _stage2Speed = rocket.CalculateSpeed(stage) + _stage1Speed;
        
        Position += stageLaunchDirection * (_stage2Speed  * Time.fixedDeltaTime);
        transform.position = Position;
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
        _angleIncrement = totalAngleDifference / stage.GetStageDuration();
    }
    public void Stage3Update(StageModel stage)
    {
        DecreaseSpeed(stage);
        
        Vector2 stageLaunchDirection = GetFlightDirection(_stage3Angle);
        _stage3Angle += _angleIncrement * Time.fixedDeltaTime;
        
        Position += stageLaunchDirection * (_currentStage3Speed * Time.fixedDeltaTime); 
        transform.position = Position;
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