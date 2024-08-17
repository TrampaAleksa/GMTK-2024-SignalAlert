using UnityEngine;
using System.Collections.Generic;

public class LineRendererUtility : MonoBehaviour
{
    public LineRenderer lineRenderer;  // Reference to the LineRenderer component
    public int numberOfPoints = 20;    // Default number of points for the path

    public float M1 = 1000f;  // Mass of stage 1
    public float M2 = 500f;   // Mass of stage 2
    public float M3 = 50f;    // Mass of stage 3
    public int E1 = 3;        // Number of engines in stage 1
    public int E2 = 2;        // Number of engines in stage 2

    public float referenceAngle = 45f;  // Reference angle in degrees
    public float referenceStageDuration = 20f;
    public float referenceStageMass = 1000f;

    public float engineForce = 50f;  // Force per engine
    public float maxSpeed = 100f;

    private List<Vector3> positions = new List<Vector3>();

    
    [ContextMenu("Simulate Flight")]
    public void SimulateFlight()
    {
        positions.Clear();
        lineRenderer.positionCount = 0;

        // Calculate stage durations
        float stage1Duration = CalculateStageDuration(M1, E1);
        float stage2Duration = CalculateStageDuration(M2, E2);
        float totalDuration = stage1Duration + stage2Duration;

        Vector2 position = Vector2.zero;
        Vector2 launchDirection;
        float speed;
        
        // Simulate over the number of points
        for (int i = 0; i < numberOfPoints; i++)
        {
            float timeFraction = (float)i / (numberOfPoints - 1);
            float timeSinceLaunch = timeFraction * totalDuration;

            // Determine stage and calculate direction and speed
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
            else
            {
                // End of simulation, no further movement
                break;
            }

            // Calculate position
            position += launchDirection * speed * (totalDuration / numberOfPoints);

            // Store position
            positions.Add(new Vector3(position.x, position.y, 0));
        }

        // Update the LineRenderer with the simulated positions
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }

    float CalculateStageDuration(float mass, int engines)
    {
        return (referenceStageDuration * referenceStageMass) / (mass * engines);
    }

    float CalculateSpeed(int engines, float stageMass)
    {
        float rocketSpeed = engineForce * engines / stageMass;
        rocketSpeed = Mathf.Min(rocketSpeed, maxSpeed);
        return rocketSpeed;
    }

    public float CalculateAdjustedAngle(float givenMass)
    {
        float adjustedAngle = referenceAngle * (referenceStageMass / givenMass);
        return adjustedAngle;
    }

    Vector2 GetFlightDirection(float mass)
    {
        float adjustedAngle = CalculateAdjustedAngle(mass);
        return new Vector2(Mathf.Cos(adjustedAngle * Mathf.Deg2Rad), Mathf.Sin(adjustedAngle * Mathf.Deg2Rad)).normalized;
    }
}
