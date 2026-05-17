using System.Linq.Expressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(
fileName = "Dialogue", 
menuName = "ScriptableObject/Eventables/Missions/SaltaMission")]
public class SaltaMission : Eventable
{
    private Player plr;
    private UIQuests qst;

    Vector3 startPosition;


    [SerializeField]
    float minJumpsToEnd;
    float madeJumps = 0;

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
            inputText = "Usa A/X per saltare";
            qst.StartQuest(inputText + " " + "0/" + minJumpsToEnd);
        }
        else if(plr.plrInp.currentControlScheme == "Keyboard&Mouse")
        {
            inputText = "Usa spazio per saltare";
            qst.StartQuest(inputText + " " + "0/" + minJumpsToEnd);
        }
        else
        {
            inputText = "Salta!";
            qst.StartQuest(inputText + " " + "0/" + minJumpsToEnd);
        }

        plr.pj.onFirstJumpPerformed.AddListener(OnFirstJumpPerformedListener);

        startPosition = plr.transform.position;
        madeJumps = 0;
    }

    void OnFirstJumpPerformedListener()
    {
        madeJumps++;
        qst.UpdateQuestText(inputText + " " + madeJumps + "/" + minJumpsToEnd);
    }

    public override void OnEnd()
    {
        plr.transform.position = startPosition;
        plr.pj.onFirstJumpPerformed.RemoveListener(OnFirstJumpPerformedListener);
        qst.EndQuest();
    }

    public override void Update()
    {
        if(madeJumps >= minJumpsToEnd)
        {
            state = EventableState.QUIT;
        }
    } 
}