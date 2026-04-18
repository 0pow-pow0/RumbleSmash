using System;
using System.Collections;
using UnityEngine;

public class PowUtilityU : MonoBehaviour
{

    // -------------------------------------------
    // ! Coroutines Related 
    // -------------------------------------------
    public void DelayAction(
        Action action,
        float timeToWait)
    {
        StartCoroutine(
            DelayActionRoutine(action, timeToWait));
    }
    private IEnumerator DelayActionRoutine(
        Action action,
        float timeToWait
        )
    {
        yield return new WaitForSeconds(timeToWait);
        action.Invoke();
    }



    // -------------------------------------------
    // ! Color related
    // -------------------------------------------
    /// <summary>
    /// Ritorna l'equivalente del colore originale ma
    /// con l'alpha settato a 1
    /// </summary>
    public Color MaxFade(Color original)
    {
        return new Color(
            original.r,
            original.g,
            original.b,
            1f
        );
    }

    /// <summary>
    /// Ritorna l'equivalente del colore originale ma
    /// con l'alpha settato a 0
    /// </summary>
    public Color MinFade(Color original)
    {
        return new Color(
            original.r,
            original.g,
            original.b,
            0
        );
    }

    void Awake()
    {
        InitSingleton();
    }


    // -------------------------------------------
    // ! Singleton shit
    // -------------------------------------------
    private static PowUtilityU inst;

    public static PowUtilityU Get()
    {
        if(inst == null)
        {
            Debug.LogError("PowUtilityU non instanziato!");    
            return null;
        }

        return inst;
    }

    void InitSingleton()
    {
        if(inst != null)
        {
            Debug.LogError("PowUtilityU gia' instanziato");
        }
        else
        {
            inst = this;
        }
    }
}
