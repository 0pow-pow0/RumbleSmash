using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(
fileName = "Dialogue", 
menuName = "ScriptableObject/Eventables/Missions/CaricaColpoMission")]
public class CaricaColpoMission : Eventable
{
    private Player plr;
    private Ball bl;
    private UIQuests qst;

    Vector3 startPosition;

    bool hasChargedAttack = false;
    [SerializeField]
    float minColpiCaricatiPallaToEnd;
    float madeColpiCaricatiPalla = 0;

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
            inputText = "Tieni premuto J per caricare il colpo";
            qst.StartQuest(inputText + " " + "0/" + minColpiCaricatiPallaToEnd);
        }
        else if(plr.plrInp.currentControlScheme == "Keyboard&Mouse")
        {
            inputText = "Tieni premuto X/O per caricare il colpo";
            qst.StartQuest(inputText + " " + "0/" + minColpiCaricatiPallaToEnd);
        }
        else
        {
            inputText = "Adesso tieni premuto per caricare il colpo!";
            qst.StartQuest(inputText + " " + "0/" + minColpiCaricatiPallaToEnd);
        }

        plr.pbi.onKickEnd.AddListener(OnKickEndListener);
        plr.pbi.onChargeEnd.AddListener(() => hasChargedAttack = true);

        hasChargedAttack = false;

        startPosition = plr.transform.position;
        madeColpiCaricatiPalla = 0;
    }

    void OnKickEndListener()
    {
        if(hasChargedAttack)
        {
            madeColpiCaricatiPalla++;
            qst.UpdateQuestText(inputText + " " + madeColpiCaricatiPalla + "/" + minColpiCaricatiPallaToEnd);
        }
        hasChargedAttack = false;
    }

    public override void OnEnd()
    {
        plr.transform.position = startPosition;
        plr.pbi.onChargeEnd.RemoveListener(OnKickEndListener);
        qst.EndQuest();
    }

    public override void Update()
    {
        if(madeColpiCaricatiPalla >= minColpiCaricatiPallaToEnd)
        {
            state = EventableState.QUIT;
        }
    } 
}