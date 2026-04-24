using UnityEngine;
using TMPro;

public class UIScoreboard : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    TextMeshProUGUI textPlayer1Score;
    [SerializeField]
    TextMeshProUGUI textPlayer2Score;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MatchManager.Get().onPlayer1Score.AddListener(
            SetPlayer1Points
        );

        
        MatchManager.Get().onPlayer2Score.AddListener(
            SetPlayer2Points
        );

        SetPlayer1Points(0);
        SetPlayer2Points(0);
    }

    void SetPlayer1Points(int totalPoints)
    {
        textPlayer1Score.text = "" + totalPoints;
    }

    void SetPlayer2Points(int totalPoints)
    {
        textPlayer2Score.text = "" + totalPoints;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
