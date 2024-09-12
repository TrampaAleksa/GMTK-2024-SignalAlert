using UnityEngine;

public class RocketStageEventHandler : MonoBehaviour
{
    public Rocket rocket;
    public Vector2 Position { get; set; }

    private StageModel stage1 => rocket.stage1;
    private StageModel stage2 => rocket.stage2;
    private StageModel stage3 => rocket.stage3;

    private RocketStageEvent _firstStageEvents;
    private RocketStageEvent _secondStageEvents;
    private RocketStageEvent _thirdStageEvents;

    
    public void Init(Rocket rocketRef)
    {
        this.rocket = rocketRef;
        _firstStageEvents = new RSEv_FirstStageFlight().InjectRocket(rocketRef);
        _secondStageEvents = new RSEv_SecondStageFlight().InjectRocket(rocketRef);
        _thirdStageEvents = new RSEv_ThirdStageFlight().InjectRocket(rocketRef);

        stage1.OnStageStart = _firstStageEvents.StageStart;
        stage1.OnStageUpdate = _firstStageEvents.StageUpdate;
        stage1.OnStageEnd = _firstStageEvents.StageEnd;
        
        stage2.OnStageStart = _secondStageEvents.StageStart;
        stage2.OnStageUpdate = _secondStageEvents.StageUpdate;
        stage2.OnStageEnd = _secondStageEvents.StageEnd;
        
        stage3.OnStageStart = _thirdStageEvents.StageStart;
        stage3.OnStageUpdate = _thirdStageEvents.StageUpdate;
        stage3.OnStageEnd = _thirdStageEvents.StageEnd;

        // stage1.OnStageStart = Stage1Start;
        // stage1.OnStageUpdate = Stage1Update;
        // stage1.OnStageEnd = Stage1End;
        
        // stage2.OnStageStart = Stage2Start;
        // stage3.OnStageStart = Stage3Start;
        // stage2.OnStageUpdate = Stage2Update;
        // stage3.OnStageUpdate = Stage3Update;
        // stage2.OnStageEnd = Stage2End;
        // stage3.OnStageEnd = Stage3End;
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
        
        float totalAngleDifference = Mathf.DeltaAngle(_stage2InitialAngle, _stage2TargetAngle);
        _angleIncrement = totalAngleDifference / stage.GetStageDuration();
        
        _stage2Speed = rocket.CalculateSpeed(stage) + rocket.CurrentSpeed;
        rocket.CurrentSpeed = _stage2Speed;
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
        _stage3InitialAngle = rocket.CurrentAngle;

        _stage3Angle = _stage3InitialAngle;

        stage.angleAtStageStart = _stage3InitialAngle;
        var targetAngle = rocket.stage3TargetAngle;

        float totalAngleDifference = Mathf.DeltaAngle(_stage3InitialAngle, targetAngle);
        _angleIncrement = totalAngleDifference / stage.GetStageDuration();

        _stage3Speed = rocket.CalculateSpeed(stage) + rocket.CurrentSpeed;
        rocket.CurrentSpeed = _stage3Speed;
        _speedDecrement = _stage3Speed - speedAtStageEnd / stage.GetStageDuration();
        _speedDecrement *=  stage.mass / stage.referenceStageMass;
    }
    public void Stage3Update(StageModel stage)
    {
        DecreaseSpeed();
        
        Vector2 stageLaunchDirection = GetFlightDirection(_stage3Angle);
        
        _stage3Angle += _angleIncrement * Time.fixedDeltaTime;
        Position += stageLaunchDirection * (rocket.CurrentSpeed * Time.fixedDeltaTime);

        rocket.CurrentAngle = _stage3Angle;
        transform.position = Position;
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