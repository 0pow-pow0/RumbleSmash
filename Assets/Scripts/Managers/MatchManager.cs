using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MatchManager : MonoBehaviour
{
    [Header("Gameplay Stats")]
    public int player1Score { get; private set;}
    public int player2Score { get; private set; }

    

    // -------------------------------------------
    // ! Events
    // -------------------------------------------
    [NonSerialized]
    UnityEvent matchBegin = new();

    [NonSerialized]
    UnityEvent matchEnd = new();


    public void MatchBegin()
    {
        matchBegin.Invoke();
    }

    public void MatchEnd()
    {

        matchEnd.Invoke();
    }

    void Start()
    {
        // TODO Ottimizza
        UI_InputHandlingScreen ui_i = 
            FindAnyObjectByType<UI_InputHandlingScreen>();
        
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
            RoundManager.Get().RoundStart();
        }
    }

    
}