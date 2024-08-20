using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageBuilderUI : MonoBehaviour
{
    public UnityEngine.UI.Slider slider;
    public TMP_Text enginesLabel;
    public Button enginesIncreaseButton;
    public Button enginesDecreaseButton;

    private int _stageNum;
    private RocketBuilder _rocketBuilder;

    public void Initialize(int stageNum, RocketBuilder rocketBuilder)
    {
        _stageNum = stageNum;
        _rocketBuilder = rocketBuilder;

        slider.onValueChanged.AddListener((val) => ChangeStageSize(RocketStageSize.Small + (int)val));

        enginesIncreaseButton.onClick.AddListener(() => ChangeStageEngines(1));
        enginesDecreaseButton.onClick.AddListener(() => ChangeStageEngines(-1));

        UpdateEngineLabel();
    }

    private void ChangeStageSize(RocketStageSize size)
    {
        _rocketBuilder.ChangeStageSize(size, _stageNum);
        UpdateEngineLabel();
    }

    private void ChangeStageEngines(int change)
    {
        RocketConfig stageConfig = _rocketBuilder.GetStageConfig(_stageNum);
        _rocketBuilder.ChangeStageEngines(Mathf.Clamp(stageConfig.Engines + change, 0, 10), _stageNum);
        UpdateEngineLabel();
    }

    private void UpdateEngineLabel()
    {
        RocketConfig stageConfig = _rocketBuilder.GetStageConfig(_stageNum);
        enginesLabel.text = stageConfig.Engines.ToString();
    }
}