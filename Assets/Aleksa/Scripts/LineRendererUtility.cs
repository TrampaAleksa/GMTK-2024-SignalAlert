using System;
using UnityEngine;
using System.Collections.Generic;

public class LineRendererUtility : MonoBehaviour
{
    public RocketLaunch RocketLaunch;
    public LineRenderer lineRenderer; 

    private List<Vector3> positions = new List<Vector3>();
    
    private float time = 0f;
    private float totalTime = 0f;

    [ContextMenu("Simulate Flight")]
    public void SimulateFlight()
    {
        RocketLaunch.Init();
        totalTime = RocketLaunch.GetTotalDuration();
        time = 0f;
        
        positions.Clear();
        lineRenderer.positionCount = 0;
        positions.Add(Vector2.zero);

        Time.timeScale = 10f;
        RocketLaunch.Launch();
    }

    private void Update()
    {
        if (time > totalTime)
        {
            Time.timeScale = 1f;
            return;
        }
        time += Time.deltaTime;
        
        positions.Add(RocketLaunch.transform.position);
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
