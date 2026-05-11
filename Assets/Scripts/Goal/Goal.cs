using UnityEngine;
using UnityEngine.Events;
using UtilityShit;

public class Goal : MonoBehaviour
{
    [Header("References"), SerializeField]
    SpriteRenderer sprite;

    [field: SerializeField]
    public GoalCollider goalColl { get; private set; }
    [field: SerializeField]
    public GoalShieldCollider shieldColl { get; private set; }
    [field: SerializeField]
    public GameObject[] movementPivots { get; private set; }
    int activePivot = 0;

    #region Gameplay Vars
    [field: Header("Gameplay Vars"), SerializeField]
    Color shieldFullColor;
    [SerializeField]
    Color shieldBrokenColor;

    [field: SerializeField]
    public GoalGameplayStats stats { get; private set; }
    public int shieldHP { get; private set; }
    public void RemoveShieldHP(int toRemove)
    {
        if(isInvulnerable)
        {
            Debug.Log("Is invulnerable");   
            return;
        }
        Ball bl = GameManager.Get().ball;

        if(!bl.fsm.stage1.isActive)
        {    
            float sign = Mathf.Sign(bl.rb.linearVelocityX);
            // bl.rb.linearVelocity -=
            //     new UnityEngine.Vector2
            //     (
            //         // ? --- Opposta al moto
            //         sign * 
            //         bl.ON_BALL_HIT_DECREASE_SPEED,
            //         0f
            //     );      
                Vector2 orientedDecrease = 
                    bl.rb.linearVelocity.normalized * 
                    bl.ON_BALL_HIT_DECREASE_SPEED;
                
                bl.rb.linearVelocity -= orientedDecrease;
        } 

        shieldHP -= toRemove;
        onShieldDamage.Invoke();
        if(shieldHP <= 0 )
        {
            shieldHP = 0;
            sprite.color = shieldBrokenColor;
            shieldColl.SetCollider(false);
            goalColl.SetCollider(true);
            PowUtility.Log("Destroyed", Color.red);
            onShieldDestroy.Invoke();
            return;
        }

        StartInvulnerability();
    }


    [SerializeField, EditorAttributes.ReadOnly]
    public bool isInvulnerable = false;
    public void StartInvulnerability()
    {
        isInvulnerable = true;

        PowUtilityU.Get().DelayAction
        (
            () =>
            {
                isInvulnerable = false;
            },
            stats.INVULNERABILITY_TIME
        );
    }

    public void SetRoundBeginState()
    {
        goalColl.SetCollider(false);
        shieldColl.SetCollider(true);

        sprite.color = shieldFullColor;

        shieldHP = stats.START_SHIELD_HP;
    }

    #endregion 

    #region Events

    public UnityEvent onShieldDamage { get; private set; } = new();
    public UnityEvent onShieldDestroy { get; private set; } = new();
    public UnityEvent onInvulnerabilityStart { get; private set; } = new();
    public UnityEvent onInvulnerabilityEnd { get; private set; } = new();


    #endregion

    void Start()
    {
        shieldHP = stats.START_SHIELD_HP;
    }



    /// <summary>
    /// A chi appartiene la porta?
    /// Se appartiene al player1 dara' il punto all'avversario
    /// </summary>
    [SerializeField]
    public PlayerNumber playerNumber = new();
    
    public void Score()
    {
        if(playerNumber == PlayerNumber.PLAYER_1)
        {
            MatchManager.Get().ScorePlayer2(1);
            GameManager.Get().ball.onBallScore.Invoke();
        }
        else if (playerNumber == PlayerNumber.PLAYER_2)
        {
            MatchManager.Get().ScorePlayer1(1);
            GameManager.Get().ball.onBallScore.Invoke();
        }
    }
    void Update()
    {
        Vector2 lerpRes =  Vector2.MoveTowards
        (
            transform.position, 
            movementPivots[activePivot].transform.position,
            1f * Time.deltaTime
        );

        transform.position = lerpRes;
        if(
            Vector2.Distance
            (
                transform.position,
                movementPivots[activePivot].transform.position
            )
                <= 0.2f)
        {
            activePivot++;
            if(activePivot >= movementPivots.Length)
                activePivot = 0;
        }
    }
}