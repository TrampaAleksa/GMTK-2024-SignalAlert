using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlightPathHistory : MonoBehaviour
{
    public Button[] showFlightPathInfoButtons;
    
    public LineRendererUtility lineRendererUtility;
    public Rocket rocket;
    public List<FlightPath> previousFlights;

    private int _currentFlightNumber = -1;

    private bool _isRecording;

    public bool isDisabled = true;

    public TMP_Text stage1Info;
    public TMP_Text stage2Info;
    public TMP_Text stage3Info;
    public Vector2 infoOffset = new Vector2(8f, -2f);
    
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
    private void Start()
    {
        rocket.stage2.OnStageStart += model =>
        {
            var flightPath = previousFlights[_currentFlightNumber];
            stage2Info.transform.position = flightPath.positions[^1] + new Vector3(infoOffset.x, infoOffset.y, 0f);
        };

        rocket.stage3.OnStageStart += model =>
        {
            var flightPath = previousFlights[_currentFlightNumber];
            stage3Info.transform.position = flightPath.positions[^1] + new Vector3(infoOffset.x, infoOffset.y, 0f);
        };
    }


    public void StartRecording()
    {
        if (isDisabled)
            return;
        
        FlightPath newPath = new FlightPath();

        stage1Info.gameObject.SetActive(false);
        stage2Info.gameObject.SetActive(false);
        stage3Info.gameObject.SetActive(false);
        
        newPath.positions = new List<Vector3>();
        newPath.AddFlightConfig(rocket.stage1.size, rocket.stage1.engines, 1);
        newPath.AddFlightConfig(rocket.stage2.size, rocket.stage2.engines, 2);
        newPath.AddFlightConfig(rocket.stage3.size, rocket.stage3.engines, 3);
        
        previousFlights.Add(newPath);

        _currentFlightNumber++;
        _isRecording = true;
        
        stage1Info.transform.position = new Vector3(infoOffset.x, infoOffset.y, 0f);
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
        
        stage1Info.text = previousFlights[flightIndex].stage1Config.ToString();
        stage2Info.text = previousFlights[flightIndex].stage2Config.ToString();
        stage3Info.text = previousFlights[flightIndex].stage3Config.ToString();
        
        stage1Info.gameObject.SetActive(true);
        stage2Info.gameObject.SetActive(true);
        stage3Info.gameObject.SetActive(true);
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