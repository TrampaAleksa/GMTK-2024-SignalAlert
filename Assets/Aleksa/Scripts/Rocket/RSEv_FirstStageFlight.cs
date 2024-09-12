using UnityEngine;

public class RSEv_FirstStageFlight : RocketStageEvent
{
    private float _stageSpeed;
    
    private float _angleIncrement;
    private float _initialAngle;
    private float _targetAngle;

    private float _currentStageAngle;
    private Vector2 _currentStagePosition;
    
    public override void StageStart(StageModel stage)
    {
        Rocket.CurrentSpeed = 0;
        _initialAngle = Rocket.CurrentAngle;
        
        _currentStagePosition = Rocket.transform.position;
        _currentStageAngle = _initialAngle;

        stage.angleAtStageStart = _initialAngle;
        _targetAngle = stage.CalculateAdjustedAngle();

        float totalAngleDifference = Mathf.DeltaAngle(_initialAngle, _targetAngle);
        _angleIncrement = totalAngleDifference / stage.GetStageDuration();
        
        _stageSpeed = Rocket.CalculateSpeed(stage);
        Rocket.CurrentSpeed = _stageSpeed;
    }

    public override void StageUpdate(StageModel stage)
    {
        Vector2 stageFlightDirection = _currentStageAngle.ToFlightDirection();
        
        _currentStageAngle += _angleIncrement * Time.fixedDeltaTime;
        _currentStagePosition += stageFlightDirection * (Rocket.CurrentSpeed * Time.fixedDeltaTime);

        Rocket.CurrentAngle = _currentStageAngle;
        Rocket.transform.position = _currentStagePosition;
    }

    public override void StageEnd(StageModel stage)
    {
        
    }
}

public class RSEv_SecondStageFlight : RocketStageEvent
{
    private float _stageSpeed;
    
    private float _angleIncrement;
    private float _initialAngle;
    private float _targetAngle;

    private float _currentStageAngle;
    private Vector2 _currentStagePosition;
    
    public override void StageStart(StageModel stage)
    {
        _initialAngle = Rocket.CurrentAngle;
        
        _currentStagePosition = Rocket.transform.position;
        _currentStageAngle = _initialAngle;
        
        stage.angleAtStageStart = _initialAngle;
        _targetAngle = stage.CalculateAdjustedAngle();

        float totalAngleDifference = Mathf.DeltaAngle(_initialAngle, _targetAngle);
        _angleIncrement = totalAngleDifference / stage.GetStageDuration();
        
        _stageSpeed = Rocket.CalculateSpeed(stage) + Rocket.CurrentSpeed;
        Rocket.CurrentSpeed = _stageSpeed;
    }

    public override void StageUpdate(StageModel stage)
    {
        Vector2 stageFlightDirection = _currentStageAngle.ToFlightDirection();
        
        _currentStageAngle += _angleIncrement * Time.fixedDeltaTime;
        _currentStagePosition += stageFlightDirection * (Rocket.CurrentSpeed * Time.fixedDeltaTime);

        Rocket.CurrentAngle = _currentStageAngle;
        Rocket.transform.position = _currentStagePosition;
    }

    public override void StageEnd(StageModel stage)
    {
        
    }
}

public class RSEv_ThirdStageFlight : RocketStageEvent
{
    private float _stageSpeed;
    
    private float _angleIncrement;
    private float _initialAngle;
    private float _targetAngle;

    private float _currentStageAngle;
    private Vector2 _currentStagePosition;

    private float _speedDecrement;
    private float _speedAtStageEnd = 3f;
    public override void StageStart(StageModel stage)
    {
        _initialAngle = Rocket.CurrentAngle;
        
        _currentStagePosition = Rocket.transform.position;
        _currentStageAngle = _initialAngle;
        
        stage.angleAtStageStart = _initialAngle;
        _targetAngle = Rocket.stage3TargetAngle;

        float totalAngleDifference = Mathf.DeltaAngle(_initialAngle, _targetAngle);
        _angleIncrement = totalAngleDifference / stage.GetStageDuration();
        
        _stageSpeed = Rocket.CalculateSpeed(stage) + Rocket.CurrentSpeed;
        Rocket.CurrentSpeed = _stageSpeed;
        _speedDecrement = _stageSpeed - _speedAtStageEnd / stage.GetStageDuration();
        _speedDecrement *=  stage.mass / stage.referenceStageMass;
    }

    public override void StageUpdate(StageModel stage)
    {
        DecreaseSpeed();
        
        Vector2 stageFlightDirection = _currentStageAngle.ToFlightDirection();
        
        _currentStageAngle += _angleIncrement * Time.fixedDeltaTime;
        _currentStagePosition += stageFlightDirection * (Rocket.CurrentSpeed * Time.fixedDeltaTime);

        Rocket.CurrentAngle = _currentStageAngle;
        Rocket.transform.position = _currentStagePosition;
    }

    public override void StageEnd(StageModel stage)
    {
        Debug.Log("Stage 3 ended");
    }
    
    private void DecreaseSpeed()
    {
        Rocket.CurrentSpeed = Mathf.Max(Rocket.CurrentSpeed - _speedDecrement, Rocket.stage3MinimumSpeed);
    }
}

public class RocketStageEvent
{
    protected Rocket Rocket;

    public RocketStageEvent InjectRocket(Rocket rocketToInject)
    {
        Rocket = rocketToInject;
        return this;
    }
    
    public virtual void StageStart(StageModel stage)
    {
        
    }

    public virtual void StageUpdate(StageModel stage)
    {
       
    }

    public virtual void StageEnd(StageModel stage)
    {
        
    }
}