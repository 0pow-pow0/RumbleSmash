using UnityEngine;
using UtilityShit;

public class BallStage2 : FSMBaseState<BallFSM>
{
    Ball bl;

    // ? --- Allo scadere setta la flag a true
    Timer minTimePassedTimer;
    

    public BallStage2(Ball _bl) :
        base("Stage 2")
    {
        bl = _bl;
        minTimePassedTimer = new Timer(bl.ballGameplayStats.STAGE2_MIN_TIME);
    }

    public override bool CanEnterState(FSM<BallFSM> p)
    {
        if(bl.isMovementStopped)    
            return false;

        return true;
    }
    public override void StateEnter(FSM<BallFSM> p)
    {
        bl.spriteRenderer.color = bl.ballGameplayStats.stage2Color;
        bl.outlineScr.OutlineColor = bl.ballGameplayStats.stage2Color;

        bl.rb.gravityScale = bl.ballGameplayStats.STAGE2_GRAVITY_SCALE;

        // ? --- Per evitare che deceleri i primi secondi
        bl.rb.sharedMaterial = bl.ballGameplayStats.stage2Bounciness;
        
        // ? --- Reinizializza sempre, cosi' se cambia il valore della
        // ? --- a cui mi appoggio il timer lo sa.
        minTimePassedTimer.ChangeMaxTime(bl.ballGameplayStats.STAGE2_MIN_TIME);
        minTimePassedTimer.Restart();        
        
        hasImpactedFramedFromCollision = false;

        bl.damage = bl.ballGameplayStats.STAGE2_DAMAGE;


        // ? --- Escludi ghostPlatform
        bl.physicsColl.excludeLayers = 
            bl.physicsColl.excludeLayers 
            | LayerMask.GetMask("GhostPlatform");


        bl.onBallStage1Start.Invoke();  
    }   

    public override void StateUpdate(FSM<BallFSM> p)
    {
        minTimePassedTimer.UpdateTime();
            
        
        if (bl.rb.linearVelocity.magnitude < 
                bl.ballGameplayStats.STAGE2_MIN_MAGNITUDE && minTimePassedTimer.HasEnded())
        {
            p.Get().SwitchState(p.Get().stage1);
            return; 
        }

        if(bl.rb.linearVelocity.magnitude > 
            bl.ballGameplayStats.STAGE3_MIN_MAGNITUDE)
        {
            p.Get().SwitchState(p.Get().stage3);
            return;
        }

        // ? --- Appena passa il tempo minimo 
        // ? --- la palla incomincia a rallentare
        if(minTimePassedTimer.HasEnded())
        {
            bl.rb.sharedMaterial = bl.ballGameplayStats.stage1Bounciness;
        }
    }

    public override void StateExit(FSM<BallFSM> p)
    {
        bl.onBallStage2End.Invoke();
    }

    #region ImpactFrames
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

        // ? --- Impact Frame
        if(other.gameObject.layer == 
            LayerMask.NameToLayer("LevelCollider"))
        {
            if(!hasImpactedFramedFromCollision
                && p.Get()._preaviousState is not BallStage3)
            {
                hasImpactedFramedFromCollision = true;
                
                bl.StartImpactFrames(bl.ballGameplayStats.STAGE2_FIRST_COLL_IF);
            }
        }
    }
    #endregion
}
