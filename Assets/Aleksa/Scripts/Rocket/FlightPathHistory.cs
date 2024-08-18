using System;
using System.Collections.Generic;
using UnityEngine;

public class FlightPathHistory : MonoBehaviour
{
    public Rocket rocket;
    public List<FlightPath> previousFlights;

    private int _currentFlightNumber = -1;

    private bool _isRecording;
    
    private void Awake()
    {
        previousFlights = new List<FlightPath>();
        rocket = FindObjectOfType<Rocket>();
    }

    public void StartRecording()
    {
        previousFlights.Add(new FlightPath
        {
            positions = new List<Vector3>()
        });
        _isRecording = true;
        _currentFlightNumber++;
    }
    
    public void FinishRecording()
    {
        _isRecording = false;
    }
    
    private void FixedUpdate()
    {
        if (!_isRecording) 
            return;
        
        previousFlights[_currentFlightNumber].positions.Add(rocket.transform.position);
    }
}

[Serializable]
public struct FlightPath
{
    public List<Vector3> positions;
}