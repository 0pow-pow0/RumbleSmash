using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIMatchAnimations : MonoBehaviour
{
    [field: Header("References"), SerializeField]   
    GameObject father;
    
    [SerializeField] 
    TextMeshProUGUI textEndMatchWinner;
    
    [SerializeField] 
    Image panelBackground;
    
    [SerializeField] 
    Button buttonRetry;
    [SerializeField] 
    Button buttonExit;


    void Start()
    {
        MatchManager.Get().onMatchEnd
            .AddListener
            (
                OnMatchEnd
            );  

        MatchManager.Get().onMatchBegin
            .AddListener
            (
                SetInitialState
            );
    }

    void SetInitialState()
    {
        father.SetActive(true);
        textEndMatchWinner.gameObject.SetActive(false);
        panelBackground.gameObject.SetActive(false);
        buttonRetry.gameObject.SetActive(false);
        buttonExit.gameObject.SetActive(false);
    }

    void OnMatchEnd(Player winner)
    {
        // ? --- Winner Text
        textEndMatchWinner.text = "";
        textEndMatchWinner.gameObject.SetActive(true);

        switch(winner.plrNumber)
        {
            case PlayerNumber.PLAYER_1:
                textEndMatchWinner.text = "Player 1 Won!";
                break;
            
            case PlayerNumber.PLAYER_2:
                textEndMatchWinner.text = "Player 2 Won!";
                break;

            default:
                textEndMatchWinner.text = "NOT SET Won!";
                break;
        }

        // ? --- Background Panel
        panelBackground.gameObject.SetActive(true);

        buttonRetry.gameObject.SetActive(true);
        buttonExit.gameObject.SetActive(true);
    }

    public void OnButtonRetryClick()
    {
        MatchManager.Get().onMatchRestart.Invoke();
    }

    public void OnButtonExitClick()
    {
        Application.Quit();
    }
}
