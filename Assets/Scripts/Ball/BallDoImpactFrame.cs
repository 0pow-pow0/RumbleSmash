using UnityEngine;
using UtilityShit;

/// <summary>
/// Stato in cui la palla si ferma per creare il feedback
/// di impact frame
/// </summary>
public class BallDoImpactFrame : FSMBaseState<BallFSM>
{
    Ball bl;

    int framesPassed;
    public int framesToWait = 5;
    public FSMBaseState<BallFSM> preaviousState;



    public BallDoImpactFrame(Ball _bl) :
        base("ImpactFrame")
    {
        bl = _bl;
    }

    public void SetValues(
        int _framesToWait,
        FSMBaseState<BallFSM> _preaviousState)
    {
        framesToWait = _framesToWait;
        preaviousState = _preaviousState;
    }

    public override bool CanEnterState(FSM<BallFSM> p)
    {
        return true;
    }
    public override void StateEnter(FSM<BallFSM> p)
    {       
        framesPassed = 0;
        bl.StopBallMovement();

        bl.onImpactFrameStart.Invoke();
    }   

    public override void StateUpdate(FSM<BallFSM> p)
    {

        if(framesPassed > framesToWait)
        {
            p.Get().SwitchState(preaviousState);
            return;
        }

        framesPassed++;
    }

    public override void StateExit(FSM<BallFSM> p)
    {
        bl.StartBallMovement();
    }

}