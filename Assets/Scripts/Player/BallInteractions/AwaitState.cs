using UnityEngine;

public class AwaitState : BaseState
{
    public AwaitState() : 
        base("Await")
    {
        
    }

    public override bool CanEnterState(BallInteractionsFSM p)
    {
        return true;
    }
    public override void StateEnter(BallInteractionsFSM p)
    {
        
    }
    public override void StateUpdate(BallInteractionsFSM p)
    {
        if(p.pbi.kickInput.WasPressedThisFrame())
        {
            Debug.Log("premuto");
            p.SwitchState(p.kickState);
        }
    }
    public override void StateExit(BallInteractionsFSM p)
    {
        
    }
}