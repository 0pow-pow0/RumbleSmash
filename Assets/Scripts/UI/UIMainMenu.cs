using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public void SingleplayerButton()
    {
        SceneManager.LoadScene("Singleplayer");
    }

    public void MultiplayerButton()
    {
        SceneManager.LoadScene("Multiplayer");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
