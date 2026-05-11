using System;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UtilityShit;

[RequireComponent(typeof(PlayerInputManager))]
public class SingleplayerInputManager : MonoBehaviour
{
    PlayerInputManager manager;
    
    [NonSerialized]
    public UnityEvent onPlayer1Joined = new();

    


    public void Awake()
    {
        InitSingleton();

        manager = GetComponent<PlayerInputManager>();

        manager.onPlayerJoined += OnPlayerJoined; 

        
    }

    void Start()
    {
        
        //TargetManager.Get().enabled = false;
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

            //TargetManager.Get().enabled = true;
        }
    }


    public void DeactivatePlayerMap()
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
    }

    public void ActivatePlayerMap()
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
    }

    public void DeactivateAllInputs()
    {
        GameManager g = GameManager.Get();
        if(g.player1 != null)
            g.player1.plrInp.DeactivateInput();
    }

    public void ActivateAllInputs()
    {
        GameManager g = GameManager.Get();
        if(g.player1 != null)
            g.player1.plrInp.ActivateInput();  
    }

    void Update()
    {
        
    }



    // -------------------------------------------
    // ! Singleton shit
    // -------------------------------------------
    private static SingleplayerInputManager inst;

    public static SingleplayerInputManager Get()
    {
        if(inst == null)
        {
            Debug.LogError("SingleplayerInputManager non instanziato!");    
            return null;
        }

        return inst;
    }

    void InitSingleton()
    {
        if(inst != null)
        {
            Debug.LogError("SingleplayerInputManager gia' instanziato");
        }
        else
        {
            inst = this;
        }
    }
}
