using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using EditorAttributes;


[System.Serializable]
public enum PlayerNumber
{
    PLAYER_1 = 1,
    PLAYER_2
}

public class Player : MonoBehaviour
{
    [field: SerializeField, EditorAttributes.ReadOnly]
    public PlayerNumber plrNumber { get; private set; } = new();

    [Header("References")]
    [field: SerializeField] 
    public Rigidbody2D rb { get; private set; }
    [field: SerializeField]  
    public GameObject pivotArrowRotation { get; private set; }
    [field: SerializeField]
    public GameObject pivotSpriteColl { get; private set; }
    [field: SerializeField] 
    public PlayerBallCollider ballCollider { get; private set; }
    [field: NonSerialized] 
    public PlayerInput plrInp { get; private set; }
    

    // ! --------------------------------------------
    #region Gameplay Stats
    [Header("Gameplay Stats")]
    [SerializeField] public float SPEED;
    [SerializeField] public float FALL_SPEED;
    [SerializeField] public float FALL_SPEED_ON_INPUT;

    [FoldoutGroup("Flags", nameof(isOnGround), nameof(canMove))]
    
    [SerializeField] private EditorAttributes.Void flagsHolder;
    [SerializeField, HideProperty, ReadOnly] public bool isOnGround;
    [SerializeField, HideProperty, ReadOnly] private bool canMove = true;
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
    [NonSerialized] public UnityEvent OnPlayerMove = new();


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
    void MovementLogic()
    { 
        Vector2 movementInputValue = 
            plrInp.actions.
                FindAction("Move").ReadValue<Vector2>();

        if(!canMove)
            return;

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

        OnPlayerMove.Invoke();
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

    #endregion



    void Awake()
    {
        canMove = true;

        plrInp = GetComponent<PlayerInput>();
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

        MovementLogic();
        FeedBackArrowMovement();
    }



    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
