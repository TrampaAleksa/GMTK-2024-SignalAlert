using UnityEngine;

public class Rocket : MonoBehaviour
{
    public RocketHandler rocketHandler;

    public RocketStateMachine rocketStateMachine;
    public RocketStageEventHandler rocketStageEventHandler;
    public FlightPathHistory flightPathHistory;

    public StageModel stage1;
    public StageModel stage2;
    public StageModel stage3;

    public float maxSpeed = 100f;
    public float startAngle = 60f; // Reference angle in degrees
    public float stage3TargetAngle = -60f;
    public float stage3MinimumSpeed = 2f;

    public float CurrentSpeed { get; set; }
    public float CurrentAngle
    {
        get => 90f + transform.GetAngle();
        set => transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, -90f+value));
    }

    private Vector2 _initialPosition = Vector2.zero;
    private Quaternion _initialQuaternion = Quaternion.identity;
    public Vector3 Position => transform.position;

    private void Awake()
    {
        _initialPosition = transform.position;
        _initialQuaternion = transform.rotation;
        
        RocketCollisionEvents.Instance.AddOnCollidedWithObstacle(r =>
            transform.position = _initialPosition);
        RocketCollisionEvents.Instance.AddOnCollidedWithObstacle(r =>
            flightPathHistory.FinishRecording());
        
        Init();
    }

    private void FixedUpdate()
    {
        rocketStateMachine.UpdateTime(Time.fixedDeltaTime); // handles stage start, update and end events
    }

    public void Init()
    {
        rocketStateMachine.SetupStateMachine(this);
        rocketStageEventHandler.Init(this);
        
        stage1.OnStageEnd = model =>
        {
            rocketHandler.StopMotors();
            rocketHandler.StageChanged(RocketStage.Stage1);
        };
        stage2.OnStageEnd = model => rocketHandler.StageChanged(RocketStage.Stage2);

        stage1.OnStageStart += s => flightPathHistory.StartRecording();
        stage3.OnStageEnd += s => flightPathHistory.FinishRecording();
        stage3.OnStageEnd += ResetRocket;

        stage1.angleAtStageStart = startAngle;
        stage2.angleAtStageStart = startAngle;
        stage3.angleAtStageStart = startAngle;
    }

    public void Launch()
    {
        transform.position = _initialPosition;
        transform.rotation = _initialQuaternion;
        rocketHandler.ResetStages();
        CameraHandler.Instance.ResetFirstPerson();
        CameraHandler.Instance.ToggleFirstPerson(true);
        rocketHandler.StartMotorAndLaunch(rocketStateMachine.LaunchStateMachine);
    }

    private void ResetRocket(StageModel m)
    {
        rocketHandler.ResetRocket();
    }


    public float CalculateStageDuration(StageModel stage)
    {
        var stageDuration = stage.GetStageDuration();
        return stageDuration;
    }

    public float CalculateSpeed(StageModel stage)
    {
        var stageSpeed = stage.CalculateSpeed(maxSpeed);
        return stageSpeed;
    }

    public float GetTotalDuration()
    {
        return CalculateStageDuration(stage1) + CalculateStageDuration(stage2) + CalculateStageDuration(stage3);
    }

    public StageModel GetStage(int stageNum)
    {
        return stageNum switch
        {
            1 => stage1,
            2 => stage2,
            3 => stage3,
            _ => StageModel.GetDefaultStage()
        };
    }
}