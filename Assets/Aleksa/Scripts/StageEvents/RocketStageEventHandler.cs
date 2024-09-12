using UnityEngine;

public class RocketStageEventHandler : MonoBehaviour
{
    public Rocket rocket;

    private StageModel stage1 => rocket.stage1;
    private StageModel stage2 => rocket.stage2;
    private StageModel stage3 => rocket.stage3;

    private RocketStageEvent _firstStageEvents;
    private RocketStageEvent _secondStageEvents;
    private RocketStageEvent _thirdStageEvents;

    
    public void Init(Rocket rocketRef)
    {
        this.rocket = rocketRef;
        _firstStageEvents = new RSEv_FirstStageFlight().InjectRocket(rocketRef);
        _secondStageEvents = new RSEv_SecondStageFlight().InjectRocket(rocketRef);
        _thirdStageEvents = new RSEv_ThirdStageFlight().InjectRocket(rocketRef);

        AddRocketStageEvent(stage1, _firstStageEvents);
        AddRocketStageEvent(stage2, _secondStageEvents);
        AddRocketStageEvent(stage3, _thirdStageEvents);
    }

    public void AddRocketStageEvent(StageModel stage, RocketStageEvent eventToSetup)
    {
        stage.OnStageStart = eventToSetup.StageStart;
        stage.OnStageUpdate = eventToSetup.StageUpdate;
        stage.OnStageEnd = eventToSetup.StageEnd;
    }
}