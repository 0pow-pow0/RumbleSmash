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
    

    
    #region Gameplay Stats
    [SerializeField, ReadOnly] string activeState;
    #endregion


    [FoldoutGroup("Flags")]
    [SerializeField] private EditorAttributes.Void flagsGroup;

    //bool isShooting;
    public BallInteractionsFSM fsm { get; private set;  }

    public void Reset()
    {
        fsm.DirtySwitch(fsm.awaitState);
    }



    // -------------------------------------------
    // ! Events
    // -------------------------------------------
    #region Events
    [NonSerialized] public UnityEvent onKickStart = new();
    [NonSerialized] public UnityEvent onKickEnd = new();

    [NonSerialized] public UnityEvent onChargeStart = new();
    [NonSerialized] public UnityEvent onCharging = new();
    [NonSerialized] public UnityEvent onChargeEnd = new();
    // ? --- Passa il punto di collisione con la palla
    [NonSerialized] public UnityEvent<Vector2> onBallHit = new();
    #endregion



    void Awake()
    {
        fsm = new BallInteractionsFSM();
        fsm.pbi = this;
        
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