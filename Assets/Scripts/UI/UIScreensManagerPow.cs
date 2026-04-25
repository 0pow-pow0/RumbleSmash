using UnityEngine;

public class UIScreensManagerPow : MonoBehaviour
{
    [Header("References"), SerializeField]
    public UI_InputHandlingScreen inputHandlingScreen;
    public UIRoundAnimations roundAnimationsScreen;

    
    // -------------------------------------------
    // ! Singleton shit
    // -------------------------------------------
    private static UIScreensManagerPow inst;

    public static UIScreensManagerPow Get()
    {
        if(inst == null)
        {
            Debug.LogError("GameManager non instanziato!");    
            return null;
        }

        return inst;
    }

    void InitSingleton()
    {
        if(inst != null)
        {
            Debug.LogError("GameManager gia' instanziato");
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
