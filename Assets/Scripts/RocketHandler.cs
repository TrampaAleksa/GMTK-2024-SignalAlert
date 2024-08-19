using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class RocketHandler : MonoBehaviour
{
    [SerializeField]
    private List<MotorHandler> Motors= new();
    [SerializeField]
    private List<StageHandler> StageHandlers= new();
    public float duration = 2.0f;
    public float destroyDuration = 2.0f;
    public bool _isTestDestroy= false;
    public bool _isTestReset = false;
    public bool _isTestMotors = false;
    public bool _isTestStopMotors = false;
    public StageType _testType;
    private void StartMotorAndLaunch() => StartCoroutine(StartAnimation());
    private void StartMotors() => MotorHandler.StartMotors(Motors, duration);
    private void StartLaunch() => MotorHandler.StartLaunch(Motors, duration);
    private void StopMotors() => MotorHandler.StopMotors(Motors, duration);
    private void StageChanged(StageType type)
    {
        if (StageHandler.TryGetStage(StageHandlers, type, out StageHandler stage))
        {
            stage.DestroyStage(destroyDuration);
        }
    }
    private void ResetStages()
    {
        StageHandler.ResetStages(StageHandlers);

    }
    private void Update()
    {
        if (_isTestMotors)
        {
            StartMotorAndLaunch();
            _isTestMotors = false;
        }
        if (_isTestStopMotors)
        {
            StopMotors();
            _isTestStopMotors = false;
        }
        if (_isTestDestroy)
        {
            StageChanged(_testType);
            _isTestDestroy = false;
        }
        if (_isTestReset)
        {
            ResetStages();
            _isTestReset = false;
        }
    }
    IEnumerator StartAnimation()
    {
        float time = 0f;
        StartMotors();
        while (time > duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        time = 0f;
        StartLaunch();
        while (time > duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
    }
}
public enum StageType
{
    S1,
    S2,
    S3
}
