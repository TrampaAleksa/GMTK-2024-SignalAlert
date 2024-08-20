using System;
using System.Collections;
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
    
    public float delayBeforeDrawingLastPath = 2f;
    public float timeBetweenDrawIterationLastPath = 2f;
    public int numberOfDrawingIterationsLastPath = 10;
    private Coroutine _drawOverTimeRoutine;

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
        if (isDisabled)
            return;
        
        rocket.stage2.OnStageStart += model =>
        {
            previousFlights[_currentFlightNumber].stage2Position = rocket.transform.position;
        };

        rocket.stage3.OnStageStart += model =>
        {
            previousFlights[_currentFlightNumber].stage3Position = rocket.transform.position;
        };
    }


    public void StartRecording()
    {
        if (isDisabled)
            return;

        lineRendererUtility.lineRenderer.positionCount = 0;
        
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

        StartCoroutine(DrawPathOverTime(previousFlights[_currentFlightNumber].positions));
    }

    private void ShowFlightPathInfo(int flightIndex)
    {
        if (isDisabled)
            return;
        var previousFlight = previousFlights[flightIndex];
        lineRendererUtility.DrawPath(previousFlight.positions);
        
        stage1Info.text = $"Stage I: {previousFlight.stage1Config.Size.ToString()}, {previousFlight.stage1Config.Engines}";
        stage2Info.text = $"Stage II: {previousFlight.stage2Config.Size.ToString()}, {previousFlight.stage2Config.Engines}";
        stage3Info.text = $"Stage III: {previousFlight.stage3Config.Size.ToString()}, {previousFlight.stage3Config.Engines}";
        
        stage1Info.transform.position = previousFlight.stage1Position + infoOffset;
        stage2Info.transform.position = previousFlight.stage2Position + infoOffset;
        stage3Info.transform.position = previousFlight.stage3Position + infoOffset;

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
    
    
    
    
    private IEnumerator DrawPathOverTime(List<Vector3> positions)
    {
        yield return new WaitForSeconds(delayBeforeDrawingLastPath);

        lineRendererUtility.lineRenderer.positionCount = 0;
        int totalPositions = positions.Count;
        int pointsPerIteration = Mathf.Max(1, totalPositions / numberOfDrawingIterationsLastPath);

        var pointsToDraw = new List<Vector3>();
        
        for (int i = 0; i < numberOfDrawingIterationsLastPath; i++)
        {
            int start = i * pointsPerIteration;
            int end = Mathf.Min(start + pointsPerIteration, totalPositions);
            
            for (int j = start; j < end; j++)
            {
                pointsToDraw.Add(positions[j]);
            }

            lineRendererUtility.DrawPath(pointsToDraw);

            yield return new WaitForSeconds(timeBetweenDrawIterationLastPath);
        }
        
        ShowFlightPathInfo(_currentFlightNumber);
    }
}

[Serializable]
public class FlightPath
{
    public List<Vector3> positions;
    public RocketConfig stage1Config;
    public RocketConfig stage2Config;
    public RocketConfig stage3Config;
    public Vector2 stage1Position;
    public Vector2 stage2Position = new Vector3(1110f, 1110f, 0f);
    public Vector2 stage3Position = new Vector3(1110f, 1110f, 0f);
    
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