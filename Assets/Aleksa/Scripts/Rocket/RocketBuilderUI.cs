using UnityEngine;

public class RocketBuilderUI : MonoBehaviour
{
    public StageBuilderUI stage1BuilderUI;
    public StageBuilderUI stage2BuilderUI;
    public StageBuilderUI stage3BuilderUI;

    private RocketBuilder _rocketBuilder;

    private void Start()
    {
        _rocketBuilder = FindObjectOfType<RocketBuilder>();

        stage1BuilderUI.Initialize(1, _rocketBuilder);
        stage2BuilderUI.Initialize(2, _rocketBuilder);
        stage3BuilderUI.Initialize(3, _rocketBuilder);
    }
}