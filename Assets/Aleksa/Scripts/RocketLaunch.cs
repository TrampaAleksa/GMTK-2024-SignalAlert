using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketLaunch : MonoBehaviour
{
    public StageModel stage1;
    public StageModel stage2;
    public StageModel stage3;
    public float maxSpeed = 100f;
    public float startAngle = 45f;  // Reference angle in degrees
    
    private float timeSinceLaunch;
    private float stage1Duration;
    private float stage2Duration;
    private Vector2 position;
    private Vector2 initialPosition = Vector2.zero;
    private bool isLaunched = false;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (!isLaunched)
            return;
        MoveRocket();
    }

    public void Launch()
    {
        timeSinceLaunch = 0f;
        
        position = initialPosition;
        transform.position = initialPosition;

        stage1.angleAtStageStart = startAngle;
        stage2.angleAtStageStart = startAngle;

        stage1Duration = CalculateStageDuration(stage1);
        stage2Duration = CalculateStageDuration(stage2);

        isLaunched = true;
    }

    public void MoveRocket()
    {
        timeSinceLaunch += Time.deltaTime;
        
        Vector2 stageLaunchDirection;
        float speed;
        
        // Calculate velocity based on current stage
        if (timeSinceLaunch <= stage1Duration)
        {
            stageLaunchDirection = GetFlightDirection(stage1);
            speed = CalculateSpeed(stage1);
        }
        else if (timeSinceLaunch > stage1Duration && timeSinceLaunch <= stage1Duration + stage2Duration)
        {
            stageLaunchDirection = GetFlightDirection(stage2);
            speed = CalculateSpeed(stage2);
        }
        // Stage 3 - No engines, maintain velocity
        else
        {
            // TODO - start lowering the angle by adding gravity downwards
            // temp for stage 3 taking values from stage 2
            stageLaunchDirection = GetFlightDirection(stage2);
            speed = CalculateSpeed(stage2);
        }

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
}