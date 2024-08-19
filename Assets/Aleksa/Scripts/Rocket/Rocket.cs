using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Rocket : MonoBehaviour
{
    public RocketHandler rocketHandler;

    public RocketStateMachine rocketStateMachine;
    public RocketStageEvents rocketStageEvents;
    public FlightPathHistory flightPathHistory;
    
    public StageModel stage1;
    public StageModel stage2;
    public StageModel stage3;
    
    public float maxSpeed = 100f;
    public float startAngle = 45f;  // Reference angle in degrees
    public float gravityStrength = 9.81f;
    
    private Vector2 _initialPosition = Vector2.zero;

    private void Awake()
    {
        _initialPosition = transform.position;
        RocketCollisionEvents.Instance.AddOnCollidedWithObstacle((r) => transform.position = _initialPosition);
        RocketCollisionEvents.Instance.AddOnCollidedWithObstacle((r) => flightPathHistory.FinishRecording());
        Init();
    }

    public void Init()
    {
        rocketStateMachine.SetupStateMachine(this);

        stage1.OnStageStart = rocketStageEvents.Stage1Start;
        stage2.OnStageStart = rocketStageEvents.Stage2Start;
        stage3.OnStageStart = rocketStageEvents.Stage3Start;
        stage1.OnStageUpdate = rocketStageEvents.Stage1Update;
        stage2.OnStageUpdate = rocketStageEvents.Stage2Update;
        stage3.OnStageUpdate = rocketStageEvents.Stage3Update;
        stage1.OnStageEnd = rocketStageEvents.Stage1End;
        stage1.OnStageEnd = (model) =>
        {
            rocketHandler.StopMotors();
            rocketHandler.StageChanged(RocketStage.Stage1);
        };
        stage2.OnStageEnd = rocketStageEvents.Stage2End;
        stage2.OnStageEnd = (model) => rocketHandler.StageChanged(RocketStage.Stage2);
        stage3.OnStageEnd = rocketStageEvents.Stage3End;

        stage1.OnStageStart += (s) => flightPathHistory.StartRecording();
        stage3.OnStageEnd += (s) => flightPathHistory.FinishRecording();
        stage3.OnStageEnd += (s) => rocketHandler.ResetStages();

        stage1.angleAtStageStart = startAngle;
        stage2.angleAtStageStart = startAngle;
        stage3.angleAtStageStart = startAngle;
    }

    public void Launch()
    {
        transform.position = _initialPosition;
        rocketHandler.StartMotorAndLaunch(rocketStateMachine.LaunchStateMachine);
    }

    void FixedUpdate()
    {
        rocketStateMachine.UpdateTime(Time.fixedDeltaTime); // handles stage start, update and end events
    }

    
    public float CalculateStageDuration(StageModel stage)
    {
        float stageDuration = stage.GetStageDuration();
        return stageDuration;
    }
    public float CalculateSpeed(StageModel stage)
    {
        float stageSpeed = stage.CalculateSpeed(maxSpeed);
        return stageSpeed;
    }
    public Vector2 GetFlightDirection(StageModel stage)
    {
        Vector2 flightDirection = stage.GetFlightDirection();
        return flightDirection;
    }

    public float GetTotalDuration()
    {
        return CalculateStageDuration(stage1) + CalculateStageDuration(stage2) + CalculateStageDuration(stage3);
    }

    public StageModel GetStage(int stageNum)
    {
        return stageNum switch
        {
            1 => stage1,
            2 => stage2,
            3 => stage3,
            _ => StageModel.GetDefaultStage()
        };
    }
}