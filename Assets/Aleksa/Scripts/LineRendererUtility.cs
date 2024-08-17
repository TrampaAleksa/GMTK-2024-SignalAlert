using UnityEngine;
using System.Collections.Generic;

public class LineRendererUtility : MonoBehaviour
{
    public RocketLaunch RocketLaunch;
    public LineRenderer lineRenderer; 
    public int numberOfPoints = 20;

    private List<Vector3> positions = new List<Vector3>();

    [ContextMenu("Simulate Flight")]
    public void SimulateFlight()
    {
        RocketLaunch.Init();
        
        positions.Clear();
        lineRenderer.positionCount = 0;
        positions.Add(Vector2.zero);
        
        RocketLaunch.rocketStateMachine.LaunchStateMachine();
        
        RocketLaunch.stage1.OnStageUpdate += (StageModel stage) => positions.Add(RocketLaunch.rocketStageEvents.position);
        RocketLaunch.stage2.OnStageUpdate += (StageModel stage) => positions.Add(RocketLaunch.rocketStageEvents.position);
        RocketLaunch.stage3.OnStageUpdate += (StageModel stage) => positions.Add(RocketLaunch.rocketStageEvents.position);
        
        // Simulated time increment (equivalent to Time.deltaTime)
        float timeIncrement = RocketLaunch.GetTotalDuration() / (numberOfPoints - 1);
        
        // Simulate over the number of points
        for (int i = 0; i < numberOfPoints; i++)
        {
            RocketLaunch.rocketStateMachine.UpdateTime(timeIncrement); // handles stage start, update and end events
        }

        // Update the LineRenderer with the simulated positions
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
