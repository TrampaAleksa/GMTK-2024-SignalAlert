using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.CullingGroup;

public class RocketHandler : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnRocketReset;
    [SerializeField]
    private List<MotorHandler> Motors= new();
    [SerializeField]
    private List<StageHandler> StageHandlers= new();
    public float duration = 2.0f;
    public float destroyDuration = 2.0f;
    public RocketStage _testType;

    private void Start()
    {
        RocketCollisionEvents.Instance.AddOnCollidedWithObstacle((r) => ResetRocket());
    }
    public void ResetRocket()
    {
        OnRocketReset?.Invoke();
        CameraHandler.Instance.ToggleFirstPerson(false);
    }
    public void StartMotorAndLaunch(Action onPrepDone) => StartCoroutine(LaunchAnimation(onPrepDone));
    private void StartMotors() => MotorHandler.StartMotors(Motors, duration);
    private void StartLaunch() => MotorHandler.StartLaunch(Motors, duration);
    public void StopMotors() => MotorHandler.StopMotors(Motors, duration);
    public void StageChanged(RocketStage type)
    {
        if (StageHandler.TryGetStage(StageHandlers, type, out StageHandler stage))
        {
            stage.DestroyStage(destroyDuration);
        }
    }
    public void ResetStages()
    {
        StageHandler.ResetStages(StageHandlers);

    }
    private void Update()
    {
        //StartMotorAndLaunch();
        //StopMotors();
        //StageChanged(_testType);
        //ResetStages();
    }
    IEnumerator LaunchAnimation(Action onDone)
    {
        float time = 0f;
        StartMotors();
        while (time < duration)
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
        onDone?.Invoke();
    }
}
