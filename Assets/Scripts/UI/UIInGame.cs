using System.Collections;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : MonoBehaviour
{
    [Header("References player 1"), SerializeField]
    private GameObject player1Father;

    [SerializeField]
    private GameObject player1IconFather;
    [SerializeField]
    private Image player1Image;
    [SerializeField]
    private Slider player1DoppioScattoSlider;
    [SerializeField]
    private Image player1DoppioScattoImage;

    
    [Header("References player 2"), SerializeField]
    private GameObject player2Father;
    [SerializeField]    
    private GameObject player2IconFather;
    [SerializeField]
    private Image player2Image;
    [SerializeField]
    private Slider player2DoppioScattoSlider;    
    [SerializeField]
    private Image player2DoppioScattoImage;


    void Start()
    {
        player1Father.SetActive(false);
        player2Father.SetActive(false);

        MatchManager.Get().onMatchBegin.AddListener(
            () =>
            {
                player1Father.SetActive(true);
                player2Father.SetActive(true);
            }
        );

        // MatchManager.Get().onPreMatchAssignDevices.AddListener
        // (
           
        // );

        GameManager g = GameManager.Get();

        // -------------------------------------------
        // ! Players
        // -------------------------------------------

        if(g.player1 == null)
        {
            player1Father.SetActive(false);
            return;
        }
        
        g.player1.pj.onDoppioScattoStart.AddListener(
            () =>
            {     
                UpdateDoppioScattoSlider(
                    player1DoppioScattoSlider,
                    player1DoppioScattoImage,
                    g.player1.pj.DOPPIOSCATTO_COOLDOWN_DURATION);
            }
        );
        
        if(g.player2 == null)
        {
            player2Father.SetActive(false);
            return;
        }

        g.player2.pj.onDoppioScattoStart.AddListener(
            () =>
            {     
                UpdateDoppioScattoSlider(
                    player2DoppioScattoSlider,
                    player2DoppioScattoImage,
                    g.player2.pj.DOPPIOSCATTO_COOLDOWN_DURATION);
            }
        );
    }

    void UpdateDoppioScattoSlider(
        Slider slider,
        Image doppioScattoImage,
        float cooldownDur)
    {        
        if(cooldownDur == 0f)
        {
            slider.value = 1f; 
            return;
        }

        StartCoroutine(
            UpdateDoppioScattoSliderRoutine
            (slider, doppioScattoImage, cooldownDur));
    }

    public IEnumerator UpdateDoppioScattoSliderRoutine(
        Slider slider,
        Image doppioScattoImage,
        float cooldownDur)
    {

        float elapsedTime = 0f;

        slider.value = 0f;

        // ! --- Importante, in questo modo il player capisce
        // ! --- che puo' farlo qualche millSec prima che il ring 
        // ? --- si completi, il che viene piu' intuitivo visivamente.
        cooldownDur += cooldownDur/12;

        while(elapsedTime <= cooldownDur && cooldownDur != 0)
        {
            elapsedTime += Time.deltaTime;

            float normalizedCooldown = 
                elapsedTime / cooldownDur;

            float normalizedClampedCooldown = 
                Mathf.Clamp(normalizedCooldown, 0f, 1f);
            
            slider.value = normalizedClampedCooldown;


            yield return null;
        }
        
        Vector3 originalScale =
            doppioScattoImage.transform.localScale;
        
        Color originalColor =
            doppioScattoImage.color;

        doppioScattoImage.transform
        .DOScale
        (
            new Vector3
            (
                doppioScattoImage.transform.localScale.x * 1.5f,
                doppioScattoImage.transform.localScale.y * 1.5f,
                doppioScattoImage.transform.localScale.z
            ),
            0.15f
        )
        .OnComplete
        (
            () =>
            {
                doppioScattoImage.transform
                .DOScale
                (
                    originalScale, 
                    0.15f
                );
            }
        );
 
        doppioScattoImage
        .DOColor(Color.white, 0.25f)
        .OnComplete
        (
            () =>
            {
                doppioScattoImage.DOColor(originalColor, 0.1f);
            }
        );

        //TODO: flavour
    }
}
