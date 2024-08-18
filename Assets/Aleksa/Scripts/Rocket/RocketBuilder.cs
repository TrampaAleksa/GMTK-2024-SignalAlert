using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RocketBuilder : MonoBehaviour
{
    [FormerlySerializedAs("rocketLaunch")] public Rocket rocket;

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

    [ContextMenu("Build Rocket")]
    public void BuildRocket()
    {
        rocket.stage1.mass = stage1Config.Mass;
        rocket.stage2.mass = stage2Config.Mass;
        rocket.stage3.mass = stage3Config.Mass;

        rocket.stage1.engines = stage1Config.Engines;
        rocket.stage2.engines = stage2Config.Engines;
        rocket.stage3.engines = 1;
        
        rocket.stage1.size = stage1Config.Size;
        rocket.stage2.size = stage2Config.Size;
        rocket.stage3.size = stage3Config.Size;
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
    Small,
    Normal, // should be equal to the reference mass
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

    public RocketConfig(RocketStageSize size, int engines)
    {
        Size = size;
        Engines = engines;
    }
}