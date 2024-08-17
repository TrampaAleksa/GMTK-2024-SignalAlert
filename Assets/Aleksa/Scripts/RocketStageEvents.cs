using UnityEngine;

public class RocketStageEvents : MonoBehaviour
{
    public RocketLaunch rocketLaunch;
    private Vector2 position;
    
    public void Stage1Start(StageModel stage)
    {
        
    }
    
    public void Stage1Update(StageModel stage)
    {
        Vector2 stageLaunchDirection = rocketLaunch.GetFlightDirection(stage);
        float speed = rocketLaunch.CalculateSpeed(stage);
        
        position += stageLaunchDirection * (speed * Time.deltaTime);
        transform.position = position;
    }
    public void Stage1End(StageModel stage)
    {
        
    }
    
    
    public void Stage2Start(StageModel stage)
    {
        
    }

    public void Stage2Update(StageModel stage)
    {
        
    }
    public void Stage2End(StageModel stage)
    {
        
    }
    
    
    public void Stage3Start(StageModel stage)
    {
        
    }
    public void Stage3Update(StageModel stage)
    {
        
    }
    public void Stage3End(StageModel stage)
    {
        
    }
    
    
    
}