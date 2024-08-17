using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketLaunch : MonoBehaviour
{
    public float M1 = 1000f;  // Mass of stage 1
    public float M2 = 500f;   // Mass of stage 2
    public float M3 = 50f;   // Mass of stage 3
    public int E1 = 3;        // Number of engines in stage 1
    public int E2 = 2;        // Number of engines in stage 2

    // Stage mase 1000 ce trajati 20 sekundi sa jednim motorom i lansirati se pod uglom od 45 stepeni
    public float referenceAngle = 45f;  // Reference angle in degrees
    public float referenceStageDuration = 20f;
    public float referenceStageMass = 1000f;

    public float engineForce = 50f;  // Force per engine
    public float maxSpeed = 100f;

    private float currentMass;
    private float timeSinceLaunch;
    private float stage1Duration;
    private float stage2Duration;
    private float speed;
    private Vector2 position;
    private Vector2 launchDirection;
    private Vector2 initialPosition = Vector2.zero;
    private bool isLaunched = false;

    private void Awake()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        MoveRocket();
    }

    public void Launch()
    {
        currentMass = M1 + M2 + M3;
        timeSinceLaunch = 0f;
        
        position = initialPosition;
        transform.position = initialPosition;

        stage1Duration = CalculateStageDuration(M1, E1);
        stage2Duration = CalculateStageDuration(M2, E2);

        launchDirection = GetFlightDirection(M1);

        isLaunched = true;
    }

    public void MoveRocket()
    {
        if (!isLaunched)
            return;
        
        timeSinceLaunch += Time.deltaTime;

        // Calculate velocity based on current stage
        if (timeSinceLaunch <= stage1Duration)
        {
            launchDirection = GetFlightDirection(M1);
            speed = CalculateSpeed(E1, M1);
        }
        else if (timeSinceLaunch > stage1Duration && timeSinceLaunch <= stage1Duration + stage2Duration)
        {
            launchDirection = GetFlightDirection(M2);
            speed = CalculateSpeed(E2, M2);
        }
        // Stage 3 - No engines, maintain velocity
        else
        {
            // TODO - start lowering the angle by adding gravity downwards
        }

        position += launchDirection * (speed * Time.deltaTime);
        transform.position = position;
    }

    public float CalculateStageDuration(float mass, int engines)
    {
        return (referenceStageDuration * referenceStageMass) / (mass * engines);
    }
    
    public float CalculateStageDuration(int stageNumber)
    {
        return stageNumber switch
        {
            1 => CalculateStageDuration(M1, E1),
            2 => CalculateStageDuration(M2, E2),
            _ => referenceStageDuration
        };
    }
    
    
    public float CalculateSpeed(int engines, float stageMass)
    {
        float rocketSpeed = engineForce * engines / stageMass;
        rocketSpeed = Mathf.Min(rocketSpeed, maxSpeed);
        return rocketSpeed;
    }
    
    public float CalculateSpeed(int stageNumber)
    {
        return stageNumber switch
        {
            1 => CalculateSpeed(E1, M1),
            2 => CalculateSpeed(E2, M2),
            _ => CalculateSpeed(1, referenceStageMass)
        };
    }
    
    //TODO - Add reference angle for stage 1 and 2 instead of one reference angle
    public float CalculateAdjustedAngle(float givenMass)
    {
        float adjustedAngle = referenceAngle * (referenceStageMass / givenMass);
        return adjustedAngle;
    }

    public Vector2 GetFlightDirection(float mass) => new Vector2(Mathf.Cos(CalculateAdjustedAngle(mass) * Mathf.Deg2Rad), Mathf.Sin(CalculateAdjustedAngle(mass) * Mathf.Deg2Rad)).normalized;

    public Vector2 GetFlightDirection(int stageNumber)
    {
        return stageNumber switch
        {
            1 => GetFlightDirection(M1),
            2 => GetFlightDirection(M2),
            _ => GetFlightDirection(referenceStageMass)
        };
    }

    public float CalculateRemainingFuel(float initialFuelMass, float stageDuration, float elapsedTime)
    {
        elapsedTime = Mathf.Clamp(elapsedTime, 0, stageDuration);

        float remainingFuel = initialFuelMass * (1 - (elapsedTime / stageDuration));

        return remainingFuel;
    }

    public int GetStagePhase()
    {
        if (timeSinceLaunch <= stage1Duration)
            return 1;
        else if (timeSinceLaunch > stage1Duration && timeSinceLaunch <= stage1Duration + stage2Duration)
            return 2;
        else
            return 3;
    }
}