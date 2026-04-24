using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UtilityShit;

/// <summary>
/// Si occupera' di ogni cosa relativa al match, 
/// dall'input manager alla scoreboard.  
/// </summary>
public class MatchManager : MonoBehaviour
{
    [Header("Gameplay Stats")]
    public int player1Score { get; private set;}
    public int player2Score { get; private set; }

    

    // -------------------------------------------
    // ! Events
    // -------------------------------------------
    // ! --- Match Related
    [NonSerialized]
    public UnityEvent onMatchBegin = new();

    [NonSerialized]
    public UnityEvent onMatchEnd = new();


    // ! --- Scoreboard Related
    // ? --- Passera' gli score dei player
    [NonSerialized]
    public UnityEvent<int> onPlayer1Score = new();
    
    [NonSerialized]
    public UnityEvent<int> onPlayer2Score = new();
    void ScorePlayer1(int toAddPoints)
    {
        player1Score += toAddPoints; 

        onPlayer1Score.Invoke(player1Score);    
    }

    void ScorePlayer2(int toAddPoints)
    {
        player2Score += toAddPoints;

        onPlayer2Score.Invoke(player2Score);
    }

    public void AssignPlayers()
    {
        if(!GameManager.Get().IsPlayersAssigned())
        {
            InputManagerLogic.Get().RestartDeviceAssignment();
            PowUtility.Log("Match: Assigning players", Color.cyan);
        }
    }



    public void MatchBegin()
    {
        PowUtility.Log("Match: BeginningMatch", Color.cyan);

        //AssignPlayers();


        onMatchBegin.Invoke();
    }

    public void MatchEnd()
    {

        onMatchEnd.Invoke();
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