using UnityEngine;

public class UIInputManagerSingleplayer : MonoBehaviour
{
    [SerializeField]
    GameObject father;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        father.SetActive(true);
        SingleplayerInputManager.Get().onPlayer1Joined.AddListener
        (
            () =>
            {
                Debug.Log("Chimaato");
                father.SetActive(false);
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
