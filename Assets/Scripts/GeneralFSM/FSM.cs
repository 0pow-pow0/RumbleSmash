using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Finite state machine per gestire gli stati di qualunque entita'
/// E' necessario solo dare una reference del GameObject da cui si vogliono
/// ricevere informazioni utili per processare azioni nei codici dei singoli stati.
/// 
/// Modificata ad hoc per questo scenario
/// </summary>

public class FSM<T>
{
    // ? --- Puntatore a se' stesso ma castato
    // ? --- alla classe specializzata.
    protected T castedFather;

    public T Get()
    {
        return castedFather;
    }

    public FSMBaseState<T> _currentState { get; protected set; }
    public FSMBaseState<T> _preaviousState { get; protected set; }


    #region STATI CONCRETI


    #endregion

    protected FSM()
    {
        _currentState = null;
    }

    protected void Update()
    {
        //if (GameManager.isGamePaused) { return; }



        //Debug.Log("state: " + _currentState);
        _currentState.StateUpdate(this);
    }

    /// <summary>
    /// Lo switch state sara' possibile solo ed esclusivamente se il nuovo stato ritorna
    /// un valore true durante l'esecuzione dello StateEnter, se e' false semplicemente
    /// non si triggera ne' lo StateExit del vecchio stato ed e' come se non avessimo fatto alcuno switch.
    /// </summary>
    public bool SwitchState(FSMBaseState<T> newState)
    {
        if(newState.CanEnterState(this))
        {
            _currentState.StateExit(this);
            _currentState.isActive = false;

            _preaviousState = _currentState;
            _currentState = newState;
            _currentState.StateEnter(this); 
            _currentState.isActive = true;
            //GameManager.animState = _currentState._d_stateName;
        }

        return false;
    }

    /// <summary>
    /// Bypassa ogni CanEnter, StateExit e StateEnter;
    /// </summary>
    /// <param name="newState"></param>
    public void DirtySwitch(FSMBaseState<T> newState)
    {
        _currentState = newState;
    }
}
