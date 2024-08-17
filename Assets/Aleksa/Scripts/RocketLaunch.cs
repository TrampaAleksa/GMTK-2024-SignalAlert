using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketLaunch : MonoBehaviour
{
    public RocketStateMachine rocketStateMachine;
    public StageModel stage1;
    public StageModel stage2;
    public StageModel stage3;
    public float maxSpeed = 100f;
    public float startAngle = 45f;  // Reference angle in degrees
    
    private Vector2 position;
    private Vector2 initialPosition = Vector2.zero;
    private bool isLaunched = false;

    private void Awake()
    {
        initialPosition = transform.position;
        rocketStateMachine = GetComponent<RocketStateMachine>();
        rocketStateMachine.SetupStateMachine(this);

        stage1.OnStageUpdate = Stage1Update;
        stage2.OnStageUpdate = Stage2Update;
    }

    public void Launch()
    {
        position = initialPosition;
        transform.position = initialPosition;

        stage1.angleAtStageStart = startAngle;
        stage2.angleAtStageStart = startAngle;

        rocketStateMachine.LaunchStateMachine();
        isLaunched = true;
    }

    void Update()
    {
        if (!isLaunched)
            return;
        
        rocketStateMachine.UpdateTime(Time.deltaTime); // handles stage start, update and end events
    }

    public void Stage1Update(StageModel stage)
    {
        Vector2 stageLaunchDirection = GetFlightDirection(stage);
        float speed = CalculateSpeed(stage);
        
        position += stageLaunchDirection * (speed * Time.deltaTime);
        transform.position = position;
    }
    public void Stage2Update(StageModel stage)
    {
        Vector2 stageLaunchDirection = GetFlightDirection(stage);
        float speed = CalculateSpeed(stage);
        
        position += stageLaunchDirection * (speed * Time.deltaTime);
        transform.position = position;
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

    public StageModel CurrentlyActiveStage => rocketStateMachine.CurrentActiveStage;
}