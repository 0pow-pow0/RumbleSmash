using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class UIScoreboard : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    GameObject father;
    [SerializeField]
    TextMeshProUGUI textPlayer1Score;
    [SerializeField]
    TextMeshProUGUI textPlayer2Score;



    void Awake()
    {
        father.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MatchManager.Get().onPlayer1Score.AddListener(
            SetPlayer1Points
        );

        
        MatchManager.Get().onPlayer2Score.AddListener(
            SetPlayer2Points
        );

        MatchManager.Get().onPreMatchAssignDevices.AddListener(
            () =>
            {
                father.SetActive(false);
            }
        );

        // ? --- Se bypassiamo l'assegnazione dei player, in ogni caso
        // ? --- la scoreboard compare.
        MatchManager.Get().onMatchBegin.AddListener(
            () =>
            {
                SetInitialState();
            }
        );


        SetPlayer1Points(0);
        SetPlayer2Points(0);
    }

    void SetInitialState()
    {
        father.SetActive(true);
        textPlayer1Score.text = "0";
        textPlayer1Score.gameObject.SetActive(true);
        textPlayer2Score.text = "0";
        textPlayer2Score.gameObject.SetActive(true);
    }

    void SetPlayer1Points(int totalPoints)
    {
        textPlayer1Score.text = "" + totalPoints;
    }

    void SetPlayer2Points(int totalPoints)
    {
        textPlayer2Score.text = "" + totalPoints;

    }

}
