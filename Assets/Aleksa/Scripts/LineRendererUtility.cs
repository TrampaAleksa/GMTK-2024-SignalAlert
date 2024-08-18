using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class LineRendererUtility : MonoBehaviour
{
    [FormerlySerializedAs("RocketLaunch")] public Rocket rocket;
    public LineRenderer lineRenderer; 

    private List<Vector3> positions = new List<Vector3>();
    
    public float flightDuration = 15f;
    private float currentTime = 9000f;

    public bool simulateInRealTime = false;

    [ContextMenu("Simulate Flight")]
    public void SimulateFlight()
    {
        rocket.Init();
        rocket.transform.position = Vector3.zero;

        positions.Clear();
        lineRenderer.positionCount = 0;
        positions.Add(Vector2.zero);
        
        flightDuration = rocket.GetTotalDuration();
        
        if (simulateInRealTime)
        {
            currentTime = 0f;
            rocket.Launch();
            return;
        }
        
        rocket.stage1.OnStageUpdate += (StageModel model) => positions.Add(rocket.rocketStageEvents.position);
        rocket.stage2.OnStageUpdate += (StageModel model) => positions.Add(rocket.rocketStageEvents.position);
        rocket.stage3.OnStageUpdate += (StageModel model) => positions.Add(rocket.rocketStageEvents.position);
        rocket.stage3.OnStageEnd += (StageModel model) => SetPositions();
        
        rocket.rocketStateMachine.LaunchStateMachine();
        
        var numberOfUpdates = (int) (flightDuration * (1f / Time.fixedDeltaTime));
        
        for (int i = 0; i < numberOfUpdates; i++) // instant simulation of flight
            rocket.rocketStateMachine.UpdateTime(Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        
        if (currentTime < flightDuration)
        {
            positions.Add(rocket.transform.position);
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
