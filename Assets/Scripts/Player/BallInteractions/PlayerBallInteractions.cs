using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/// <summary>
/// ? Il behaviour di questa classe si trova all'interno degli stati
/// ? gestiti dalla FSM, questa classe contiene dunque solo i dati utili
/// ? per i behaviours
/// </summary>
public class PlayerBallInteractions : MonoBehaviour
{
    [Header("References")]
    public Player plr;
    [NonSerialized] public InputAction kickInput;
    

    
    [Header("Gameplay Stats")]
    [SerializeField, ReadOnly] string activeState;
    [field:SerializeField] public int KICK_FORCE { get; private set; }
    
    // ? --- La forza del calcio quando viene chargato al massimo.
    // ? --- Il calcolo della forza finale che verra' applicato 
    // ? --- alla palla:
    // ? --- KICK_FORCE + ((tempo di prezzione[da 0 a 1]) * KICK_FORCE_CHARGED_MAX)
    [field:SerializeField] public int KICK_FORCE_CHARGED_MAX { get; private set; }



    // -------------------------------------------
    // ! Timing 
    // -------------------------------------------
    // ? --- Tempo di pressione del tasto del CALCIO necessario per 
    // ? --- raggiungere il massimo della carica del CALCIO.
    [field: Space(10), SerializeField] 
    public float TIME_TO_REACH_MAX_CHARGE { get; private set; }

    // ? --- Tempo di pressione necessaria prima 
    // ? --- di CARICA che il giocatore entri in stato di CARICA
    [field:SerializeField] 
    public float CHARGE_WINDOW { get; private set; }

    [field:SerializeField] 
    public int KICK_COLLIDER_FRAME_DURATION { get; private set; }

    [FoldoutGroup("Flags")]
    [SerializeField] private EditorAttributes.Void flagsGroup;

    //bool isShooting;
    BallInteractionsFSM fsm;

    public void Reset()
    {
        fsm.DirtySwitch(fsm.awaitState);
    }

    // -------------------------------------------
    // ! Events
    // -------------------------------------------
    [NonSerialized] public UnityEvent onKickStart;
    [NonSerialized] public UnityEvent onKickEnd;

    [NonSerialized] public UnityEvent onChargeStart;
    [NonSerialized] public UnityEvent onCharging;
    [NonSerialized] public UnityEvent onChargeEnd;


    void Awake()
    {
        fsm = new BallInteractionsFSM();
        fsm.pbi = this;
        

        
        onKickStart = new UnityEvent();
        onKickEnd = new UnityEvent();
        onChargeStart = new UnityEvent();
        onCharging = new UnityEvent();
        onChargeEnd = new UnityEvent(); 
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        kickInput = plr.plrInp.actions.FindAction("Kick");
    }

    // Update is called once per frame
    void Update()
    {
        activeState = fsm._currentState._d_stateName;
        fsm.Update();
    }   

}