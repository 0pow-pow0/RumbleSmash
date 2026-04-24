using System.Runtime.InteropServices;
using UnityEditor.Build.Content;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] 
    public Ball ball { get; private set; }

    [field: SerializeField] 
    public Player player1 { get; private set; }
    [field: SerializeField] 
    public Player player2 { get; private set; }

    [field: Header("Gameplay Stats"), SerializeField]
    public Goal player1Goal { get; private set;}
    
    [field: SerializeField]
    public Goal player2Goal { get; private set; }

    /// <summary>
    /// Check per vedere se i player sono assegnati.       
    /// </summary>
    /// <returns>Vero se ENTRAMBI i player sono assegnati</returns>
    public bool IsPlayersAssigned()
    {
        if(player1 != null ||
            player2 != null)
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

}
