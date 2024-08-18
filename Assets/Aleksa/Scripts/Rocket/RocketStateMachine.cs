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

    public void SetupStateMachine(Rocket rocket)
    {
        _stage1 = rocket.stage1;
        _stage2 = rocket.stage2;
        _stage3 = rocket.stage3;
        
        _stage1 ??= StageModel.GetDefaultStage();
        _stage2 ??= StageModel.GetDefaultStage();
        _stage3 ??= StageModel.GetDefaultStage();
        
        _timeSinceLaunch = 0f;
        currentState = RocketStage.None;

        _stage1.OnStageStart += (s) => Debug.Log("start");

    }

    public void LaunchStateMachine()
    {
        _timeSinceLaunch = 0f;
        currentState = RocketStage.Stage1;
        HandleStageStart();
    }
    
    public void UpdateTime(float deltaTime)
    {
        _timeSinceLaunch += deltaTime;

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
            case RocketStage.Stage3 when IsInStage3Timeframe:
                HandleStageUpdate();
                break;
            case RocketStage.Stage3 :
                TransitionToStage(RocketStage.None);
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
        CurrentActiveStage.OnStageUpdate?.Invoke(CurrentActiveStage);
    }
    private void HandleStageStart()
    {
        CurrentActiveStage.OnStageStart?.Invoke(CurrentActiveStage);
    }
    private void HandleStageEnd()
    {
        CurrentActiveStage.OnStageEnd?.Invoke(CurrentActiveStage);
    }
    public StageModel CurrentActiveStage => currentState switch 
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
    
    private bool IsInStage3Timeframe =>
        _timeSinceLaunch > _stage1.GetStageDuration() + _stage2.GetStageDuration() &&
        _timeSinceLaunch <= _stage1.GetStageDuration() + _stage2.GetStageDuration() + _stage3.GetStageDuration();
}

public enum RocketStage
{
    None,
    Stage1,
    Stage2,
    Stage3
}
