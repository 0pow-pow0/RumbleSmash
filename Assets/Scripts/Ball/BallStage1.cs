using UnityEngine;

// ? --- Questo stato non ha impact frames.
public class BallStage1 : FSMBaseState<BallFSM>
{
    Ball bl;
    float timePassedInThisStage = 0f;
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
        bl.spriteRenderer.color = bl.ballGameplayStats.stage1Color;   
        bl.outlineScr.OutlineColor = bl.ballGameplayStats.stage1Color;

        bl.rb.gravityScale = bl.ballGameplayStats.STAGE1_GRAVITY_SCALE;
        bl.rb.sharedMaterial = bl.ballGameplayStats.stage1Bounciness;

        bl.damage = bl.ballGameplayStats.STAGE1_DAMAGE;

        // ? --- Includi ghostPlatform
        bl.physicsColl.excludeLayers = 
            bl.physicsColl.excludeLayers &
            ~LayerMask.GetMask("GhostPlatform");

        timePassedInThisStage = 0f;

        bl.onBallStage1Start.Invoke();
    }

    public override void StateUpdate(FSM<BallFSM> p)
    {
        // DEBUG !TODO 
        // E' una riga utile solo per debugging purposes
        timePassedInThisStage += Time.deltaTime;
        if(timePassedInThisStage <= 3f)
        {
            return;
        }

        if(bl.rb.linearVelocity.magnitude >= bl.ballGameplayStats.STAGE2_MIN_MAGNITUDE)
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
