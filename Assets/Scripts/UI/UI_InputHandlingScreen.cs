using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class UI_InputHandlingScreen : MonoBehaviour
{
    [Header("References"), SerializeField]
    public GameObject father;
    public TextMeshProUGUI textPlayer1;
    public TextMeshProUGUI textPlayer2;
    public TextMeshProUGUI textDescription;
    public Image panel;

    void Start()
    {
        MatchManager.Get().onPreMatchAssignDevices.AddListener
        (
            () =>
            {
                father.SetActive(true);
            }
        );
        MatchManager.Get().onMatchBegin.AddListener
        ( 
            () =>
            {
                father.SetActive(false);
            }  
        );

        InputManagerLogic.Get().onPlayer1Joined
        .AddListener
        (
            Player1JoinedBehaviour
        );
    
        InputManagerLogic.Get().onPlayer2Joined
        .AddListener
        (
            Player2JoinedBehaviour
        );

        InputManagerLogic.Get().onRestartDeviceAssignment
        .AddListener
        (
            SetInitialState
        );
    }

    public void SetInitialState()
    {
        father.SetActive(true);
        textPlayer1.gameObject.SetActive(true);
        textPlayer2.gameObject.SetActive(false);
        

        textPlayer1.color =
            PowUtilityU.Get().MaxFade(textPlayer1.color);
        textPlayer2.color = 
            PowUtilityU.Get().MaxFade(textPlayer2.color);
        textDescription.color = 
            PowUtilityU.Get().MaxFade(textDescription.color);
        panel.color = 
            PowUtilityU.Get().MaxFade(panel.color);
        
        Debug.Log("Pisello");
    }

    void Player1JoinedBehaviour()
    {
        textPlayer1.gameObject.SetActive(false);
        textPlayer2.gameObject.SetActive(true);
    }

    void Player2JoinedBehaviour()
    {
        textPlayer1.DOFade(0f, 3f);
        textPlayer2.DOFade(0f ,3f);
        textDescription.DOFade(0f, 3f);
        panel.DOFade(0f ,3f);
        // PowUtilityU.Get().DelayAction(
        //     () =>
        //     {
        //         father.SetActive(false);
        //     },
        //     3.2f
        // );

        //TODO: Activate animation
        //TODO: Trigger scene change
    }

    void Update()
    {

    }
}
