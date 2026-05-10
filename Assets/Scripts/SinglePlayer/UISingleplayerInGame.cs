using TMPro;
using UnityEngine;

public class UISingleplayerInGame : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textPoints;

    

    void Start()
    {
        GameManager g = GameManager.Get();

        TargetManager.Get().onTargetDestroy.AddListener(
            (obj) =>
            {
                textPoints.text = "Points: " + 
                    g.player1.GetComponent<PlayerSPVars>().scoredPoints;
            }
        );
    }

    void Update()
    {
        
    }
}
