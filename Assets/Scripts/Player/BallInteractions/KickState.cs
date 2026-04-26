using UnityEngine;

public class KickState : BaseState
{
    float timePassedSinceInput = 0;

    // ? --- Se decidiamo di caricare il colpo
    // ? --- l'evento onKickEnd, non dev'essere richiamato
    bool isSwitchingToCharge = false;

    public KickState() : 
        base("Kick")
    {
        
    }

    public override bool CanEnterState(BallInteractionsFSM p)
    {
        return true;
    }
    public override void StateEnter(BallInteractionsFSM p)
    {        
        // ! --- Importantissimo, possiamo entrare in kick state
        // ! --- solo se non e' attivo il collider dell'ultima azione effettuata
        if(p.pbi.plr.ballCollider.gameObject.activeInHierarchy)
        {
           return; 
        }
        p.pbi.plr.DisableMovement();

        timePassedSinceInput = 0;
        isSwitchingToCharge = false;
    }
    public override void StateUpdate(BallInteractionsFSM p)
    {
        timePassedSinceInput += Time.deltaTime;

        // ? --- Se abbiamo premuto per un tempo necessario
        // ? --- andiamo in stato di carica
        if(timePassedSinceInput >= p.pbi.CHARGE_WINDOW)
        {
            isSwitchingToCharge = true;
            p.SwitchState(p.chargingState); 
        }

        if(p.pbi.kickInput.WasReleasedThisFrame())
        {
            // Attiva colliders
            // Torna in stato di AWAIT  
            // Attiva Events
            
            p.SwitchState(p.awaitState);
            p.pbi.onKickEnd.Invoke();

            // TODO --- Attiva collider e passando le forze fisiche necessarie
            p.pbi.plr.ballCollider.Activate
            (
                p.pbi.KICK_FORCE,
                p.pbi.plr.directionLastInput
            );   
        }
    }
    public override void StateExit(BallInteractionsFSM p)
    {
        p.pbi.plr.EnableMovement();

        if(!isSwitchingToCharge)
            p.pbi.onKickEnd.Invoke();
    }
}