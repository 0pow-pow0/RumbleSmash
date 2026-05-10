using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIPausePanel : MonoBehaviour
{
    [SerializeField]
    GameObject father;


    void Start()
    {
        
    }

    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if(father.activeInHierarchy)
            {
                DeactivatePanel();
            }
            else
            {
                ActivatePanel();
            }
        }
    }

    public void ActivatePanel()
    {
        father.SetActive(true);
        Time.timeScale = 0f;

        InputSystem.actions.FindActionMap("Player").Disable();
    }

    public void DeactivatePanel()
    {
        father.SetActive(false);
        Time.timeScale = 1f;

        InputSystem.actions.FindActionMap("Player").Enable();
    }

    public void ResumeButton()
    {
        DeactivatePanel();
    }

    public void ExitButton()
    {
        //TODO
        SceneManager.LoadScene("MainMenu");
    }
}
