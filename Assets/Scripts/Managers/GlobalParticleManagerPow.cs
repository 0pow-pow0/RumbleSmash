using UnityEngine;


/// <summary>
/// UtileSolo come container di tutti i particle manager
/// </summary>
public class GlobalParticleManagerPow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        InitSingleton();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    // -------------------------------------------
    // ! Singleton Shit
    // -------------------------------------------
    private static GlobalParticleManagerPow inst;

    public static GlobalParticleManagerPow Get()
    {
        if(inst == null)
        {
            Debug.LogError("GlobalParticleManagerPow non instanziato!");    
            return null;
        }

        return inst;
    }

    void InitSingleton()
    {
        if(inst != null)
        {
            Debug.LogError("GlobalParticleManagerPow gia' instanziato");
        }
        else
        {
            inst = this;
        }
    }

}
