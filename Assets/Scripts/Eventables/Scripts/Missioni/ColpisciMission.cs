using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(
fileName = "Dialogue", 
menuName = "ScriptableObject/Eventables/Missions/ColpisciMission")]
public class ColpisciMission : Eventable
{
    private Player plr;
    private Ball bl;
    private UIQuests qst;

    Vector3 startPosition;


    [SerializeField]
    float minColpiPallaToEnd;
    float madeColpiPalla = 0;

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

        bl = GameManager.Get().ball;
        if(qst == null)
        {
            Debug.LogError("Ball nullo");
            state = EventableState.QUIT;
            return;
        }

        if(plr.plrInp.currentControlScheme == "Gamepad")
        {
            inputText = "Premi J per colpire la palla";
            qst.StartQuest(inputText + " " + "0/" + minColpiPallaToEnd);
        }
        else if(plr.plrInp.currentControlScheme == "Keyboard&Mouse")
        {
            inputText = "Premi X/O per colpire la palla";
            qst.StartQuest(inputText + " " + "0/" + minColpiPallaToEnd);
        }
        else
        {
            inputText = "Premi il tasto per calciare la palla!";
            qst.StartQuest(inputText + " " + "0/" + minColpiPallaToEnd);
        }

        plr.pbi.onBallHit.AddListener(OnColpoPallaListener);

        startPosition = plr.transform.position;
        madeColpiPalla = 0;

        bl.gameObject.SetActive(true);
        bl.rb.AddForce(Vector2.left * 3, ForceMode2D.Impulse);
    }

    void OnColpoPallaListener(Vector2 hitPos)
    {
        Debug.Log("Kick " + madeColpiPalla);
        madeColpiPalla++;
        qst.UpdateQuestText(inputText + " " + madeColpiPalla + "/" + minColpiPallaToEnd);
    }

    public override void OnEnd()
    {
        plr.transform.position = startPosition;
        plr.pbi.onBallHit.RemoveListener(OnColpoPallaListener);
        qst.EndQuest();
    }

    public override void Update()
    {
        if(madeColpiPalla >= minColpiPallaToEnd)
        {
            state = EventableState.QUIT;
        }
    } 
}