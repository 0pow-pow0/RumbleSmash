using System;
using EditorAttributes;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UtilityShit;

/// <summary>
/// Si occupera' di ogni cosa relativa al match, 
/// dall'input manager alla scoreboard.  
/// 
/// Gestisce anche le porte 
/// </summary>
public class MatchManager : MonoBehaviour
{
    [field: Header("Gameplay Stats"), SerializeField]
    public int ScoreToWin { get; private set; }

    public int player1Score { get; private set;}
    public int player2Score { get; private set; }

    

    // -------------------------------------------
    // ! Events
    // -------------------------------------------
    // ! --- Match Related
    [NonSerialized]
    public UnityEvent onMatchBegin = new();
    public void MatchBegin()
    {
        PowUtility.Log("Match: BeginningMatch", Color.cyan);

        //AssignPlayers();


        onMatchBegin.Invoke();
    }

    [NonSerialized]
    public UnityEvent onMatchEnd = new();
    public void MatchEnd()
    {
        SetGoals(false);
        InputManagerLogic.Get().DeactivateAllInputs();
                

        onMatchEnd.Invoke();
    }

    // ! --- Scoreboard Related
    // ? --- Passera' gli score dei player
    [NonSerialized]
    public UnityEvent<int> onPlayer1Score = new();
    
    [NonSerialized]
    public UnityEvent<int> onPlayer2Score = new();

    /// <summary>
    /// Chiamata dalle porte.
    /// </summary>
    /// <param name="toAddPoints">Quanto aggiungere allo score</param>
    public void ScorePlayer1(int toAddPoints)
    {
        player1Score += toAddPoints; 

        onPlayer1Score.Invoke(player1Score);   
        
        if(!CheckMatchEndConditions())
        {
            RoundManager.Get().RoundEnd();
        }
        else
        {
            MatchEnd();
        }
    }

    /// <summary>
    /// Chiamata dalle porte.
    /// </summary>
    /// <param name="toAddPoints">Quanto aggiungere allo score</param>
    public void ScorePlayer2(int toAddPoints)
    {
        player2Score += toAddPoints;


        onPlayer2Score.Invoke(player2Score);
        if(!CheckMatchEndConditions())
        {
            RoundManager.Get().RoundEnd();
        }
        else
        {
            MatchEnd();
        }
    }

    public void AssignPlayers()
    {
        if(!GameManager.Get().IsPlayersAssigned())
        {
            InputManagerLogic.Get().RestartDeviceAssignment();
            PowUtility.Log("Match: Assigning players", Color.cyan);
        }
    }

    void SetGoals(bool isActive)
    {
        GameManager.Get().player1Goal.gameObject.SetActive(isActive);
        GameManager.Get().player2Goal.gameObject.SetActive(isActive);
    }

    bool CheckMatchEndConditions()
    {
        if(player1Score >= ScoreToWin)
        {
            PowUtility.Log("Player 1 Won", Color.yellow);
            return true;
        }
        if(player2Score >= ScoreToWin)
        {
            PowUtility.Log("Player 2 Won", Color.yellow);
            return true;
        }

        return false;
    }





    void Awake()
    {
        InitSingleton();
    }

    void Start()
    {        
        MatchBegin();
    }

    void Update()
    {
        if(RoundManager.Get().roundState ==
            RoundState.MID)
        {
            RoundManager.Get().RoundUpdate();
        }

        if(Keyboard.current.mKey.wasPressedThisFrame)
        {
            ScorePlayer1(1);
        }
    }


    // -------------------------------------------
    // ! Singleton shit
    // -------------------------------------------
    private static MatchManager inst;

    public static MatchManager Get()
    {
        if(inst == null)
        {
            Debug.LogError("MatchManager non instanziato!");    
            return null;
        }

        return inst;
    }

    void InitSingleton()
    {
        if(inst != null)
        {
            Debug.LogError("MatchManager gia' instanziato");
        }
        else
        {
            inst = this;
        }
    }
}