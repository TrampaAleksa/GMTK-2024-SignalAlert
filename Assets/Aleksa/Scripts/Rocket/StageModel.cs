using System;
using UnityEngine;

[Serializable]
public class StageModel
{
    [Header("Base Values")]
    public float referenceStageMass = 1000f;
    public float stageDurationAtReferenceMass = 20f;
    public float engineForce = 50f;  // Force per engine

    [Header("User Defined")]
    public float mass = 1000f;  // Mass of stage 1
    public int engines = 3;        // Number of engines in stage 1

    [HideInInspector]
    public float angleAtStageStart = 45f;  // Reference angle in degrees
    
    public Action<StageModel> OnStageStart;
    public Action<StageModel> OnStageEnd;
    public Action<StageModel> OnStageUpdate;

    [HideInInspector]
    public RocketStageSize size;
    
    
    public float GetStageDuration()
    {
        return (stageDurationAtReferenceMass * (mass) / referenceStageMass);
    }
    
    public float CalculateSpeed(float maxSpeed)
    {
        float rocketSpeed = engineForce * (engines * 0.3f) / mass;
        rocketSpeed = Mathf.Min(rocketSpeed, maxSpeed);
        return rocketSpeed;
    }
    public float CalculateAdjustedAngle()
    {
        float adjustedAngle = angleAtStageStart * (referenceStageMass / mass);
        return adjustedAngle;
    }
    
    public Vector2 GetFlightDirection() => new Vector2(Mathf.Cos(CalculateAdjustedAngle() * Mathf.Deg2Rad), Mathf.Sin(CalculateAdjustedAngle() * Mathf.Deg2Rad)).normalized;


    public static StageModel GetDefaultStage()
    {
        Debug.LogWarning("Using default stage model");
        
        return new StageModel
        {
            referenceStageMass = 1000f,
            stageDurationAtReferenceMass = 20f,
            engineForce = 50f,
            mass = 1000f,
            engines = 3,
            angleAtStageStart = 45f
        };
    }
}