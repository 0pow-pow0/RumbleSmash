using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public void SingleplayerButton()
    {
        PowSceneManager.Get().ChangeScene("Singleplayer");
    }

    public void MultiplayerButton()
    {
        PowSceneManager.Get().ChangeScene("Multiplayer");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
