using UnityEngine;
using System;
using UnityEngine.Serialization;

public class RocketStateMachine : MonoBehaviour
{
    public RocketStage currentState = RocketStage.None;

    private float _timeSinceLaunch;
    private StageModel _stage1;
    private StageModel _stage2;
    private StageModel _stage3;

    public void LaunchStateMachine(RocketLaunch rocketLaunch)
    {
        _stage1 = rocketLaunch.stage1;
        _stage2 = rocketLaunch.stage2;
        _stage3 = rocketLaunch.stage3;
        
        _stage1 ??= StageModel.GetDefaultStage();
        _stage2 ??= StageModel.GetDefaultStage();
        _stage3 ??= StageModel.GetDefaultStage();

        _timeSinceLaunch = 0f;
        
        currentState = RocketStage.Stage1;
        HandleStageStart();
    }
    
    private void Update()
    {
        _timeSinceLaunch += Time.deltaTime;

        switch (currentState)
        {
            case RocketStage.Stage1 when IsInStage1Timeframe:
                HandleStageUpdate();
                break;
            case RocketStage.Stage1:
                TransitionToStage(RocketStage.Stage2);
                break;
            case RocketStage.Stage2 when IsInStage2Timeframe:
                HandleStageUpdate();
                break;
            case RocketStage.Stage2:
                TransitionToStage(RocketStage.Stage3);
                break;
            case RocketStage.Stage3:
                HandleStageUpdate();
                break;
            case RocketStage.None:
                // No active stage
                break;
        }
    }
    
    private void TransitionToStage(RocketStage newStage)
    {
        HandleStageEnd();
        currentState = newStage;
        HandleStageStart();
    }

    private void HandleStageUpdate()
    {
        CurrentActiveStage.OnStageUpdate?.Invoke();
    }
    private void HandleStageStart()
    {
        CurrentActiveStage.OnStageStart?.Invoke();
    }
    private void HandleStageEnd()
    {
        CurrentActiveStage.OnStageEnd?.Invoke();
    }
    private StageModel CurrentActiveStage => currentState switch 
    {
        RocketStage.Stage1 => _stage1,
        RocketStage.Stage2 => _stage2,
        RocketStage.Stage3 => _stage3,
        _ => StageModel.GetDefaultStage()
    };
    
    private bool IsInStage1Timeframe => _timeSinceLaunch <= _stage1.GetStageDuration();

    private bool IsInStage2Timeframe =>
        _timeSinceLaunch > _stage1.GetStageDuration() &&
        _timeSinceLaunch <= _stage1.GetStageDuration() + _stage2.GetStageDuration();
}

public enum RocketStage
{
    Stage1,
    Stage2,
    Stage3,
    None
}
