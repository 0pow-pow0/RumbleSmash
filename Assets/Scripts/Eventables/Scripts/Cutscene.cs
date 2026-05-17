using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UtilityShit;


[CreateAssetMenu(
fileName = "Dialogue", 
menuName = "ScriptableObject/Eventables/Cutscene")]
public class Cutscene : Eventable
{
    // ? --- Eh si', andra' sempre instanziato
    PlayableDirector director;

    [SerializeField]
    TimelineAsset cutscene;

    bool hasCutsceneEnded = false;

    public override void OnStart()
    {
        director = FindAnyObjectByType<PlayableDirector>();

        director.playableAsset = cutscene;

        hasCutsceneEnded = false;
        Debug.Log("Cutscene Dur: " + cutscene.duration);
        
        
        // ? --- Per qualche motivo non funziona
        //director.stopped += OnCutsceneEnd;

        director.Play();
        PowUtilityU.Get().DelayAction(OnCutsceneEnd, (float)cutscene.duration);
        Debug.Log("Cutscene started");
    }

    void OnCutsceneEnd()
    {
        hasCutsceneEnded = true;
        Debug.Log("Cutscene Ended!");
    }

    public override void OnEnd()
    {
        PowUtility.Log("Cutscene ended!", Color.magenta);
        //director.stopped -= OnCutsceneEnd;
    }

    public override void Update()
    {
        if(hasCutsceneEnded)
        {
            state = EventableState.QUIT;
        }
    } 

    
}
