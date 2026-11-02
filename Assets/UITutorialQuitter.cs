using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITutorialQuitter : MonoBehaviour
{
    [SerializeField]
    GameObject father;

    [SerializeField]
    TextMeshProUGUI showButtonToPressToSkip;

    void Start()
    {
        SingleplayerInputManager.Get().onPlayer1Joined.AddListener
        (
            () =>
            {
                Player plr = GameManager.Get().player1;
                if(plr.plrInp.currentControlScheme == "Gamepad")
                {
                    showButtonToPressToSkip.text = 
                    "Tieni premuto START per saltare il tutorial";
                }
                else if(plr.plrInp.currentControlScheme == "Keyboard&Mouse")
                {
                    showButtonToPressToSkip.text = 
                    "Tieni premuto BACKSPACE per saltare il tutorial";
                }
                else
                {
                    showButtonToPressToSkip.text = 
                    "Tieni premuto BACKSPACE per saltare il tutorial";
                }
                quitSlider.gameObject.SetActive(false);
                showButtonToPressToSkip.gameObject.SetActive(true);
                showButtonToPressToSkip.DOFade(0f, 4f)
                .OnComplete
                (() => showButtonToPressToSkip.gameObject.SetActive(false));
            }
        );
    }

    [SerializeField]
    Slider quitSlider;

    [SerializeField]
    float minTimeToQuit = 5f;

    float quitProgressTimer = 0f;
    void Update()
    {
        if(InputSystem.actions.FindAction("Skip").IsPressed())
        {
            if(!quitSlider.isActiveAndEnabled)
                quitSlider.gameObject.SetActive(true);


            quitProgressTimer += Time.deltaTime;
            quitSlider.value = quitProgressTimer / minTimeToQuit;

        }
        if(InputSystem.actions.FindAction("Skip").WasReleasedThisFrame())
        {
            quitProgressTimer = 0f;
            quitSlider.value = 0f;
            quitSlider.gameObject.SetActive(false);
        }

        if(quitProgressTimer >= minTimeToQuit)
        {
            PowSceneManager.Get().EndTutorial();
        }
    }
}
