using UnityEngine;

public class RSEv_FirstStageFlight : RocketStageEvent
{
    private float _stage1Speed;
    
    private float _angleIncrement;
    private float _initialAngle;
    private float _targetAngle;

    private float _currentAngle;
    private Vector2 _currentPosition;
    
    public override void StageStart(StageModel stage)
    {
        _currentPosition = Vector2.zero;
        
        //TODO - Swap with rocket's current angle instead
        _initialAngle = stage.angleAtStageStart;
        _currentAngle = _initialAngle;
        _targetAngle = stage.CalculateAdjustedAngle();
        
        float totalAngleDifference = Mathf.DeltaAngle(_initialAngle, _targetAngle);
        _angleIncrement = totalAngleDifference / stage.GetStageDuration();
    }

    public override void StageUpdate(StageModel stage)
    {
        Vector2 stageFlightDirection = _currentAngle.ToFlightDirection();
        _currentAngle += _angleIncrement * Time.fixedDeltaTime;
        
        // TODO - stage.CalculateSpeed() instead -- move max speed into the stage itself
        _stage1Speed = Rocket.CalculateSpeed(stage); 
        
        _currentPosition += stageFlightDirection * (_stage1Speed * Time.fixedDeltaTime);
        Rocket.transform.position = _currentPosition;
        Rocket.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -90f+_currentAngle));
    }

    public override void StageEnd(StageModel stage)
    {
        
    }
}

public class RSEv_SecondStageFlight : RocketStageEvent
{
    
    public override void StageStart(StageModel stage)
    {
        
    }

    public override void StageUpdate(StageModel stage)
    {
       
    }

    public override void StageEnd(StageModel stage)
    {
        
    }
}

public class RSEv_ThirdStageFlight : RocketStageEvent
{
    public override void StageStart(StageModel stage)
    {
        
    }

    public override void StageUpdate(StageModel stage)
    {
       
    }

    public override void StageEnd(StageModel stage)
    {
        
    }
}

public class RocketStageEvent
{
    public Rocket Rocket;
    
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