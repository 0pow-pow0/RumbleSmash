using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraEffects : MonoBehaviour
{
    void Start()
    {
        GameManager g = GameManager.Get();

        // -------------------------------------------
        // ! Goal
        // -------------------------------------------
        g.player1Goal.onShieldDestroy.AddListener(
            () =>
            {
                transform.DOShakePosition(0.5f ,0.03f);
            }
        );    
        g.player2Goal.onShieldDestroy.AddListener(
            () =>
            {
                transform.DOShakePosition(0.5f ,0.03f);
            }
        ); 

        // -------------------------------------------
        // ! Ball
        // -------------------------------------------
        // g.ball.onImpactFrameStart.AddListener(
        //     () =>
        //     {
        //         transform.DOShakePosition(0.5f, 0.03f);
        //     }
        // );

        g.ball.onBallScore.AddListener(
            () =>
            {
                transform.DOShakePosition(1f, 0.03f);
                
            }
        );
    }

    void Update()
    {
    }

}
