using UnityEngine;
using UtilityShit;

public class BallStage3 : FSMBaseState<BallFSM>
{
    Ball bl;

    // ? --- Tiene traccia del tempo passato in questo stato
    Timer minTimePassedTimer;
    public bool bypassMinTimer;

    public BallStage3(Ball _bl) :
        base("Stage 3")
    {
        bl = _bl;
        minTimePassedTimer = new Timer(bl.STAGE3_MIN_TIME);
    }

    public override bool CanEnterState(FSM<BallFSM> p)
    {
        if(bl.isMovementStopped)    
            return false;

        return true;
    }
    public override void StateEnter(FSM<BallFSM> p)
    {
        hasImpactedFramedFromCollision = false;

        bl.spriteRenderer.color = bl.stage3Color;
        bl.outlineScr.OutlineColor = bl.stage3Color;

        bl.rb.gravityScale = bl.STAGE3_GRAVITY_SCALE;

        // ? --- Per evitare che deceleri i primi secondi
        bl.rb.sharedMaterial = bl.endlessBounciness;

        minTimePassedTimer.Restart();        
        
        bl.damage = bl.STAGE3_DAMAGE;

        bl.onBallStage3Start.Invoke();  
    }   

    public override void StateUpdate(FSM<BallFSM> p)
    {
        minTimePassedTimer.UpdateTime();


        
        if(minTimePassedTimer.HasEnded())
        {
            Debug.Log("Stage3: timer ended");
            bl.rb.sharedMaterial = bl.veryHardBounciness;
        }
        
        if(bl.rb.linearVelocity.magnitude > bl.STAGE3_MAX_MAGNITUDE)
        {
            Vector2 normLV = bl.rb.linearVelocity.normalized;
            bl.rb.linearVelocity = new Vector2
            (
                normLV.x * bl.STAGE3_MAX_MAGNITUDE,
                normLV.y * bl.STAGE3_MAX_MAGNITUDE
            );

        } 
                 
        if (bl.rb.linearVelocity.magnitude < bl.STAGE3_MIN_MAGNITUDE)
        {
            Debug.Log("Stage3: switch to Stage2");
            p.Get().SwitchState(p.Get().stage2);
            return;
        }

    }

    

    public override void StateExit(FSM<BallFSM> p)
    {
        bypassMinTimer = false;
        bl.onBallStage3End.Invoke();
    }

    bool hasImpactedFramedFromCollision = false;
    public override void OnCollisionEnter2D(
        FSM<BallFSM> p,
        Collision2D other)
    {   
        if(other.gameObject.layer ==
            LayerMask.NameToLayer("Goal"))
        {
            p.SwitchState(p.Get().stage1);
        }

        if(other.gameObject.layer == 
            LayerMask.NameToLayer("LevelCollider"))
        {
            if(!hasImpactedFramedFromCollision)
            {
                hasImpactedFramedFromCollision = true;
                
                bl.StartImpactFrames(bl.STAGE3_FIRST_COLL_IF);
            }
        }
    }

}
