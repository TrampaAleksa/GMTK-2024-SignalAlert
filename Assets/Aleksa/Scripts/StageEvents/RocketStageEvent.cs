public class RocketStageEvent
{
    protected Rocket Rocket;

    public RocketStageEvent InjectRocket(Rocket rocketToInject)
    {
        Rocket = rocketToInject;
        return this;
    }
    
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