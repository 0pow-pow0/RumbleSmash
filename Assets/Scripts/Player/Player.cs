using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using EditorAttributes;
using Unity.VisualScripting;


public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public GameObject arrowRotationPivot;



    // ! --------------------------------------------
    [Header("Gameplay Stats")]
    [SerializeField] public float SPEED;
    [SerializeField] public float FALL_SPEED;
    [SerializeField] public float FALL_SPEED_ON_INPUT;
    [SerializeField, ReadOnly] public bool isOnGround;
    [SerializeField, ReadOnly] private bool canMove = true;



    // -------------------------------------------
    // ! Events
    // -------------------------------------------
    public UnityEvent OnPlayerMove;


    public InputAction moveInput { get; private set; }
    public InputAction jumpInput { get; private set; }
    



    // -------------------------------------------
    // Gameplay Logic
    // -------------------------------------------
    private float lastMovInputValueX = 0f;
    private float lastMovInputValueY = 0f;
    void MovementLogic()
    {
        Vector2 movementInputValue = 
            moveInput.ReadValue<Vector2>();
        
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

            return;
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

        Vector2 position2D = transform.position;
        // TODO --- Va messo in FIXED
        rb.linearVelocity = new Vector2
            (
                SPEED * movementInputValue.x,
                rb.linearVelocity.y
            );

            


        OnPlayerMove.Invoke();
        Debug.Log(movementInputValue);
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

    void MovementLogicTest()
    {
        Vector2 movementInputValue = 
            moveInput.ReadValue<Vector2>();
        
        rb.position= new Vector2
        (
            rb.position.x + SPEED * movementInputValue.x,
            rb.position.y + SPEED * movementInputValue.y
        );
    }
    
    

    /// <summary>
    /// Describes the movement of the feedback arrow
    /// </summary>
    void FeedBackArrowMovement()
    {
        Vector2 movementInputValue = 
            moveInput.ReadValue<Vector2>();


        if(movementInputValue != Vector2.zero)
        {
            //arrowRotationPivot.transform.LookAt(movementInputValue); 
            float angle = Vector2.SignedAngle(new Vector2(1f,0), movementInputValue);
            Debug.Log("Angle: " + angle); 
            arrowRotationPivot.transform.localRotation = 
            Quaternion.Euler(0f, 0f, angle);
        }
    }




    void Awake()
    {
        OnPlayerMove = new UnityEvent();
        canMove = true;
    } 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput = InputSystem.actions.FindAction("Move");
        jumpInput = InputSystem.actions.FindAction("Jump");

        rb.gravityScale = FALL_SPEED;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        MovementLogic();
        FeedBackArrowMovement();
    }
}
