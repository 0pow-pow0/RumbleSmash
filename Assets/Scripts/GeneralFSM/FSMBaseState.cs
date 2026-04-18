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
public abstract class FSMBaseState<T>
{
    // ? Nome dello stato utilizzato per il debugging
    public string _d_stateName { get; private set; }

    public FSMBaseState(string str)
    {
        _d_stateName = str;
    }

    // ? Flag per sapere se lo stato e' attivo
    // ? si puo' anche semplicemente utilizzare un operatore di comparazione
    // ? con la classe
    // ? Esempio: if(currentState is RunState)

    public bool isActive = false;
    public virtual bool CanEnterState(FSM<T> p)
    {
        return true;
    }
    public abstract void StateEnter(FSM<T> p);
    public abstract void StateUpdate(FSM<T> p);
    public virtual void StateFixedUpdate(FSM<T> p) { }
    public abstract void StateExit(FSM<T> p);

    public virtual void OnTriggerEnter(FSM<T> p, Collider other) { }
    public virtual void OnTriggerStay(FSM<T> p, Collider other) { }
    public virtual void OnTriggerExit(FSM<T> p, Collider other) { }
    
    public virtual void OnCollisionEnter(FSM<T> p, Collider other) { }
    public virtual void OnCollisionStay(FSM<T> p, Collider other) { }
    public virtual void OnCollisionExit(FSM<T> p, Collider other) { }
    
    public virtual void OnCollisionEnter2D(FSM<T> p, Collision2D other) { }
    public virtual void OnCollisionStay2D(FSM<T> p, Collision2D other) { }
    public virtual void OnCollisionExit2D(FSM<T> p, Collision2D other) { }
    
}
  