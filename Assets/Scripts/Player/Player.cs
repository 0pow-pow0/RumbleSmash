using System;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public GameObject arrowRotationPivot;



    // ! --------------------------------------------
    [Header("Gameplay Stats")]
    [SerializeField] public float SPEED = 10f;

    // ? --- Gestione del doppioScatto
    [SerializeField] public float JUMP_FORCE = 0.5f;
    [SerializeField] public float DOPPIOSCATTO_FORCE = 0.5f;
    
    public bool isOnGround;
    public bool firstJumpPerformed;
    public bool doppioScattoPerformed;



    // -------------------------------------------
    // ! Events
    // -------------------------------------------
    UnityEvent OnPlayerMove;


    InputAction moveInput;
    InputAction jumpInput;
    

    // -------------------------------------------
    // Input Related
    // -------------------------------------------
    void MovementLogic()
    {
        Vector2 movementInputValue = 
            moveInput.ReadValue<Vector2>();


        // ? --- Evita che premendo il tasto W si voli
        if(movementInputValue.y > 0f)
        {
            movementInputValue = new Vector2(
                movementInputValue.x,
                0f
            );              
        }

        rb.AddForce(movementInputValue * SPEED, ForceMode2D.Impulse);
        Debug.Log(movementInputValue);
    }
    

    
    void DoppioScattoLogic()
    {
        if(jumpInput.WasPerformedThisFrame())
        {
            Debug.Log("InputArrivato");
            Vector2 moveInputValue = 
                moveInput.ReadValue<Vector2>();

            // ? --- Primo Salto
            if(!firstJumpPerformed)
            {
                Debug.Log("PrimoSalto");
                firstJumpPerformed = true;
                
                // ? --- Rimuovi momentum dovuta gravità
                rb.linearVelocity = new Vector2
                (
                    rb.linearVelocity.x,
                    0f
                );

                // ? --- Direzione del salto = Su
                rb.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);
            }

            // ? --- DoppioScatto
            else if(!doppioScattoPerformed)
            {
                doppioScattoPerformed = true;
                
                // ? --- Visto che possiamo saltare in tutte
                // ? --- le direzioni, resettiamo totalmente
                rb.linearVelocity = Vector2.zero;

                

                rb.AddForce(moveInputValue * DOPPIOSCATTO_FORCE, 
                    ForceMode2D.Impulse);
            }
        }
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

            Quaternion rot = Quaternion.LookRotation(movementInputValue);
            arrowRotationPivot.transform.localRotation = rot; 
        }
    }


    // -------------------------------------------
    // ! Physics
    // -------------------------------------------
    public void ResetJumpConditions()
    {
        firstJumpPerformed = false;
        doppioScattoPerformed = false;
    }



    void Awake()
    {
        
    } 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput = InputSystem.actions.FindAction("Move");
        jumpInput = InputSystem.actions.FindAction("Jump");
    }



    // Update is called once per frame
    void Update()
    {
        MovementLogic();

        DoppioScattoLogic();

    }
}
