using System;
using System.ComponentModel;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using UtilityShit;

public enum RoundState
{
    START,
    MID,
    END,
    NONE
}

/// <summary>
/// L'update viene gestito dal match manager
/// TODO: Sposta dentro MatchManager
/// </summary>
public class RoundManager : MonoBehaviour
{
    [field: Header("References"), SerializeField]
    public GameObject spawnpointPlayer1 { get; private set; }
    
    [field: SerializeField]
    public GameObject spawnpointPlayer2 { get; private set; }
    
    [field: SerializeField]
    public GameObject spawnpointBall { get; private set; }

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
    [NonSerialized]
    public UnityEvent<float> onRoundStartCountdown = new();

    /// <summary>
    /// Inizio vero e proprio del round
    /// </summary>
    [NonSerialized]
    public UnityEvent onRoundStartBegin = new();

    [NonSerialized]
    public UnityEvent onRoundEnd = new();


    void Awake()
    {
        InitSingleton();
        roundState = RoundState.NONE;    
    }


    public void RoundStart()
    {
        roundState = RoundState.START;
        PowUtility.Log("RoundManager: Round countdown", Color.cyan);
        InputManagerLogic.Get().DeactivateAllInputs();

        GameManager g = GameManager.Get();

        g.player1.transform.position = 
            spawnpointPlayer1.transform.position;
        g.player1.Reset();
        g.player2.transform.position =
            spawnpointPlayer2.transform.position;
        g.player2.Reset();
        g.ball.transform.position =
            spawnpointBall.transform.position;
        g.ball.Reset();
        

        g.player1Goal.SetScoreCollider(true);
        g.player2Goal.SetScoreCollider(true);

    
        Time.timeScale = 1f;

        onRoundStartCountdown.Invoke(START_ROUND_COUNTDOWN_DURATION);

        PowUtilityU.Get().DelayAction
        (
            (() =>
            {
                roundState = RoundState.MID;
                
                InputManagerLogic.Get().ActivateAllInputs();

                onRoundStartBegin.Invoke();
                PowUtility.Log("RoundManager: Round begin!", Color.cyan);

            }),
            START_ROUND_COUNTDOWN_DURATION
        );

    }

    public void RoundEnd()
    {
        roundState = RoundState.END;
        //InputManagerLogic.Get().DeactivateAllInputs();
        
        Time.timeScale = 0.5f;

        GameManager g = GameManager.Get();
                
        g.player1Goal.SetScoreCollider(false);
        g.player2Goal.SetScoreCollider(false);

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