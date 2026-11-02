using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Classica Finite State Machine
/// Creiamo un'interfaccia virtuale che descrive 
/// dei metodi comuni a tutti gli stati possibili del player
/// C'e' un piccolo twist, ovvero la funzione AnyState
/// Che verra' eseguita INDIPENDENTEMENTE DALLO STATO ATTUALE
/// Usare con parsimonia, grz   
/// </summary>
public abstract class BaseState
{
    // ? Nome dello stato utilizzato per il debugging
    public string _d_stateName { get; private set; }

    public BaseState(string str)
    {
        _d_stateName = str;
    }

    // ? Flag per sapere se lo stato e' attivo
    // ? si puo' anche semplicemente utilizzare un operatore di comparazione
    // ? con la classe
    // ? Esempio: if(currentState is RunState)

    public bool isActive = false;
    public virtual bool CanEnterState(BallInteractionsFSM p)
    {
        return true;
    }
    public abstract void StateEnter(BallInteractionsFSM p);
    public abstract void StateUpdate(BallInteractionsFSM p);
    public abstract void StateExit(BallInteractionsFSM p);

    public virtual void OnTriggerEnter(BallInteractionsFSM p) { }
    public virtual void OnTriggerStay(BallInteractionsFSM p) { }
    public virtual void OnTriggerExit(BallInteractionsFSM p) { }
    
}
  