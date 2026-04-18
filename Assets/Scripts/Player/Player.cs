using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using EditorAttributes;
using EditorAttributes.Editor;

public class Player : MonoBehaviour
{
    [Header("References")]
    [field: SerializeField] 
    public Rigidbody2D rb { get; private set; }
    [field: SerializeField] 
    public GameObject arrowRotationPivot { get; private set; }
    [field: SerializeField] 
    public PlayerBallCollider ballCollider { get; private set; }
    [field: SerializeField] 
    public PlayerInput plrInp { get; private set; }


    // ! --------------------------------------------
    #region GameplayStats
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
    [NonSerialized] public UnityEvent OnPlayerMove;


    public InputAction moveInput { get; private set; }
    public InputAction jumpInput { get; private set; }



    // -------------------------------------------
    // ! Gameplay Logic
    // -------------------------------------------
    //TODO: Si potrebbe linkare al playerInput, ma
    // bisogna modificarlo
    void    MovementLogic()
    { 
        Vector2 movementInputValue = 
            plrInp.actions.
                FindAction("Move").ReadValue<Vector2>();
            //moveInput.ReadValue<Vector2>();
        Debug.Log("ASD: " + moveInput.ReadValue<Vector2>());
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

        Debug.Log("M: " + movementInputValue.x + " " + movementInputValue.y);
        // TODO --- Va messo in FIXED
        rb.linearVelocity = new Vector2
            (
                SPEED * movementInputValue.x,
                rb.linearVelocity.y
            );
        Debug.Log("Porcodio");

        OnPlayerMove.Invoke();
    }
    
    /// <summary>
    /// Ferma tutta la logica del movimento, resettando le variabili
    /// che lo necessitano
    /// </summary>
    public void DisableMovement()
    {
        canMove = false;
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
        plrInp.ActivateInput();

        if(movementInputValue != Vector2.zero)
        {
            //arrowRotationPivot.transform.LookAt(movementInputValue); 
            float angle = 
                Vector2.SignedAngle
                (
                    new Vector2(1f,0),
                    movementInputValue
                );
            arrowRotationPivot.transform.localRotation = 
                Quaternion.Euler(0f, 0f, angle);

        }
    }




    void Awake()
    {
        OnPlayerMove = new UnityEvent();
        canMove = true;

        plrInp = GetComponent<PlayerInput>();
    } 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput = plrInp.actions.FindAction("Move");
        jumpInput = plrInp.actions.FindAction("Jump");
        Debug.Log(plrInp.devices.Count);

        rb.gravityScale = FALL_SPEED;
    }

    void Update()
    {

        //Debug.Log("Gm: " + GameManager.Get().ball);
        if(moveInput.ReadValue<Vector2>() != Vector2.zero)
        {
            directionLastInput = moveInput.ReadValue<Vector2>();
        }
        plrInp.ActivateInput();
        Debug.Log("PlayerInp: " + plrInp.inputIsActive);
        Debug.Log("ActionInp: " + moveInput.enabled);
        Debug.Log("GetAction: " + plrInp.actions.FindAction("Move").ReadValue<Vector2>());        
        Debug.Log("Action: " + moveInput.ReadValue<Vector2>());        

        MovementLogic();
        FeedBackArrowMovement();
    }

    public void OnMove()
    {
        Debug.Log("Player: OnMove");
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
