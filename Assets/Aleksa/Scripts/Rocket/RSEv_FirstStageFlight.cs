using UnityEngine;

public class RSEv_FirstStageFlight : RocketStageEvent
{
    
    public override void StageStart(StageModel stage)
    {
        
    }

    public override void StageUpdate(StageModel stage)
    {
       
    }

    public override void StageEnd(StageModel stage)
    {
        
    }
}

public class RSEv_SecondStageFlight : RocketStageEvent
{
    
    public override void StageStart(StageModel stage)
    {
        
    }

    public override void StageUpdate(StageModel stage)
    {
       
    }

    public override void StageEnd(StageModel stage)
    {
        
    }
}

public class RSEv_ThirdStageFlight : RocketStageEvent
{
    public override void StageStart(StageModel stage)
    {
        
    }

    public override void StageUpdate(StageModel stage)
    {
       
    }

    public override void StageEnd(StageModel stage)
    {
        
    }
}

public class RocketStageEvent
{
    public virtual void StageStart(StageModel stage)
    {
        
    }

    public virtual void StageUpdate(StageModel stage)
    {
       
    }

    public virtual void StageEnd(StageModel stage)
    {
    }
}