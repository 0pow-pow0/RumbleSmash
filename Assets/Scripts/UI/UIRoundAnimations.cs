using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UtilityShit;

public class UIRoundAnimations : MonoBehaviour
{
    [Header("References"), SerializeField]
    private GameObject father;
    
    [SerializeField]
    TextMeshProUGUI textRoundStart;
    
    [SerializeField]
    TextMeshProUGUI textRoundCountdown;

    /// <summary>
    /// Verra' mostrato a schermo un timer arrotondato alla cifra decimale  
    /// </summary>
    /// <param name="duration"></param>
    public void StartCountdownAnimation(float duration)
    {
        Debug.Log("Started!");
        textRoundCountdown.text = "" + (int)duration;
        textRoundCountdown.gameObject.SetActive(true);


        PowUtilityU.Get().DoActionUntil(
            (float passedTime) =>
            {
                // ? --- Durata Massima - tempo passato = countdown
                // ? --- +0.99 per non mostrare lo 0 e non arrotondare al
                // ? --- numero successivo
                int roundedTime = (int)(duration - passedTime + 0.99f);

                textRoundCountdown.text = "" + roundedTime;
            },
            (float passedTime) =>
            {
                if(passedTime <= duration)
                {
                    return true;
                }

                textRoundCountdown.text = "START!";
                textRoundCountdown.DOFade(0, 1f).
                OnComplete(() =>
                {
                    textRoundCountdown.gameObject.SetActive(false);
                    // ? --- Resetta fade al massimo
                    textRoundCountdown.color = 
                        PowUtilityU.Get().MaxFade(textRoundCountdown.color);
                    
                    textRoundCountdown.text = "";

                });

                return false;
            }
        );
    }


    void Update()
    {
        if(Keyboard.current.zKey.wasPressedThisFrame)
        { 
            Debug.Log("pene");
            StartCountdownAnimation(3f);
        }
    }
}
