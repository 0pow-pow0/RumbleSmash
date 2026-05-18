using UnityEngine;

public class TutorialAudioManager : MonoBehaviour
{
    public void PlayLoopingMusic()
    {
        GetComponent<AudioSource>().Play();
    }
}
