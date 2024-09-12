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
        
        _stage1InitialAngle = rocket.CurrentAngle;
        stage.angleAtStageStart = _stage1InitialAngle;
        
        _stage1TargetAngle = stage.CalculateAdjustedAngle();
        _stage1Angle = _stage1InitialAngle;
        
        float totalAngleDifference = Mathf.DeltaAngle(_stage1InitialAngle, _stage1TargetAngle);
        _angleIncrement = totalAngleDifference / stage.GetStageDuration();
        
        _stage1Speed = rocket.CalculateSpeed(stage);
        rocket.CurrentSpeed = _stage1Speed;
    }
    public void Stage1Update(StageModel stage)
    {
        Vector2 stageLaunchDirection = GetFlightDirection(_stage1Angle);
        _stage1Angle += _angleIncrement * Time.fixedDeltaTime;

        Position += stageLaunchDirection * (rocket.CurrentSpeed * Time.fixedDeltaTime);
        transform.position = Position;
        rocket.CurrentAngle = _stage1Angle;
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
        _stage2InitialAngle = rocket.CurrentAngle;
        stage.angleAtStageStart = _stage2InitialAngle;
        
        _stage2Angle = _stage2InitialAngle;
        _stage2TargetAngle = stage.CalculateAdjustedAngle();
        
        _stage2Speed = rocket.CalculateSpeed(stage) + rocket.CurrentSpeed;
        rocket.CurrentSpeed = _stage2Speed;
        
        float totalAngleDifference = Mathf.DeltaAngle(_stage2InitialAngle, _stage2TargetAngle);
        _angleIncrement = totalAngleDifference / stage.GetStageDuration();
    }
    public void Stage2Update(StageModel stage)
    {
        Vector2 stageLaunchDirection = GetFlightDirection(_stage2Angle);
        _stage2Angle += _angleIncrement * Time.fixedDeltaTime;
        
        Position += stageLaunchDirection * (rocket.CurrentSpeed  * Time.fixedDeltaTime);
        transform.position = Position;
        rocket.CurrentAngle = _stage2Angle;

    }
    public void Stage2End(StageModel stage)
    {
        
    }

    
    private float _stage3Speed;
    private float _speedDecrement;
    public float speedAtStageEnd = 3f;

    private float _stage3InitialAngle;
    private float _stage3Angle;
    
    public void Stage3Start(StageModel stage)
    {
        _stage3Speed = rocket.CalculateSpeed(stage) + rocket.CurrentSpeed;
        rocket.CurrentSpeed = _stage3Speed;

        _speedDecrement = _stage3Speed - speedAtStageEnd / stage.GetStageDuration();
        _speedDecrement *=  stage.mass / stage.referenceStageMass;
        
        _stage3InitialAngle = rocket.CurrentAngle;
        stage.angleAtStageStart = _stage3InitialAngle;
        
        _stage3Angle = _stage3InitialAngle;
        
        float totalAngleDifference = Mathf.DeltaAngle(_stage3InitialAngle, rocket.stage3TargetAngle);
        _angleIncrement = totalAngleDifference / stage.GetStageDuration();
    }
    public void Stage3Update(StageModel stage)
    {
        DecreaseSpeed();
        
        Vector2 stageLaunchDirection = GetFlightDirection(_stage3Angle);
        _stage3Angle += _angleIncrement * Time.fixedDeltaTime;
        
        Position += stageLaunchDirection * (rocket.CurrentSpeed * Time.fixedDeltaTime); 
        transform.position = Position;
        rocket.CurrentAngle = _stage3Angle;
    }
    public void Stage3End(StageModel stage)
    { 
        Debug.Log("Stage 3 ended");
    }

    private void DecreaseSpeed()
    {
        rocket.CurrentSpeed = Mathf.Max(rocket.CurrentSpeed - _speedDecrement, rocket.stage3MinimumSpeed);
    }

    private Vector2 GetFlightDirection(float angle)
    {
        return new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }
}