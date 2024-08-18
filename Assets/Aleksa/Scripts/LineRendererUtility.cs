using System;
using UnityEngine;
using System.Collections.Generic;

public class LineRendererUtility : MonoBehaviour
{
    public RocketLaunch RocketLaunch;
    public LineRenderer lineRenderer; 

    private List<Vector3> positions = new List<Vector3>();
    
    private float timescale = 1f;

    public float numberOfSeconds;
    private float currentTime;

    [ContextMenu("Simulate Flight")]
    public void SimulateFlight()
    {
        currentTime = 0f;
        
        RocketLaunch.Init();
        
        positions.Clear();
        lineRenderer.positionCount = 0;
        positions.Add(Vector2.zero);

        Time.timeScale = timescale;
        RocketLaunch.Launch();
    }

    private void FixedUpdate()
    {
        currentTime += Time.fixedDeltaTime;
        
        if (currentTime < numberOfSeconds)
        {
            positions.Add(RocketLaunch.transform.position);
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }
    }
}
