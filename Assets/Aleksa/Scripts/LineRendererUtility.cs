using System;
using UnityEngine;
using System.Collections.Generic;

public class LineRendererUtility : MonoBehaviour
{
    public RocketLaunch RocketLaunch;
    public LineRenderer lineRenderer; 

    private List<Vector3> positions = new List<Vector3>();
    
    public float flightDuration = 15f;
    private float currentTime = 9000f;

    public bool simulateInRealTime = false;

    [ContextMenu("Simulate Flight")]
    public void SimulateFlight()
    {
        RocketLaunch.Init();
        RocketLaunch.transform.position = Vector3.zero;

        positions.Clear();
        lineRenderer.positionCount = 0;
        positions.Add(Vector2.zero);
        
        flightDuration = RocketLaunch.GetTotalDuration();
        
        if (simulateInRealTime)
        {
            currentTime = 0f;
            RocketLaunch.Launch();
            return;
        }
        

        RocketLaunch.stage1.OnStageUpdate += (StageModel model) => positions.Add(RocketLaunch.rocketStageEvents.position);
        RocketLaunch.stage2.OnStageUpdate += (StageModel model) => positions.Add(RocketLaunch.rocketStageEvents.position);
        RocketLaunch.stage3.OnStageUpdate += (StageModel model) => positions.Add(RocketLaunch.rocketStageEvents.position);
        RocketLaunch.stage3.OnStageEnd += (StageModel model) => SetPositions();
        
        RocketLaunch.rocketStateMachine.LaunchStateMachine();
        
        var numberOfUpdates = (int) (flightDuration * (1f / Time.fixedDeltaTime));
        
        for (int i = 0; i < numberOfUpdates; i++) // instant simulation of flight
            RocketLaunch.rocketStateMachine.UpdateTime(Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        
        if (currentTime < flightDuration)
        {
            positions.Add(RocketLaunch.transform.position);
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }
    }

    private void SetPositions()
    {
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
