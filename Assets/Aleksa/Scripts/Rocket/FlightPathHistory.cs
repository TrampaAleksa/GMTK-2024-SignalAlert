using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlightPathHistory : MonoBehaviour
{
    public TMP_Text flightPathInfoText;
    public Button[] showFlightPathInfoButtons;
    
    public LineRendererUtility lineRendererUtility;
    public Rocket rocket;
    public List<FlightPath> previousFlights;

    private int _currentFlightNumber = -1;

    private bool _isRecording;

    public bool isDisabled = true;
    
    private void Awake()
    {
        if (isDisabled)
            return;
        
        previousFlights = new List<FlightPath>();
        rocket = FindObjectOfType<Rocket>();
        lineRendererUtility = FindObjectOfType<LineRendererUtility>();

        for (int i = 0; i < showFlightPathInfoButtons.Length; i++)
        {
            var index = i;
            showFlightPathInfoButtons[index].onClick.AddListener(() => ShowFlightPathInfo(index));
            showFlightPathInfoButtons[index].GetComponentInChildren<TMP_Text>().text = (index + 1).ToString();
            showFlightPathInfoButtons[index].gameObject.SetActive(false);
        }
    }

    public void StartRecording()
    {
        if (isDisabled)
            return;
        
        FlightPath newPath = new FlightPath();
        
        newPath.positions = new List<Vector3>();
        newPath.AddFlightConfig(rocket.stage1.size, rocket.stage1.engines, 1);
        newPath.AddFlightConfig(rocket.stage2.size, rocket.stage2.engines, 2);
        newPath.AddFlightConfig(rocket.stage3.size, rocket.stage3.engines, 3);
        
        previousFlights.Add(newPath);

        _currentFlightNumber++;
        _isRecording = true;
    }
    
    public void FinishRecording()
    {
        if (isDisabled)
            return;
        showFlightPathInfoButtons[_currentFlightNumber].gameObject.SetActive(true);
        _isRecording = false;
    }

    private void ShowFlightPathInfo(int flightIndex)
    {
        if (isDisabled)
            return;
        lineRendererUtility.DrawPath(previousFlights[flightIndex].positions);
        flightPathInfoText.text = previousFlights[flightIndex].ToString();
    }
    
    private void FixedUpdate()
    {
        if (isDisabled)
            return;
        
        if (!_isRecording) 
            return;
        
        previousFlights[_currentFlightNumber].positions.Add(rocket.transform.position);
    }
}

[Serializable]
public struct FlightPath
{
    public List<Vector3> positions;
    public RocketConfig stage1Config;
    public RocketConfig stage2Config;
    public RocketConfig stage3Config;
    
    public void AddFlightConfig(RocketStageSize size, int engines, int stageNum)
    {
        switch (stageNum)
        {
            case 1:
                stage1Config = new RocketConfig(size, engines);
                break;
            case 2:
                stage2Config = new RocketConfig(size, engines);
                break;
            case 3:
                stage3Config = new RocketConfig(size, engines);
                break;
            default:
                Debug.LogError("Invalid stage number");
                break;
        }
    }

    public override string ToString()
    {
        return $"Stage I: {stage1Config.Size}, {stage1Config.Engines} \nStage II: {stage2Config.Size}, {stage2Config.Engines} \nStage III: {stage3Config.Size}, {stage3Config.Engines}";
    }
}