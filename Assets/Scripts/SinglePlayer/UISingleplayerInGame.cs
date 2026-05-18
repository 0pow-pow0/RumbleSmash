using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UISingleplayerInGame : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textPoints;

    
    [SerializeField]
    TextMeshProUGUI textTimeLeft;
    [SerializeField]
    float startTime = 30f; 
    float passedTime;
    
    [SerializeField]
    TextMeshProUGUI textHighscore;

    PlayerSPVars plrScore;
    int preaviousHighscore;

    bool hasStarted = false;
    void Start()
    {
        GameManager g = GameManager.Get();

        TargetManager.Get().onTargetDestroy.AddListener(
            (obj) =>
            {
                textPoints.text = "Points: " + 
                    g.player1.GetComponent<PlayerSPVars>().scoredPoints;
            }
        );

        SingleplayerInputManager.Get().onPlayer1Joined.AddListener
        (
            () =>
            {
                plrScore = GameManager.Get().player1.GetComponent<PlayerSPVars>();
                hasStarted = true;
            }
        );

        preaviousHighscore = PowSceneManager.Get().singleplayerHighscore;
        textHighscore.text = "Get over " + preaviousHighscore + "!";

        passedTime = 0f;
    }

    void Update()
    {
        if(!hasStarted)
            return;

        passedTime += Time.deltaTime;
        textTimeLeft.text = "Time left: " + (int)(startTime - passedTime);

        if(passedTime >= startTime)
        {
            PowSceneManager.Get().singleplayerHighscore = 
                GameManager.Get().player1.GetComponent<PlayerSPVars>().scoredPoints;

            PowSceneManager.Get().ChangeScene("MainMenu");
        }

        if(plrScore.scoredPoints > preaviousHighscore)
        {
            textHighscore.text = "You've set a new highscore!";
            PowSceneManager.Get().singleplayerHighscore = plrScore.scoredPoints;
        }
    }
}
