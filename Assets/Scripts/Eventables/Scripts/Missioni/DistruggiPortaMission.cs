using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(
fileName = "Dialogue", 
menuName = "ScriptableObject/Eventables/Missions/DistruggiPortaMission")]
public class DistruggiPortaMission : Eventable
{
    private Player plr;
    private Goal gl;
    private Ball bl;
    private UIQuests qst;

    Vector3 startPosition;

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

        gl = GameManager.Get().player1Goal;
        if(gl == null)
        {
            Debug.LogError("Goal nullo");
            state = EventableState.QUIT;
            return;
        }

        bl = GameManager.Get().ball;
        if(bl == null)
        {
            Debug.LogError("Ball nullo");
            state = EventableState.QUIT;
            return;
        }
        UIInGame uiiG = FindAnyObjectByType<UIInGame>();
        if(uiiG == null)
        {
            Debug.LogError("UIInGame nullo");
            state = EventableState.QUIT;
            return;
        }
        // ? --- Eccoci qui, le conseguenze di aver fatto l'iscrizione degli eventi
        // ? --- al contrario DAJE
        uiiG.father.SetActive(true);
        uiiG.player1Father.SetActive(true);
        
        // ? --- Dio perdonami per i miei peccati
        UIInGameGoals uiiggs = FindAnyObjectByType<UIInGameGoals>();
        
        // ? --- Sono stanco capo, sono un figlio di troia capo 
        uiiggs
        .player1GoalHPSlider.gameObject.SetActive(true);



        inputText = "Distruggi lo scudo della porta e segna";
        qst.StartQuest(inputText);
        startPosition = plr.transform.position;

        gl.gameObject.SetActive(true);
        
        bl.onBallScore.AddListener(OnScore);
    }

    void OnScore()
    {   
        bl.gameObject.SetActive(false);
        
        qst.EndQuest();
        bl.onBallScore.RemoveListener(OnScore);
        PowUtilityU.Get().DelayAction(() => state = EventableState.QUIT, 2f); 
    }

    public override void OnEnd()
    {
    }

    public override void Update()
    {

    } 
}