using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(
fileName = "Dialogue", 
menuName = "ScriptableObject/Eventables/Missions/EndMission")]
public class EndMission : Eventable
{
    public override void OnStart()
    {


        UIAnimationManager.Get().StartScreenWideFadeToAndDecay(Color.black, 5f, 90f);
        PowUtilityU.Get().DelayAction(() => state = EventableState.QUIT, 6f);
    }

    public override void OnEnd()
    {
        PowSceneManager.Get().EndTutorial();
    }

    public override void Update()
    {

        
    } 
}