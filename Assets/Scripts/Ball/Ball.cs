using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UtilityShit;

public class Ball : MonoBehaviour
{

    #region Properties

    [field: Header("References"), SerializeField] 
    public Rigidbody2D rb { get; private set; }

    //[field: SerializeField] 
    //public PhysicsMaterial2D rbMat { get; private set; }
    [field: SerializeField]
    public CircleCollider2D physicsColl { get; private set; }
    
    [field: SerializeField]
    public CircleCollider2D goalColl { get; private set; } 

    [field: SerializeField]
    public SpriteRenderer spriteRenderer { get; private set; }
    
    [field: SerializeField]
    public GameObject mesh { get; private set; }
    [field: SerializeField]
    public QuickOutline outlineScr { get; private set; }

    public BallFSM fsm;

    #region Gameplay Stats
    
    [NonSerialized]
    public int damage;

    /// <summary>
    /// Potrei valutare di ridurre in percentuale la velocita'
    /// </summary>
    [field: SerializeField]
    public float ON_BALL_HIT_DECREASE_SPEED { get; private set; }
    #endregion


    // -------------------------------------------
    // ! Bounciness Related
    // -------------------------------------------
    #region PhysicsRelated
    public bool isMovementStopped { get; private set; } = false;
    

    [field: Header("Bounciness Related"), SerializeField] 
    public PhysicsMaterial2D endlessBounciness;
    public PhysicsMaterial2D veryHardBounciness;
    public PhysicsMaterial2D hardBounciness;


    #endregion

    /// <summary>
    /// Resetta completamente la palla, la sua fisica, il suo behaviour,
    /// portandola ad uno stato simile a quello di "inizio play della scena"  
    /// </summary>
    public void Reset()
    {
        StopAllCoroutines();

        rb.linearVelocity = Vector2.zero;
        fsm.DirtySwitch(fsm.stage1);
        spriteRenderer.gameObject.SetActive(true);
        damage = STAGE1_DAMAGE;

        // ? --- Attiva ghostPlatform
        physicsColl.excludeLayers = 
            physicsColl.excludeLayers &
            ~LayerMask.GetMask("GhostPlatform");
            
        mesh.SetActive(true);
        outlineScr.OutlineColor = stage1Color;

        // ? --- Ricopia stateEnter poiche' chiamare lo stateEnter
        // ? --- richiamerebbe l'evento associato, non lo vogliamo
        spriteRenderer.color = Color.gray;   
        rb.gravityScale = STAGE1_GRAVITY_SCALE;
        rb.sharedMaterial = hardBounciness;

        goalColl.enabled = true;

        physicsColl.excludeLayers =
            physicsColl.excludeLayers &
            ~LayerMask.GetMask("Goal");


        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // -------------------------------------------
    // ! Stage1 
    // -------------------------------------------
    #region Stage1
    [field: Header("Stage 1 Stats"), SerializeField] 
    public Color stage1Color { get; private set; }
    
    [field: SerializeField]
    public Color impactFrameColor { get; private set; }


    [field: SerializeField]
    public int STAGE1_DAMAGE { get; private set; }

    [field: SerializeField]
    public float STAGE1_MIN_MAGNITUDE { get; private set; }
    [field: SerializeField]
    public float STAGE1_GRAVITY_SCALE { get; private set; }
    #endregion



    // -------------------------------------------
    // ! Stage2 
    // -------------------------------------------
    #region Stage2
    // ? --- Per andare allo stage 2
    [field: Header("Stage 2 Stats"), Space(10), SerializeField]
    public Color stage2Color { get; private set; }
    [field: SerializeField]
    public int STAGE2_DAMAGE { get; private set; }
    [field: SerializeField]
    public float STAGE2_MIN_MAGNITUDE { get; private set; }
    
    [field: SerializeField]
    public float STAGE2_GRAVITY_SCALE { get; private set; }
    
    // ? --- Tempo minimo che deve trascorrere dentro lo stage2
    // ? --- prima di poter cambiare stato al precedente.    
    // ? --- NON VALE PER LO STAGE 3 
    [field: SerializeField]
    public float STAGE2_MIN_TIME { get; private set; }

    [field: SerializeField]
    public int STAGE2_FIRST_COLL_IF { get; private set; } 
    #endregion



    // -------------------------------------------
    // ! Stage3 
    // -------------------------------------------
    #region Stage3
    [field: Header("Stage 3 Stats"), Space(10), SerializeField]
    public Color stage3Color { get; private set; }
    [field: SerializeField]
    public int STAGE3_DAMAGE { get; private set; }

    [field: SerializeField]
    public float STAGE3_MIN_MAGNITUDE { get; private set; }
    [field: SerializeField]
    public float STAGE3_MAX_MAGNITUDE { get; private set; }
    
    [field: SerializeField]
    public float STAGE3_GRAVITY_SCALE { get; private set; }
    [field: SerializeField]
    public float STAGE3_MIN_TIME { get; private set; }
    
    [field: SerializeField]
    public int STAGE3_FIRST_COLL_IF { get; private set; }
    #endregion

    // ? --- Numero di magnitudine sotto il quale
    // ? --- i bounces vengono resettati
    [field: Space(20), SerializeField]
    public float MIN_MAGNITUDE_BEFORE_BOUNCES_RESET { get; private set; }

    // ? --- Numero di rimbalzi dall'ultimo reset,
    // ? --- il reset avviene quando la palla è prossima
    // ? --- al fermarsi
    [field: Space(20),Header("Debug"), 
    EditorAttributes.ReadOnly, SerializeField]
    public int wallBouncesSinceReset { get; private set; }
    [field: SerializeField, EditorAttributes.ReadOnly] 
    private float magnitude;

    [EditorAttributes.ReadOnly, SerializeField]
    private string ballStage;
    

    #endregion

    // -------------------------------------------
    // ! Events
    // -------------------------------------------
    #region UnityEvents
    [NonSerialized]
    public UnityEvent onBallStage1Start = new UnityEvent();

    [NonSerialized]
    public UnityEvent onBallStage1End = new UnityEvent();

    [NonSerialized]
    public UnityEvent onBallStage2Start = new UnityEvent();

    [NonSerialized]
    public UnityEvent onBallStage2End = new UnityEvent();
    [NonSerialized]
    public UnityEvent onBallStage3Start = new UnityEvent();
    [NonSerialized]
    public UnityEvent onBallStage3End = new UnityEvent();

    [NonSerialized]
    public UnityEvent onImpactFrameStart = new UnityEvent();
    [NonSerialized]
    public UnityEvent onImpactFrameEnd = new UnityEvent();
    [NonSerialized]
    public UnityEvent onBallScore = new UnityEvent();
    #endregion

    #region PhyisicsRelatedMethods
    // ? --- WHY: in pratica quando attivo gli impact frame
    // ? --- il sistema di fisica di unity interagisce prima che
    // ? --- io possa freezare la palla, questo genera un bug
    // ? --- per colpa del quale la palla di sposta di un frame 
    // ? --- lontano dal muro prima di essere freezata.
    // ? --- Dunque mi serve ricavare la posizione PRIMA che 
    // ? --- il sistema di fisica intervenga. 
    private Vector2 posInFixedUpdate;

    // ? --- Spostamento che avviene a causa del sistema fisico di Unity
    // ? --- senza che io voglia.
    private Vector2 beforeStopLinearVelocity = Vector2.zero;
    private float beforeStopGravityScale = 0f;

    public void StopBallMovement()
    {
        isMovementStopped = true;

        beforeStopLinearVelocity = rb.linearVelocity;
        beforeStopGravityScale = rb.gravityScale;

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    
    public void StartBallMovement()
    {
        isMovementStopped = false;

        rb.linearVelocity = beforeStopLinearVelocity;
        rb.gravityScale = beforeStopGravityScale;

        // ? --- La rotazione dev'essere sempre lockata.
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void StartImpactFrames(int framesToWait)
    {
        if(!isMovementStopped)
        {        
            // ? --- WHY: se lo stop della palla viene attivato
            // ? --- dalla coroutine, la palla potrebbe bloccarsi
            // ? --- con dei FRAME DI RITARDO a causa deli update
            // ? --- indipendenti del Physics System.
            StartCoroutine(ImpactFramesRoutine(framesToWait));
        }
    }

    IEnumerator ImpactFramesRoutine(int framesToWait)
    {
        StopBallMovement();
        onImpactFrameStart.Invoke();
        Color preaviousColor = outlineScr.OutlineColor;
        outlineScr.OutlineColor = impactFrameColor;
        for(int i = 0; i < framesToWait; i++)
        {
            yield return null;
        }
        outlineScr.OutlineColor = preaviousColor;
        onImpactFrameEnd.Invoke();
        StartBallMovement();
    }


    // ? --- Utilizzeremo questo metodo per applicare la forza
    // ? --- alla palla, in modo che possiamo gestire meglio la logica
    // ? --- e la fisica in generale.
    public void AddForce(Vector2 dirNormalized, int chargeAdd)
    {
        Vector2 linearVel = rb.linearVelocity;
        // ? --- Rende tutto piu' actionnnnn
        rb.linearVelocity = Vector2.zero;

        //Debug.Log("LinearVel: " + linearVel);
        Vector2 resultingChargeForce = 
            new Vector2
            (
                dirNormalized.x * linearVel.magnitude + 
                dirNormalized.x * chargeAdd,
                dirNormalized.y * linearVel.magnitude + 
                dirNormalized.y* chargeAdd
            );
        
        //Debug.Log("RCF: " + resultingChargeForce);

        rb.AddForce(resultingChargeForce, ForceMode2D.Impulse);
    }
    #endregion

    private void Awake()
    {
        fsm = new BallFSM();
        fsm.Setup(this);

        onBallScore.AddListener(
            () =>
            {
                spriteRenderer.gameObject.SetActive(false);
                mesh.SetActive(false);
                goalColl.gameObject.SetActive(false);
                physicsColl.excludeLayers =
                physicsColl.excludeLayers |
                    LayerMask.GetMask("Goal");
            }
        );
    }

    private void Start()
    {
        outlineScr.OutlineColor = stage1Color;
        damage = STAGE1_DAMAGE;
    }


    public void Update()
    {
        ballStage = fsm._currentState._d_stateName;

        if(rb.linearVelocity.magnitude <= 
            MIN_MAGNITUDE_BEFORE_BOUNCES_RESET 
            && !fsm.doImpactFrame.isActive)
        {
            wallBouncesSinceReset = 0;
        }

        magnitude = rb.linearVelocity.magnitude;

        //if(impactFr)
        fsm.Update();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer 
            == LayerMask.NameToLayer("LevelCollider"))
        {

            //TODOStartImpactFrames();
            fsm._currentState.OnCollisionEnter2D(fsm, collision);
            wallBouncesSinceReset++; 
        }    
    }


    #region UNUSED
    // TODO commenta 
    /// <summary>
    /// Guarda spiegazione Bug (#01)  
    /// </summary>
    /// 
    /*
    public void SnapToWall(Collision2D coll)
    {
        ContactPoint2D levelCP =
             HasContactWith("LevelCollider");
    }
    
    ContactPoint2D HasContactWith(
        string layerName, 
        bool mustLog = false)
    {
        ContactPoint2D result = new ContactPoint2D();
        int layer = LayerMask.NameToLayer(layerName);


        foreach(ContactPoint2D cP in activeContactPoints)
        {
            if(mustLog)
            {
                Debug.Log("Contact point with: " + 
                    cP.collider.gameObject.name);
            }

            if(cP.collider.gameObject.layer == 
                layer)
            {
                if (mustLog)
                    Debug.Log("Found right gameObject, exiting");
                
                result = cP;
                return result;
            }
        }

        if(mustLog)
            Debug.Log("Nothing found");

        return result;
    }

    ContactPoint2D[] activeContactPoints;
    private void FixedUpdate()
    {
        posInFixedUpdate = rb.position;

        // ? --- Vedi BUG #31
        ContactPoint2D[] cPs = new ContactPoint2D[10];
        int contactsNum = rb.GetContacts(cPs);

        if(cPs.Length != contactsNum)
        {
            Debug.LogError("Buffer dei contactPoints" + 
                " piu' piccolo del dovuto");
        }
        else
        {
            activeContactPoints = cPs;
        }
    }
    */
    #endregion
}
