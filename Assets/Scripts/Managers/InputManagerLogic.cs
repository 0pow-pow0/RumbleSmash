using System;
using System.Linq.Expressions;
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
    public UnityEvent onPlayer1Joined = new();
    [NonSerialized]
    public UnityEvent onPlayer2Joined = new();
    [NonSerialized]
    public UnityEvent onRestartDeviceAssignment = new();



    public void Awake()
    {
        InitSingleton();
        InputManagerLogic.Get();

        manager = GetComponent<PlayerInputManager>();

        manager.onPlayerJoined += OnPlayerJoined; 

        //GameManager.Get().ball.StopBallMovement();
        //manager.playerPrefab = cazzo;
    }

    public void OnPlayerJoined(PlayerInput player)
    {   
        Debug.Log("Pisello ");
        if(GameManager.Get().player1 == null)
        {
            GameManager.Get().SetPlayer1( 
                player.gameObject.GetComponent<Player>()
            );

            //  GameManager.Get().player1.plrInp.
                //DeactivateInput();
            Player p = player.gameObject.GetComponent<Player>();
            p.plrNumber = PlayerNumber.PLAYER_1;
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
            
            Player p = player.gameObject.GetComponent<Player>();
            p.plrNumber = PlayerNumber.PLAYER_2;

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

    public void DeactivateAllPlayerMap()
    {
        GameManager g = GameManager.Get();
        if(g.player1 != null)
        {
            InputActionMap aMp = 
                g.player1.plrInp.actions.FindActionMap("Player");

            if(aMp != null)
            {
                aMp.Disable();
            }
        }

        if(g.player2 != null)
        {
            InputActionMap aMp = 
                g.player2.plrInp.actions.FindActionMap("Player");

            if(aMp != null)
            {
                aMp.Disable();
            }
        }
    }

    public void ActivateAllPlayerMap()
    {
        GameManager g = GameManager.Get();
        if(g.player1 != null)
        {
            InputActionMap aMp = 
                g.player1.plrInp.actions.FindActionMap("Player");

            if(aMp != null)
            {
                aMp.Enable();
            }
        }

        if(g.player2 != null)
        {
            InputActionMap aMp = 
                g.player2.plrInp.actions.FindActionMap("Player");

            if(aMp != null)
            {
                aMp.Enable();
            }
        }
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
