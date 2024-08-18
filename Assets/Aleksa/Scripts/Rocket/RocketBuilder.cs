using System;
using UnityEngine;

public class RocketBuilder : MonoBehaviour
{
    public RocketLaunch rocketLaunch;

    public RocketConfig stage1 = new RocketConfig(800f, 1000f, 1200f, 1400f);
    public RocketConfig stage2 = new RocketConfig(300f, 500f, 550f, 600f);
    public RocketConfig stage3 = new RocketConfig(4.5f, 5f, 5.2f, 5.5f);

    public void ChangeStageSize(RocketStageSize sizeToSet, int stageNum)
    {
        switch (stageNum)
        {
            case 1:
                stage1.Size = sizeToSet;
                break;
            case 2:
                stage2.Size = sizeToSet;
                break;
            case 3:
                stage3.Size = sizeToSet;
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
                stage1.Engines = engines;
                break;
            case 2:
                stage2.Engines = engines;
                break;
            case 3:
                stage3.Engines = engines;
                break;
            default:
                Debug.LogError("Invalid stage number!");
                break;
        }
    }

    
    
    
    private void SetMassBasedOnSize(int stageNum)
    {
        switch (stageNum)
        {
            case 1:
                stage1.Mass = stage1.MassOptions[(int)stage1.Size];
                break;
            case 2:
                stage2.Mass = stage2.MassOptions[(int)stage2.Size];
                break;
            case 3:
                stage3.Mass = stage3.MassOptions[(int)stage3.Size];
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