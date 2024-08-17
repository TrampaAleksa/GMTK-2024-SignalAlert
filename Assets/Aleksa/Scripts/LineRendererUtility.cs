using UnityEngine;
using System.Collections.Generic;

public class LineRendererUtility : MonoBehaviour
{
    public RocketLaunch RocketLaunch;
    public LineRenderer lineRenderer;  // Reference to the LineRenderer component
    public int numberOfPoints = 20;    // Default number of points for the path

    private List<Vector3> positions = new List<Vector3>();

    [ContextMenu("Simulate Flight")]
    public void SimulateFlight()
    {
        positions.Clear();
        lineRenderer.positionCount = 0;

        // Calculate stage durations
        float stage1Duration = RocketLaunch.CalculateStageDuration(1);
        float stage2Duration = RocketLaunch.CalculateStageDuration(2);
        float totalDuration = stage1Duration + stage2Duration;

        Vector2 position = Vector2.zero;
        Vector2 launchDirection = Vector2.zero;
        float speed = 0f;
        
        positions.Add(Vector2.zero);
        
        // Simulated time increment (equivalent to Time.deltaTime)
        float timeIncrement = totalDuration / (numberOfPoints - 1);
        float elapsedTime = 0f;
        
        // Simulate over the number of points
        for (int i = 0; i < numberOfPoints; i++)
        {
            elapsedTime += timeIncrement;

            // Determine stage and calculate direction and speed
            if (elapsedTime <= stage1Duration)
            {
                launchDirection = RocketLaunch.GetFlightDirection(1);
                speed = RocketLaunch.CalculateSpeed(1);
            }
            else if (elapsedTime > stage1Duration && elapsedTime <= stage1Duration + stage2Duration)
            {
                launchDirection = RocketLaunch.GetFlightDirection(2);
                speed = RocketLaunch.CalculateSpeed(2);
            }
            else
            {
                // End of simulation, no further movement
                break;
            }

            // Calculate position
            var previousPosition = new Vector2(positions[^1].x, positions[^1].y);
            position = previousPosition + launchDirection * speed * timeIncrement;

            // Store position
            positions.Add(new Vector3(position.x, position.y, 0));
        }

        // Update the LineRenderer with the simulated positions
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
