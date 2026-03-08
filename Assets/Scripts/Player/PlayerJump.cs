using System;
using UnityEngine;
using UnityEngine.Events;
using EditorAttributes;

/// <summary>
/// Quando il giocatore effettua il salto si muovera' di 
/// X unita' senza possibilita' di utilizzare input.
/// </summary>
public class PlayerJump : MonoBehaviour
{
    [SerializeField] Player plr;

    [Header("Gameplay Stats")]
    // ? --- Gestione del doppioScatto
    [SerializeField] public float JUMP_FORCE;
    [SerializeField] public float DOPPIOSCATTO_FORCE;    

    // ? --- Il doppioScatto si interrompera' dopo X metri percorsi
    private float distanceTravelledDoppioScatto;
    [SerializeField] public float DOPPIOSCATTO_MAX_DISTANCE;
    


    //[Header("Flags")]
    [FoldoutGroup("Flags", nameof(firstJumpPerformed), nameof(firstJumpPerforming),
         nameof(doppioScattoPerformed),  nameof(doppioScattoPerforming))]

    [SerializeField] private EditorAttributes.Void flagsHolder;

    // ? --- Serve per sapere se e' gia' stato effettuato il primo salto
    [SerializeField, ReadOnly, HideProperty] public bool firstJumpPerformed;
    // ? --- Serve per sapere se stiamo PERFORMANDO il salto
    [SerializeField, ReadOnly, HideProperty] public bool firstJumpPerforming;
    [Space]
    // ? --- Stessa cosa ma per il doppioScatto
    [SerializeField, ReadOnly, HideProperty] public bool doppioScattoPerformed;

    // ? --- La differenza tra questo e' "doppioScattoPerformed" e'
    // ? --- che non dipende dai check fatti col ground.
    // ? --- Questa variabile gestisce solo i calcoli applicati al movimento,
    // ? --- una volta raggiunta la "DOPPIOSCATTO_MAX_DISTANCE",
    // ? --- la variabile si resetta.
    [SerializeField, ReadOnly, HideProperty] public bool doppioScattoPerforming;



    // -------------------------------------------
    // ! Events
    // -------------------------------------------
    UnityEvent onFirstJumpPerformed;
    UnityEvent onDoppioScattoPerformed;

    // ? --- Potrei anche fare dei metodi che si richiamano fino a che
    // ? --- non si termina uno dei due salti



    void InputLogic()
    {
        if(plr.jumpInput.WasPerformedThisFrame())
        {
            Debug.Log("InputArrivato");
            Vector2 moveInputValue = 
                plr.moveInput.ReadValue<Vector2>();

            // ? --- Primo Salto
            if(!firstJumpPerformed)
            {
                Debug.Log("PrimoSalto");
                firstJumpPerformed = true;
                firstJumpPerforming = true;
                
                // ? --- Rimuovi momentum dovuto alla gravità
                plr.rb.linearVelocity = new Vector2
                (
                    plr.rb.linearVelocity.x,
                    0f
                );

                // ? --- Direzione del salto = Su
                plr.rb.AddForce(Vector2.up * JUMP_FORCE, ForceMode2D.Impulse);

                onFirstJumpPerformed.Invoke ();
            }

            // ? --- DoppioScatto
            else if(!doppioScattoPerformed
                // ? --- Se stai gia' performando il salto, non entrare 
                && !doppioScattoPerforming )
            {
                firstJumpPerforming = false;
                doppioScattoPerformed = true;
                doppioScattoPerforming = true;
                plr.DisableMovement();
                
                // ? --- Visto che possiamo saltare in tutte
                // ? --- le direzioni, resettiamo totalmente
                plr.rb.linearVelocity = Vector2.zero;
                plr.rb.gravityScale = 0f;

                // ! --- Al momento se non diamo nulla in input 
                // ! --- per il doppioScatto andiamo semplicemente
                // ! --- verso l'alto.
                Vector2 doppioScattoValue = moveInputValue;
                
                if(doppioScattoValue == Vector2.zero)
                    doppioScattoValue = new Vector2(0f, 1f);

                plr.rb.AddForce(doppioScattoValue * DOPPIOSCATTO_FORCE, 
                    ForceMode2D.Impulse);

                plr.moveInput.Disable();


                onDoppioScattoPerformed.Invoke();
            }
        }
    
        if(plr.jumpInput.WasReleasedThisFrame() &&
            // ? --- Solo se stiamo gia' volando     
            firstJumpPerforming
            )
        {
            firstJumpPerforming = false;
            plr.rb.linearVelocity = new Vector2
            (
                plr.rb.linearVelocity.x,
                0f
            );
        }
    }

    /// <summary>
    /// ? Controlla che tutte le condizioni siano vere
    /// ? in caso contrario setta le flag
    /// </summary>
    void FirstJumpLogic()
    {
        if(plr.rb.linearVelocity.y < 0)
        {
            firstJumpPerforming = false;
        }
    }

    // ? --- Utilizzato solo quando si sta effettuando il DoppioScatto
    void DoppioScattoLogic()
    {
        if(doppioScattoPerforming)
        {
            distanceTravelledDoppioScatto += 
                plr.rb.linearVelocity.magnitude * Time.deltaTime;        
            
            
            Debug.Log("Speed: " + plr.rb.linearVelocity 
                + " " + distanceTravelledDoppioScatto);


            if(
                distanceTravelledDoppioScatto 
                >= 
                DOPPIOSCATTO_MAX_DISTANCE 
                ||
                // ? --- Se si ferma contro un ostacolo 
                plr.rb.linearVelocity == Vector2.zero)
            {
                doppioScattoPerforming = false;
                plr.EnableMovement();

                distanceTravelledDoppioScatto = 0f;
                plr.rb.linearVelocity = Vector2.zero;
                plr.rb.gravityScale = 1f;
                plr.moveInput.Enable();
            }
        }
    }

    public void ResetJumpConditions()
    {
        // ? --- Non serve
        firstJumpPerformed = false;
        doppioScattoPerformed = false;
    }



    void Awake()
    {
        onFirstJumpPerformed = new UnityEvent();
        onDoppioScattoPerformed = new UnityEvent();
    }

    void Update()
    {
        InputLogic();
        FirstJumpLogic();
        DoppioScattoLogic();
    }
}
