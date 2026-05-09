using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameGoals : MonoBehaviour
{
    [Header("References Goal 1"), SerializeField]
    private Slider player1GoalHPSlider;

    [Header("References Goal 2"), SerializeField]
    private Slider player2GoalHPSlider;

    void Awake()
    {
        player1GoalHPSlider.gameObject.SetActive(false);
        player2GoalHPSlider.gameObject.SetActive(false);
    }
    void Start()
    {
        GameManager g = GameManager.Get();

        if(g.player1Goal == null)
            return;

        MatchManager.Get().onPreMatchShowRivals.AddListener
        (
            (dur) =>
            {
                player1GoalHPSlider.gameObject.SetActive(false);
                player2GoalHPSlider.gameObject.SetActive(false);
            }
        );

        MatchManager.Get().onMatchBegin.AddListener
        (
            () =>
            {
                player1GoalHPSlider.gameObject.SetActive(true);
                player2GoalHPSlider.gameObject.SetActive(true);
            }
        );

        RoundManager.Get().onRoundStartCountdown.AddListener
        (
            (countdownDur) =>
            {
                DOTween.To
                (
                    () => player1GoalHPSlider.value, 
                    val => player1GoalHPSlider.value = val, 
                    1,
                    0.4f
                );
                DOTween.To
                (
                    () => player2GoalHPSlider.value, 
                    val => player2GoalHPSlider.value = val, 
                    1,
                    0.4f
                );
            }
        );

        g.player1Goal.onShieldDamage.AddListener
        (
            () =>
            {
                float normalizedHealth = 
                    (float)g.player1Goal.shieldHP /
                    (float)g.player1Goal.stats.START_SHIELD_HP;
                if(normalizedHealth < 0)
                    normalizedHealth = 0;

                Debug.Log(normalizedHealth + 
                " " + g.player2Goal.stats.START_SHIELD_HP 
                + " " + g.player2Goal.shieldHP);
                
                DOTween.To
                (
                    () => player1GoalHPSlider.value, 
                    val => player1GoalHPSlider.value = val, 
                    normalizedHealth,
                    0.4f
                );
            }
        );


        if(g.player2Goal == null)
            return;

        g.player2Goal.onShieldDamage.AddListener
        (
            () =>
            {
                float normalizedHealth = 
                (float)g.player2Goal.shieldHP /
                (float)g.player2Goal.stats.START_SHIELD_HP;
                if(normalizedHealth < 0)
                    normalizedHealth = 0;

                DOTween.To
                (
                    () => player2GoalHPSlider.value, 
                    val => player2GoalHPSlider.value = val, 
                    normalizedHealth,
                    0.4f
                );

                
            }
        );
    }

    void Update()
    {
        
    }
}
