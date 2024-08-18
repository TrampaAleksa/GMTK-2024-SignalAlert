using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketBuilder : MonoBehaviour
{
    public RocketLaunch rocketLaunch;

    public RocketConfig stage1Config = new RocketConfig(800f, 1000f, 1200f, 1400f);
    public RocketConfig stage2Config = new RocketConfig(300f, 500f, 550f, 600f);
    public RocketConfig stage3Config = new RocketConfig(4.5f, 5f, 5.2f, 5.5f);

    public void ChangeStageSize(RocketStageSize sizeToSet, int stageNum)
    {
        switch (stageNum)
        {
            case 1:
                stage1Config.Size = sizeToSet;
                break;
            case 2:
                stage2Config.Size = sizeToSet;
                break;
            case 3:
                stage3Config.Size = sizeToSet;
                break;
            default:
                Debug.LogError("Invalid stage number!");
                break;
        }
        
        SetMassBasedOnSize(stageNum);
    }

    public void ChangeStageEngines(int engines, int stageNum)
    {
        switch (stageNum)
        {
            case 1:
                stage1Config.Engines = engines;
                break;
            case 2:
                stage2Config.Engines = engines;
                break;
            case 3:
                stage3Config.Engines = engines;
                break;
            default:
                Debug.LogError("Invalid stage number!");
                break;
        }
    }

    public void BuildRocket()
    {
        rocketLaunch.stage1.mass = stage1Config.Mass;
        rocketLaunch.stage2.mass = stage2Config.Mass;
        rocketLaunch.stage3.mass = stage3Config.Mass;

        rocketLaunch.stage1.engines = stage1Config.Engines;
        rocketLaunch.stage2.engines = stage2Config.Engines;
        rocketLaunch.stage3.engines = 0;
    }


    public RocketConfig GetStageConfig(int stageNum)
    {
        switch (stageNum)
        {
            case 1: return stage1Config;
            case 2: return stage2Config;
            case 3: return stage3Config;
            default:
                Debug.LogError("Invalid stage number!");
                return null;
        }
    }

    private void SetMassBasedOnSize(int stageNum)
    {
        switch (stageNum)
        {
            case 1:
                stage1Config.Mass = stage1Config.MassOptions[(int)stage1Config.Size];
                break;
            case 2:
                stage2Config.Mass = stage2Config.MassOptions[(int)stage2Config.Size];
                break;
            case 3:
                stage3Config.Mass = stage3Config.MassOptions[(int)stage3Config.Size];
                break;
            default:
                Debug.LogError("Invalid stage number!");
                break;
        }
    }
}


public enum RocketStageSize
{
    Normal, // should be equal to the reference mass
    Small,
    Big,
    Huge
}

[Serializable]
public class RocketConfig
{
    public float[] MassOptions;
    public RocketStageSize Size;

    public float Mass;
    public int Engines;
    
    public RocketConfig(float mass1, float mass2, float mass3, float mass4)
    {
        MassOptions = new float[] { mass1, mass2, mass3, mass4 };
        Mass = mass2; // Default to "Normal"
        Engines = 2;
        Size = RocketStageSize.Normal;
    }
}