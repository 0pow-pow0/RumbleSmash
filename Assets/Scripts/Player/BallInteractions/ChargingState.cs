using System;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class ChargingState : BaseState
{
    // ? --- Il tempo che abbiamo passato a caricare il colpo
    float timePassedSinceStart;

    bool hasTriggeredChargeEvent = false;

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
        p.pbi.plr.DisableMovement();
        timePassedSinceStart = 0f;
        hasTriggeredChargeEvent = false;
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

        if(!hasTriggeredChargeEvent &&
            timePassedSinceStart >= p.pbi.TIME_TO_REACH_MAX_CHARGE)
        {
            p.pbi.onChargeEnd.Invoke();
            hasTriggeredChargeEvent = true;
        }

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
        p.pbi.plr.EnableMovement();
        p.pbi.onKickEnd.Invoke();
        //p.pbi.onChargeEnd.Invoke();
    }
}
