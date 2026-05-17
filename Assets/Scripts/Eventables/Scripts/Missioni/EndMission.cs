using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(
fileName = "Dialogue", 
menuName = "ScriptableObject/Eventables/Missions/EndMission")]
public class EndMission : Eventable
{
    private Player plr;

    Vector3 startPosition;


    [SerializeField]
    float minMovementTimeToEnd;
    float passedTimeInMovement = 0;

    string inputText;
    public override void OnStart()
    {
        // ? --- E' sempre uno
        plr = GameObject.FindAnyObjectByType<Player>();
        if(plr == null)
        {
            Debug.LogError("Player nullo!");
            state = EventableState.QUIT;
            return;
        }

        UIAnimationManager.Get().StartScreenWideFadeToAndDecay(Color.black, 5f, 90f);
        PowUtilityU.Get().DelayAction(() => state = EventableState.QUIT, 6f);
    }

    public override void OnEnd()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public override void Update()
    {

        
    } 
}