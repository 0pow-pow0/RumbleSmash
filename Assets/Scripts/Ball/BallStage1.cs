using UnityEngine;

// ? --- Questo stato non ha impact frames.
public class BallStage1 : FSMBaseState<BallFSM>
{
    Ball bl;
    public BallStage1(Ball _bl) :
        base("Stage 1")
    {
        bl = _bl;
    }

    public override bool CanEnterState(FSM<BallFSM> p)
    {
        if(bl.isMovementStopped)
            return false;   

        return true;
    }
    public override void StateEnter(FSM<BallFSM> p)
    {
        bl.spriteRenderer.color = Color.gray;   
        bl.rb.gravityScale = bl.STAGE1_GRAVITY_SCALE;
        bl.rb.sharedMaterial = bl.hardBounciness;

        bl.onBallStage1Start.Invoke();
    }

    public override void StateUpdate(FSM<BallFSM> p)
    {
        if(bl.rb.linearVelocity.magnitude >= bl.STAGE2_MIN_MAGNITUDE)
        {
            p.Get().SwitchState(p.Get().stage2);
            return;
        }
    }
    

    public override void StateExit(FSM<BallFSM> p)
    {
        bl.onBallStage1End.Invoke();    
    }

    public override void OnCollisionEnter2D(
        FSM<BallFSM> p, 
        Collision2D other) 
    {
        if(other.gameObject.layer ==
            LayerMask.NameToLayer("Goal"))
        {
            
        }
    }

}
