using System;
using System.Collections;
using System.Text.RegularExpressions;
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
    public int SCORE_TO_WIN { get; private set; }

    [SerializeField]
    public bool shouldAssignPlayers;
    public int player1Score { get; private set;}
    public int player2Score { get; private set; }

    

    // -------------------------------------------
    // ! Events
    // -------------------------------------------
    // ! --- Match Related
    // ? --- Flag per evitarmi di riavviare 12 volte
    // ? --- per cambiare una riga di codice 
    [SerializeField]
    private bool areWeTesting;

    // ? --- Servira' alla ui per capire quando far comparire
    // ? --- la schermata di prematch
    [NonSerialized]
    public UnityEvent<float> onPreMatchShowRivals = new();
    
    [field: SerializeField]
    public float preMatchShowRivalsDuration { get; private set; }

    [NonSerialized]
    public UnityEvent onPreMatchAssignDevices = new();
    public IEnumerator PreMatchAssignDevices()
    {
        AssignPlayers();

        // ? --- Aspetta fino a che non si
        while(!GameManager.Get().IsPlayersAssigned())
        {
            yield return null;
        }
        
        onPreMatchAssignDevices.Invoke();
        
    }

    [NonSerialized]
    public UnityEvent onMatchBegin = new();

    /// <summary>
    /// Routine completa di inizio round PER LA PRIMA VOLTA
    /// (ovvero, non ho riavviato il match)
    /// </summary>
    /// Protezione da restart coroutiune non necessaria.
    public void StartFirstTimeFlow()
    {
        StartCoroutine(StartFirstTimeFlowRoutine());
    }

    private IEnumerator StartFirstTimeFlowRoutine()
    {
        PowUtility.Log("Match: StartFirstTimeFlow", Color.cyan);

        if(shouldAssignPlayers)
        {
            InputManagerLogic.Get().ActivateAllInputs();
            onPreMatchAssignDevices.Invoke();
            while(!GameManager.Get().IsPlayersAssigned())
            {
                yield return null;
            }
        }

        if(!areWeTesting)
        {
            onPreMatchShowRivals.Invoke(preMatchShowRivalsDuration);
            yield return new WaitForSeconds(preMatchShowRivalsDuration);            
        }


        PowUtility.Log("Match: Match Begin", Color.cyan);

        
        onMatchBegin.Invoke();
        RoundManager.Get().RoundStart();
    }

    /// <summary>
    /// Invochera' passando come parametro il player vincitore.
    /// </summary>
    [NonSerialized]
    public UnityEvent<Player> onMatchEnd = new();
    public void MatchEnd()
    {
        GameManager g = GameManager.Get();
        g.player1Goal.goalColl.SetCollider(false);
        g.player2Goal.goalColl.SetCollider(false);

        InputManagerLogic.Get().DeactivateAllPlayerMap();


        if(player1Score >= SCORE_TO_WIN)
        {
            onMatchEnd.Invoke(g.player1);   
        }
        else if (player2Score >= SCORE_TO_WIN)
        {
            onMatchEnd.Invoke(g.player2);
        }
    }

    [NonSerialized]
    public UnityEvent onMatchRestart = new();

    public void MatchRestart()
    {
        PowUtility.Log("Match: Restarting!", Color.cyan);


        // ? --- TODO: teoricamente non dovrei usarla
        // ? --- ma siccome dovrei reiscrivere la UI all'evento
        // ? --- onMatchRestart mi rompo il cazzo e lo faccio.
        onMatchBegin.Invoke();

        player1Score = 0;
        player2Score = 0;

        RoundManager.Get().RoundStart();

        onMatchRestart.Invoke();
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
    bool CheckMatchEndConditions()
    {
        if(player1Score >= SCORE_TO_WIN)
        {
            PowUtility.Log("Player 1 Won", Color.yellow);
            return true;
        }
        if(player2Score >= SCORE_TO_WIN)
        {
            PowUtility.Log("Player 2 Won", Color.yellow);
            return true;
        }

        return false;
    }




    // -------------------------------------------
    // ! Unity Methods
    // -------------------------------------------
    void Awake()
    {
        InitSingleton();
    }

    void Start()
    {        
        StartFirstTimeFlow();
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