using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Gestisce animazioni che riguardano la UI,
/// indipendenemente dalla sua struttura.
/// 
/// Pensato per utilizzare animazioni di uso generico,
/// come transizioni che portano al bianco per animare
/// transizioni tra una SCENA ed un altra.
/// 
/// Ogni funzione possiede un elemento della UI da 
/// utilizzare per la sua animazione, dunque chiamarla
/// piu' di una volta non fara' altro che riavviarla.
/// </summary>
public class UIAnimationManager : MonoBehaviour
{
    [Header("References"), SerializeField]
    GameObject father;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    Animator anim;

    /// <summary>
    /// Non sfrutta Animator
    /// 
    /// Mostra un colore a pieno schermo e poi
    /// lo rimuove, utilizzarlo piu
    /// </summary>
    [Header("Panel References"), SerializeField]
    Image codedFadeAnimationPanel;
    public void StartScreenWideFadeToAndDecay(
        Color targetColor,
        float fadeDuration,
        float fullColorDur,
        Action followUpAction = null)
    {
        codedFadeAnimationPanel.gameObject.SetActive(true);

        if(fadeDuration == 0 ||
            fullColorDur == 0)
        {
            Debug.LogError("Can't start a transition " + 
            "that lasts 0 seconds");
            return;
        }

        float halfFadeTime = fadeDuration / 2;
        IEnumerator FadeRoutine()
        {
            codedFadeAnimationPanel.color =
                new Color(
                    targetColor.r,
                    targetColor.g,
                    targetColor.b,
                    0
                );

            // ? --- Going to color
            float alpha = 0f;
            float timePassed = 0f;
            while(alpha <= 1f)
            {   
                alpha = timePassed / halfFadeTime;
                Debug.Log(alpha);
                codedFadeAnimationPanel.color = new Color(
                    codedFadeAnimationPanel.color.r,
                    codedFadeAnimationPanel.color.g,
                    codedFadeAnimationPanel.color.b,
                    alpha
                );

                timePassed += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(fullColorDur);

            alpha = 0f;
            timePassed = 0f;
            // ? --- Going to blank (transparent)
            while(alpha >= 0f)
            {   
                alpha = 
                    1 - (timePassed / halfFadeTime);

                codedFadeAnimationPanel.color = new Color(
                    codedFadeAnimationPanel.color.r,
                    codedFadeAnimationPanel.color.g,
                    codedFadeAnimationPanel.color.b,
                    alpha
                );

                timePassed += Time.deltaTime;
                yield return null;
            }
            Debug.Log("Ended"); 
            codedFadeAnimationPanel.gameObject.SetActive(false);
            
            if(followUpAction != null)
                followUpAction.Invoke();
        }   

        StartCoroutine(FadeRoutine());
    }

    [Space(10), SerializeField]
    Image curtainOpenAnimationPanel1;
    [SerializeField]
    Image curtainOpenAnimationPanel2;
    public void StartScreenWideCurtainOpen(
        Color color,
        float duration,
        Action followUp = null
    )
    {
        curtainOpenAnimationPanel1.color = new Color(
            color.r,
            color.g,
            color.b,
            color.a
        );

        curtainOpenAnimationPanel2.color = new Color(
            color.r,
            color.g,
            color.b,
            color.a
        );

        anim.SetFloat("CurtainOpenDuration", 1/duration);
        anim.SetTrigger("CurtainOpenTrigger");
        
        if(followUp != null)
            PowUtilityU.Get().DelayAction(followUp, duration);
    }
    
    [Space(10), SerializeField]
    Image vignetteAnimationPanel;
    public void StartScreenWideVignetteAndDecay(
        //Color vignetteColor,
        //float vignetteDuration
        )
    {

    }

    void Update()
    {
        
    }

    // -------------------------------------------
    // ! Singleton Shit
    // -------------------------------------------
    private static UIAnimationManager inst;

    public static UIAnimationManager Get()
    {
        if(inst == null)
        {
            Debug.LogError("UIAnimationManager non instanziato!");    
            return null;
        }

        return inst;
    }

    void InitSingleton()
    {
        if(inst != null)
        {
            Debug.LogError("UIAnimationManager gia' instanziato");
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
