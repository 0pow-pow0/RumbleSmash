using System;
using Unity.VisualScripting;
using UnityEngine;

public class ChargingState : BaseState
{
    // ? --- Il tempo che abbiamo passato a caricare il colpo
    float timePassedSinceStart;

    public ChargingState() : 
        base("Charge")
    {
        
    }

    public override bool CanEnterState(BallInteractionsFSM p)
    {
        return true;
    }
    public override void StateEnter(BallInteractionsFSM p)
    {
        timePassedSinceStart = 0f;
        p.pbi.onChargeStart.Invoke();
    }
    public override void StateUpdate(BallInteractionsFSM p)
    {
        timePassedSinceStart += Time.deltaTime;

        // ! --- Controlla se crea problemi
        int normalizedChargePower = 
            (int)Math.Clamp
            (
                timePassedSinceStart / p.pbi.TIME_TO_REACH_MAX_CHARGE,
                0f,
                1f
            ); 


        if(p.pbi.kickInput.WasReleasedThisFrame())
        {
            p.SwitchState(p.awaitState);


            // TODO --- Attiva collider e passando le forze fisiche necessarie
            p.pbi.plr.ballCollider.Activate
            (
                p.pbi.KICK_FORCE +
                normalizedChargePower *
                p.pbi.KICK_FORCE_CHARGED_MAX,
                p.pbi.plr.directionLastInput
            );   

        }
    }
    public override void StateExit(BallInteractionsFSM p)
    {
        p.pbi.onChargeEnd.Invoke();
    }
}
