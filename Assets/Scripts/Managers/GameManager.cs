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

    public void SetPlayer1(Player plr1)
    {
        player1 = plr1;
    }

    public void SetPlayer2(Player plr2)
    {
        player2 = plr2;
    }

    public void DeactivateAllInputs()
    {
        if(player1 != null)
            player1.plrInp.DeactivateInput();
        if(player2 != null)
        player2.plrInp.DeactivateInput();
        // TODO: disattiva input UI        
    }

    public void ActivateAllInputs()
    {
        if(player1 != null)
            player1.plrInp.ActivateInput();
        if(player2 != null)
            player2.plrInp.ActivateInput();
        // TODO: attiva input UI                
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
