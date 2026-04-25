using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UtilityShit;

[RequireComponent(typeof(PlayerInputManager))]
public class InputManagerLogic : MonoBehaviour
{
    PlayerInputManager manager;

    
    [NonSerialized]
    public UnityEvent onPlayer1Joined;
    [NonSerialized]
    public UnityEvent onPlayer2Joined;
    [NonSerialized]
    public UnityEvent onRestartDeviceAssignment;



    public void Awake()
    {
        InitSingleton();
        InputManagerLogic.Get();

        onPlayer1Joined = new UnityEvent();
        onPlayer2Joined = new UnityEvent();
        onRestartDeviceAssignment = new UnityEvent();

        manager = GetComponent<PlayerInputManager>();

        manager.onPlayerJoined += OnPlayerJoined; 

        //GameManager.Get().ball.StopBallMovement();
        //manager.playerPrefab = cazzo;
    }

    public void OnPlayerJoined(PlayerInput player)
    {   

        if(GameManager.Get().player1 == null)
        {
            GameManager.Get().SetPlayer1( 
                player.gameObject.GetComponent<Player>()
            );

            //  GameManager.Get().player1.plrInp.
                //DeactivateInput();

            PowUtility.Log("Player 1 joined!", Color.blue);
            onPlayer1Joined.Invoke();

            // TODO: cambia prefab al player2


        }

        else if (GameManager.Get().player2 == null)
        {
            GameManager.Get().SetPlayer2( 
                player.gameObject.GetComponent<Player>()
            );            

            GameManager.Get().player2.plrInp.
                DeactivateInput();

            PowUtility.Log("Player 2 joined!", Color.yellow);
            onPlayer2Joined.Invoke();
        }
    }

    /// <summary>
    /// Impossibile da realizzare senza un'architettura decente
    /// </summary>
    public void RestartDeviceAssignment()
    {
        GameManager g = GameManager.Get();
        if(g.player1 != null)
        {
            Destroy(g.player1.gameObject);
        }
        if(g.player2 != null)
        {
            Destroy(g.player2.gameObject);
        }
        
        UIScreensManagerPow.Get().inputHandlingScreen.SetInitialState();
    }

    public void DeactivateAllInputs()
    {
        GameManager g = GameManager.Get();
        if(g.player1 != null)
            g.player1.plrInp.DeactivateInput();
        if(g.player2 != null)
            g.player2.plrInp.DeactivateInput();
        // TODO: disattiva input UI        
    }

    public void ActivateAllInputs()
    {
        GameManager g = GameManager.Get();
        if(g.player1 != null)
            g.player1.plrInp.ActivateInput();
        if(g.player2 != null)
            g.player2.plrInp.ActivateInput();
        // TODO: attiva input UI                
    }

    void Update()
    {
    }



    // -------------------------------------------
    // ! Singleton shit
    // -------------------------------------------
    private static InputManagerLogic inst;

    public static InputManagerLogic Get()
    {
        if(inst == null)
        {
            Debug.LogError("InputManagerLogic non instanziato!");    
            return null;
        }

        return inst;
    }

    void InitSingleton()
    {
        if(inst != null)
        {
            Debug.LogError("InputManagerLogic gia' instanziato");
        }
        else
        {
            inst = this;
        }
    }
}
