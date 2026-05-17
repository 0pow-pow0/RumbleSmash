using System.Linq.Expressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
[CreateAssetMenu(
fileName = "Dialogue", 
menuName = "ScriptableObject/Eventables/Missions/MuovitiMission")]
public class MuovitiMission : Eventable
{
    private Player plr;
    private UIQuests qst;

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

        qst = GameObject.FindAnyObjectByType<UIQuests>();
        if(qst == null)
        {
            Debug.LogError("UIQuests nullo");
            state = EventableState.QUIT;
            return;
        }

        if(plr.plrInp.currentControlScheme == "Gamepad")
        {
            inputText = "Usa le freccette per muoverti";
            qst.StartQuest(inputText + " 0%");
        }
        else if(plr.plrInp.currentControlScheme == "Keyboard&Mouse")
        {
            inputText = "Usa WASD per muoverti";
            qst.StartQuest(inputText + " 0%");
        }
        else
        {
            inputText = "Muoviti un po'";
            qst.StartQuest(inputText + " 0%");
        }


        startPosition = plr.transform.position;
        passedTimeInMovement = 0;
    }

    public override void OnEnd()
    {
        plr.transform.position = startPosition;
        qst.EndQuest();
    }

    public override void Update()
    {
        if(Mathf.Abs(plr.rb.linearVelocityX) > 0f)
        {
            passedTimeInMovement += Time.deltaTime;
            float passedTimePercentual = passedTimeInMovement / minMovementTimeToEnd;
            qst.UpdateQuestText(inputText + " %" + (int)(passedTimePercentual * 100));
        }
        if(passedTimeInMovement >= minMovementTimeToEnd)
        {
            state = EventableState.QUIT;
        }

        
    } 
}