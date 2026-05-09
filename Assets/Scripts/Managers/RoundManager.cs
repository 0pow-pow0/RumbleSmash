using System;
using System.ComponentModel;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

public enum RoundState
{
    START,
    MID,
    END,
    NONE
}

/// <summary>
/// Il beaviour viene gestito dal matchManager
/// </summary>
public class RoundManager : MonoBehaviour
{
    [Header("References"), SerializeField]
    GameObject spawnpointPlayer1;
    
    [SerializeField]
    GameObject spawnpointPlayer2;

    #region START_ROUND
    [Header("Start Round Vars"), SerializeField]
    float START_ROUND_COUNTDOWN_DURATION;
    #endregion

    #region END_ROUND
    [Header("End Round Vars"), SerializeField]
    float END_ROUND_DELAY_RESTART;
    #endregion


    [field: SerializeField, EditorAttributes.ReadOnly]
    public RoundState roundState { get; private set; } 
        = new();

    /// <summary>
    /// Quando il round inizia ma 
    /// i player non possono muoversi
    /// </summary>
    UnityEvent onRoundStartCountdown = new();

    /// <summary>
    /// Inizio vero e proprio del round
    /// </summary>
    UnityEvent onRoundStartBegin = new();

    UnityEvent onRoundEnd = new();


    void Awake()
    {
        InitSingleton();
        roundState = RoundState.NONE;    
    }


    public void RoundStart()
    {
        roundState = RoundState.START;
        Debug.Log("RoundManager: Round started");
        GameManager.Get().player1.
            plrInp.DeactivateInput();
        
        GameManager.Get().player2.
            plrInp.DeactivateInput();

        

        onRoundStartCountdown.Invoke();

        PowUtilityU.Get().DelayAction
        (
            () =>
            {
                GameManager.Get().player1.
                    plrInp.ActivateInput();
            
                GameManager.Get().player2.
                    plrInp.ActivateInput();


                onRoundStartBegin.Invoke();
                Debug.Log("RoundManager: Round begin!");

                roundState = RoundState.MID;
            },
            START_ROUND_COUNTDOWN_DURATION
        );

    }

    public void RoundEnd()
    {
        roundState = RoundState.END;
        //InputManagerLogic.Get().DeactivateAllInputs();
        
        Time.timeScale = 0.5f;

        GameManager g = GameManager.Get();
                
        g.player1Goal.goalColl.SetCollider(false);
        g.player2Goal.goalColl.SetCollider(false);

        g.ball.goalColl.enabled = false;

        onRoundEnd.Invoke();

        PowUtilityU.Get().DelayAction(
            RoundStart, END_ROUND_DELAY_RESTART);
    }

    public void RoundUpdate()
    {
        
    }



    // -------------------------------------------
    // ! Singleton Shit 
    // -------------------------------------------
    private static RoundManager inst;

    public static RoundManager Get()
    {
        if(inst == null)
        {
            Debug.LogError("RoundManager non instanziato!");    
            return null;
        }

        return inst;
    }

    void InitSingleton()
    {
        if(inst != null)
        {
            Debug.LogError("RoundManager gia' instanziato");
        }
        else
        {
            inst = this;
        }
    }
}