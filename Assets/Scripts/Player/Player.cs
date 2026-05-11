using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using EditorAttributes;
using Unity.VisualScripting;


[System.Serializable]
public enum PlayerNumber
{
    PLAYER_1 = 1,
    PLAYER_2
}

public class Player : MonoBehaviour
{
    [field: SerializeField, EditorAttributes.ReadOnly]
    public PlayerNumber plrNumber { get; set; } = new();

    [Header("References")]
    [field: SerializeField] 
    public PlayerBallInteractions pbi; 
    [field: SerializeField] 
    public PlayerJump pj;
    [field: SerializeField] 
    public Rigidbody2D rb { get; private set; }
    [field: SerializeField] 
    public BoxCollider2D bodyColl { get; private set; }
    [field: SerializeField]
    public GameObject meshScalePivot { get; private set; }

    [field: SerializeField]
    public GameObject pivotRotationBasedCollider { get; private set;}
    [field: SerializeField]  
    public GameObject pivotArrowRotation { get; private set; }
    [field: SerializeField]
    public GameObject pivotSpriteColl { get; private set; }
    
    [field: SerializeField] 
    public PlayerBallCollider ballCollider { get; private set; }

    [field: SerializeField] 
    public PlayerInput plrInp { get; private set; }

    [field: SerializeField]
    public GameObject player1Mesh;
    [field: SerializeField]
    public GameObject player2Mesh;

    [field: NonSerialized]
    public GameObject activeMesh { get; private set; }

    void SetMesh()
    {
        switch(plrNumber)
        {
            case PlayerNumber.PLAYER_1:
            player1Mesh.SetActive(true);
            activeMesh = player1Mesh;
            player2Mesh.SetActive(false);
            break;

            case PlayerNumber.PLAYER_2:
            player1Mesh.SetActive(false);
            player2Mesh.SetActive(true);
            activeMesh = player2Mesh;
            break;

            default:
            Debug.Log("Player not set");
            player1Mesh.SetActive(true);
            player2Mesh.SetActive(false);
            break;
        }
    }
    

    // ! --------------------------------------------
    #region Gameplay Stats
    [Header("Gameplay Stats")]
    /// <summary>
    /// Se sto saltando posso oltrepassare le ghostPlatforms
    /// </summary>
    [SerializeField] public bool canPassGhostPlatforms;
    [SerializeField] public float SPEED;
    [SerializeField] public float FALL_SPEED;
    [SerializeField] public float FALL_SPEED_ON_INPUT;
    // ? --- Velocita' con cui si ferma quando viene interrotto
    // ? --- il movimento per cause esterne.
    // ? --- Sara' la t di un lerp.
    [SerializeField] public float STOP_SPEED;

    [FoldoutGroup("Flags", nameof(isOnGround), nameof(canMove),
    nameof(isFacingRight))]
    
    [SerializeField] private EditorAttributes.Void flagsHolder;
    [SerializeField, HideProperty, ReadOnly] public bool isOnGround;
    // ? --- Teoricamente posso rimuoverla
    [SerializeField, HideProperty, ReadOnly] public bool canMove = true;
    [SerializeField, HideProperty, ReadOnly] public bool isFacingRight = false;
    #endregion


    // -------------------------------------------
    // ! Physics
    // -------------------------------------------
    [FoldoutGroup("Physics", nameof(directionLastInput))]
    [SerializeField] private EditorAttributes.Void physicsHolder;


    // ? --- Servira' per sapere la direzione verso cui
    // ? --- imprimere la forza alla palla.
    // ? --- Questa variabile sara' un contenitore dell'ultimo input,
    // ? --- non sara' mai zero, potra' essere usato per sapere dove sta
    // ? --- puntando il giocatore.
    [SerializeField, HideProperty, ReadOnly] public Vector2 directionLastInput;
    //[NonSerialized] public Vector2 directionFacing;


    // -------------------------------------------
    // ! Events
    // -------------------------------------------
    #region Events
    [NonSerialized] 
    public UnityEvent onPlayerMoveEnd = new();
    [NonSerialized] 
    public UnityEvent onPlayerMoveStart = new();
    #endregion
    
    
    public InputAction moveInput { get; private set; }
    public InputAction jumpInput { get; private set; }



    // -------------------------------------------
    // ! Gameplay Logic
    // -------------------------------------------

    #region Gameplay Logic
    /// <summary>
    /// Resetta a valori iniziali il player, la sua fisica e la sua fsm
    /// </summary>
    public void Reset()
    {
        StopAllCoroutines();
        rb.linearVelocity = Vector2.zero;
        EnableMovement();


        // TODO: Brutto ma necessario xD
        GetComponent<PlayerBallInteractions>().Reset();
    } 



    //TODO: Si potrebbe linkare al playerInput, ma
    // bisogna modificarlo

    // ? --- TODO: Ci vuole una lunga spiegazione
    /// <summary>
    /// Quando il player cambia direzione l'input si setta a 0,
    /// per evitare che gli eventi si triggherino ad ogni cambio di direzione
    /// CHE PERO' non rappresenta l'azione di fermarsi, necessiatiamo di 
    /// questa variabile
    /// </summary>
    int movementSteerFrameTolerance;

    [field: Header("Gameplay Logic"), SerializeField]
    public int MOVEMENT_STEER_FRAME_LENGTH_TOLLERANCE { get; private set; }
    bool hasEnded = false;
    bool hasStarted = false;

    // ? --- La parte peggiore del mio codice, rip
    void MovementLogic()
    { 
        Vector2 movementInputValue = 
            plrInp.actions.
                FindAction("Move").ReadValue<Vector2>();
        
        
        if(!canMove)
        {
            return;
        } 



        // -------------------------------------------
        // ! Movement events
        // -------------------------------------------
        // ? --- Un bordello lol
        if(movementInputValue == Vector2.zero)
        {
            movementSteerFrameTolerance++;
        } 
        // ? --- Ogni qual volta siamo in movimento
        else
        {
            movementSteerFrameTolerance = 0;
            //hasStarted = false;
            hasEnded = false; 
        }



        if(movementInputValue == Vector2.zero 
            && movementSteerFrameTolerance >= 
                MOVEMENT_STEER_FRAME_LENGTH_TOLLERANCE
            && !hasEnded)
        {
            onPlayerMoveEnd.Invoke();
            hasEnded = true;
            hasStarted = false;
        }

        if(movementInputValue != Vector2.zero
            && !hasStarted)
        {
            hasStarted = true;
            onPlayerMoveStart.Invoke();
        }




        if(movementInputValue == Vector2.zero)
        {
            // ? --- Resetta asse X, l'unica che ci interessa
            // ? --- Visto che la Y la gestira' il rigidbody per cazzi suoi
            rb.linearVelocity = new Vector2
            (
                0f,
                rb.linearVelocity.y
            );

            // ? --- WHY: serve arrivarea agli altri check
            //return;
        }



        // ? --- Evita che premendo il tasto W si voli
        if(movementInputValue.y > 0f)
        {
            movementInputValue = new Vector2(
                movementInputValue.x,
                0f
            );              
        }

        // ? --- Stiamo premendo S oppure stiamo puntando verso il basso
        if(movementInputValue.y < 0f)
        {
            rb.gravityScale = FALL_SPEED_ON_INPUT;
        }
        // ? --- Se smettiamo di premerlo
        else
        {
            rb.gravityScale = FALL_SPEED;
        }

        //rb.AddForce(movementInputValue * SPEED, ForceMode2D.Impulse);

        //Debug.Log("M: " + movementInputValue.x + " " + movementInputValue.y);
        // TODO --- Va messo in FIXED
        rb.linearVelocity = new Vector2
            (
                SPEED * movementInputValue.x,
                rb.linearVelocity.y
            );
    }
    
    /// <summary>
    /// Ferma tutta la logica del movimento, resettando le variabili
    /// che lo necessitano
    /// </summary>
    public void DisableMovement()
    {
        canMove = false;

        // ? --- Se disabilitiamo l'input mentre il player
        // ? --- utilizza la "FAST FALL" rimarrebbe lockata in quel modo.
        rb.gravityScale = FALL_SPEED;
    }

    public void GhostPlatformsLogic()
    {
        if(canPassGhostPlatforms)
        {
            if(rb.linearVelocityY > 0)
            {
                // ? --- Ha priorita' su includeLayers
                // ? --- dunque non servono check aggiuntivi
                bodyColl.excludeLayers =
                    bodyColl.excludeLayers |
                    LayerMask.GetMask("GhostPlatform");
            }
            else if (rb.linearVelocityY < 0 
                && !pj.doppioScattoPerforming)
            {
                // ? --- Semplice bitwise per mantenere
                // ? --- gli exlcudeLayers origianli, 
                // ? --- staccando pero' solo GhostPlatform
                bodyColl.excludeLayers =
                    bodyColl.excludeLayers &
                    ~LayerMask.GetMask("GhostPlatform");
            }
        }
    }

    public void EnableMovement()
    {
        canMove = true;
    }
    

    /// <summary>
    /// Describes the movement of the feedback arrow
    /// </summary>
    void FeedBackArrowMovement()
    {
        Vector2 movementInputValue = 
            plrInp.actions.FindAction("Move").ReadValue<Vector2>();
            //moveInput.ReadValue<Vector2>();


        if(movementInputValue != Vector2.zero)
        {
            //arrowRotationPivot.transform.LookAt(movementInputValue); 
            float angle = 
                Vector2.SignedAngle
                (
                    new Vector2(1f,0),
                    movementInputValue
                );
            pivotArrowRotation.transform.localRotation = 
                Quaternion.Euler(0f, 0f, angle);

        }
    }



    public void RotateTowardsFeedbackArrow()
    {
        pivotSpriteColl.transform.localRotation = 
            Quaternion.Euler(new Vector3
            (
                0f,
                0f,
                pivotArrowRotation.transform.localEulerAngles.z
            ));
    }
    
    /// <summary>
    /// Ruota i pivot dei collider in base
    /// alla direzione che si sta puntando.
    /// </summary>
    public void AdjustRotationBasedColliders()
    {
        if(directionLastInput == Vector2.zero)
            return;

        // ? --- Angolo fra l'origine del mondo e l'asse di input.
        // ? --- Entrambi sono normalizzati.
        float angleBetween =
            Vector2.SignedAngle
            (
                Vector2.right,
                directionLastInput
            ); 
        
        // ? --- Chiaramente ci interessa solo la rotazione sulla z
        // ? --- la quale coinvolge gli assi X e Y.
        pivotRotationBasedCollider.transform.localRotation =
            Quaternion.Euler(
                0f,
                0f,
                angleBetween
            );
    }

    public void ResetSpriteAndCollRotation()
    {
         pivotSpriteColl.transform.localRotation = 
            Quaternion.Euler(new Vector3
            (
                0f,
                0f,
                0f
            ));
    }

    /// <summary>
    /// Specchia il player verso una direzione in base all'input.
    /// In questo momento si basa solo su destra e sinistra.
    /// </summary>
    public void AdjustSpriteBasedOnDirection()
    {
        Vector2 inputDirection = 
            moveInput.ReadValue<Vector2>();

        if(inputDirection.x > 0)
        {
            isFacingRight = true;
            bodyColl.gameObject.transform.localScale = 
                new Vector3(
                    Mathf.Abs(bodyColl.transform.localScale.x),
                    bodyColl.transform.localScale.y,
                    bodyColl.transform.localScale.z
                );
            meshScalePivot.transform.localScale =
                new Vector3(
                    Mathf.Abs(meshScalePivot.transform.localScale.x),
                    meshScalePivot.transform.localScale.y,
                    meshScalePivot.transform.localScale.z);
        }
        else if(inputDirection.x < 0)
        {
            isFacingRight = false;
            bodyColl.gameObject.transform.localScale = 
                new Vector3( 
                    -Mathf.Abs(bodyColl.transform.localScale.x),
                    bodyColl.transform.localScale.y,
                    bodyColl.transform.localScale.z
                );

            meshScalePivot.transform.localScale =
                new Vector3(
                    -Mathf.Abs(meshScalePivot.transform.localScale.x),
                    meshScalePivot.transform.localScale.y,
                    meshScalePivot.transform.localScale.z);
        }
    }
    

    #endregion



    void Awake()
    {
        canMove = true;

        SetMesh();
    } 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput = plrInp.actions.FindAction("Move");
        jumpInput = plrInp.actions.FindAction("Jump");

        rb.gravityScale = FALL_SPEED;

    }

    void Update()
    {
        if(moveInput.ReadValue<Vector2>() != Vector2.zero)
        {
            directionLastInput = moveInput.ReadValue<Vector2>();
        }

        if((pbi.fsm.kickState.isActive ||
            pbi.fsm.chargingState.isActive) || 
            pj.doppioScattoPerforming)
        {
            DisableMovement();

            float deacreaseLinVelX =
                Mathf.Lerp
                (
                    rb.linearVelocityX,
                    0f,
                    STOP_SPEED
                );
            

            rb.linearVelocity = new Vector2
            (
                deacreaseLinVelX,
                rb.linearVelocityY
            );
        }
        //else if((pj.firstJumpPerforming || pj.doppioScattoPerforming))
        //{
        //    DisableMovement();
        //}
        else
        {
            EnableMovement();
            AdjustSpriteBasedOnDirection();
            AdjustRotationBasedColliders(); 
        }
        
        GhostPlatformsLogic();
        // ? --- Se no, quando si disabilita' il movimento
        // ? --- continua a traslare all'infinito poiche'
        // ? --- non viene resettata la velocita' lineare.
        // ? --- In questo modo la scalo senza dare l'effetto 
        // ? --- di stop repentino.
        if(!canMove && rb.linearVelocityX != 0)
        {
            
            float result = 
                Mathf.Lerp(rb.linearVelocity.x, 0, 0.02f);

            rb.linearVelocity = new Vector2(
                result, 
                rb.linearVelocity.y 
            );
        }
 
        FeedBackArrowMovement();
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        MovementLogic();
    }
}
