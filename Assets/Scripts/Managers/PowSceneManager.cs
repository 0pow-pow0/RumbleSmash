using EditorAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PowSceneManager : MonoBehaviour
{
    [SerializeField, EditorAttributes.ReadOnly]
    bool hasShownTutorial = false;

    
    [SerializeField, EditorAttributes.ReadOnly]
    public int singleplayerHighscore = 400;

    void Awake()
    {
        PowSceneManager[] pw = FindObjectsByType<PowSceneManager>(FindObjectsSortMode.None);
        if(pw != null)
        {
            foreach(PowSceneManager p in pw)
            {
                if(p != this)
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }
        Debug.Log("PW: " + pw);
        if(InitSingleton())
        {
            DontDestroyOnLoad(this);
        } 
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string holdedScene;
    public void ChangeScene(string nextScene)
    {
        if(!hasShownTutorial)
        {
            SceneManager.LoadScene("Tutorial");
            hasShownTutorial = true;
            holdedScene = nextScene;
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    // ? --- Chiamato dall'ultima missione del tutorial
    public void EndTutorial()
    {
        SceneManager.LoadScene(holdedScene);
    }

    // -------------------------------------------
    // ! Singleton shit
    // -------------------------------------------
    private static PowSceneManager inst;

    public static PowSceneManager Get()
    {
        if(inst == null)
        {
            Debug.LogError("TutorialWatcher non instanziato!");    
            return null;
        }

        return inst;
    }

    bool InitSingleton()
    {
        if(inst != null)
        {
            Debug.LogError("TutorialWatcher gia' instanziato");
            Destroy(gameObject);
            return false;
        }
        else
        {
            inst = this;
            return true;
        }
    }



}
