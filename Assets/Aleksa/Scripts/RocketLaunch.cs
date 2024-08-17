using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketLaunch : MonoBehaviour
{
    public RocketStateMachine rocketStateMachine;
    public RocketStageEvents rocketStageEvents;
    
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
        stage2.OnStageEnd = rocketStageEvents.Stage2End;
        stage3.OnStageEnd = rocketStageEvents.Stage3End;
        
        stage1.angleAtStageStart = startAngle;
        stage2.angleAtStageStart = startAngle;
        stage3.angleAtStageStart = startAngle;
    }

    public void Launch()
    {
        transform.position = _initialPosition;
        rocketStateMachine.LaunchStateMachine();
    }

    void Update()
    {
        rocketStateMachine.UpdateTime(Time.deltaTime); // handles stage start, update and end events
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
}