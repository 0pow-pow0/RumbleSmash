using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BallFSM : FSM<BallFSM>
{
    Ball bl;

    public BallStage1 stage1;
    public BallStage2 stage2;
    public BallStage3 stage3;
    public BallDoImpactFrame doImpactFrame;

    public BallFSM() { }

    public void Awake()
    {
        bl = GameManager.Get().ball;

        stage1 = new BallStage1(bl);
        stage2 = new BallStage2(bl);
        stage3 = new BallStage3(bl);
        doImpactFrame = new BallDoImpactFrame(bl);  
        castedFather = this;

        _currentState = stage1;
    }

    public void DecreaseState()
    {
        if(_currentState is BallStage1)
        {
            
        }
        else if (_currentState is BallStage2)
        {
            SwitchState(stage1);
        }
        else if (_currentState is BallStage3)
        {
            SwitchState(stage2);
        }
    }

    public new void Update()
    {
        base.Update();
    }
}
