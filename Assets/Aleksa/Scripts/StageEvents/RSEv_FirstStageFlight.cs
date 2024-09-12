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