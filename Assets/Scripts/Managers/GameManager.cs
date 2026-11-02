using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    public Ball ball;

    [SerializeField] 
    public Player player1;
    [SerializeField] 
    public Player player2;

    [Header("Gameplay Stats"), SerializeField]
    public Goal player1Goal;
    
    [SerializeField]
    public Goal player2Goal;

    bool hasBeenInitialized = false;
    public void InitializeReferences(
        Ball blRef,
        Player player1Ref,
        Player player2Ref,
        Goal player1GoalRef,
        Goal player2GoalRef)
    {
        if(hasBeenInitialized)
        {
            
            Debug.Log("The GameManager has already been initialized");
        }

        ball = blRef;
        player1 = player1Ref;
        player2 = player2Ref;
        player1Goal = player1GoalRef;
        player2Goal = player2GoalRef;
    }

    /// <summary>
    /// Check per vedere se i player sono assegnati.       
    /// </summary>
    /// <returns>Vero se ENTRAMBI i player sono assegnati</returns>
    public bool IsPlayersAssigned()
    {
        if(player1 == null ||
            player2 == null)
        {
            return false;
        }

        return true;    
    }

    public void SetPlayer1(Player plr1)
    {
        player1 = plr1;
    }

    public void SetPlayer2(Player plr2)
    {
        player2 = plr2;
    }


    // -------------------------------------------
    // ! Singleton shit
    // -------------------------------------------
    private static GameManager inst;

    public static GameManager Get()
    {
        if(inst == null)
        {
            Debug.LogError("GameManager non instanziato!");    
            return null;
        }

        return inst;
    }

    void InitSingleton()
    {
        if(inst != null)
        {
            Debug.LogError("GameManager gia' instanziato");
        }
        else
        {
            inst = this;
        }
    }

    void Awake()
    {
        InitSingleton();
    }


    void Start()
    {
        ball.onImpactFrameStart.AddListener(
            () =>
            {
                Time.timeScale = 0f;
            }
        );

        ball.onImpactFrameEnd.AddListener(
            () =>
            {
                Time.timeScale = 1f;
            }
        );
    }
}
