using UnityEngine;

[CreateAssetMenu(
fileName = "Dialogue", 
menuName = "ScriptableObject/Eventables/Missions/DoppioScattoMission")]
public class DoppioScattoMission : Eventable
{
    private Player plr;
    private UIQuests qst;

    Vector3 startPosition;


    [SerializeField]
    float minDoppioScattoToEnd;
    float madeDoppioScatto = 0;

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
            inputText = "Premi A/X dopo un salto per fare un doppioScatto";
            qst.StartQuest(inputText + " " + "0/" + minDoppioScattoToEnd);
        }
        else if(plr.plrInp.currentControlScheme == "Keyboard&Mouse")
        {
            inputText = "Premi spazio dopo un salto per fare un doppioScatto";
            qst.StartQuest(inputText + " " + "0/" + minDoppioScattoToEnd);
        }
        else
        {
            inputText = "Premi due volte il tasto per saltare per fare un doppioScatto!";
            qst.StartQuest(inputText + " " + "0/" + minDoppioScattoToEnd);
        }

        plr.pj.onDoppioScattoEnd.AddListener(OnDoppioScattoEndListener);

        startPosition = plr.transform.position;
        madeDoppioScatto = 0;
    }

    void OnDoppioScattoEndListener()
    {
        madeDoppioScatto++;
        qst.UpdateQuestText(inputText + " " + madeDoppioScatto + "/" + minDoppioScattoToEnd);
    }

    public override void OnEnd()
    {
        plr.transform.position = startPosition;
        plr.pj.onDoppioScattoEnd.RemoveListener(OnDoppioScattoEndListener);
        qst.EndQuest();
    }

    public override void Update()
    {
        if(madeDoppioScatto >= minDoppioScattoToEnd)
        {
            state = EventableState.QUIT;
        }
    } 
}