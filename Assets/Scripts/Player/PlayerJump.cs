using System;
using UnityEngine;
using UnityEngine.Events;
using EditorAttributes;
using UtilityShit;

/// <summary>
/// Quando il giocatore effettua il salto si muovera' di 
/// X unita' senza possibilita' di utilizzare input.
/// </summary>
public class PlayerJump : MonoBehaviour
{
    [SerializeField] Player plr;

    #region Gameplay Stats
    // ? --- Gestione del doppioScatto
    [field: Header("Gameplay Stats"), SerializeField] 
    public float JUMP_FORCE { get; private set; }

    [field: SerializeField] 
    public float DOPPIOSCATTO_FORCE { get; private set; }    

    // ? --- Il doppioScatto si interrompera' dopo X frame
    [field: SerializeField] 
    public int DOPPIOSCATTO_FRAME_DURATION { get; private set; }
    
    [field: SerializeField] 
    public float DOPPIOSCATTO_COOLDOWN_DURATION { get; private set; }
    private Timer doppioScattoCooldownTimer;
    
    #endregion


    //[Header("Flags")]
    [FoldoutGroup("Flags", nameof(firstJumpPerformed), nameof(firstJumpPerforming),
         nameof(doppioScattoPerformed),  nameof(doppioScattoPerforming),
         nameof(canPerformJustPressedJumpInputEvent), 
         nameof(canPerformJustPressedReleaseEvent))]

    [SerializeField]
    private EditorAttributes.Void flagsHolder;

    // ? --- Serve per sapere se e' gia' stato effettuato il primo salto
    [SerializeField, ReadOnly, HideProperty]
    public bool firstJumpPerformed;
    
    // ? --- Serve per sapere se stiamo PERFORMANDO il salto
    [SerializeField, ReadOnly, HideProperty]
    public bool firstJumpPerforming;
    [Space]
    
    // ? --- Stessa cosa ma per il doppioScatto
    [SerializeField, ReadOnly, HideProperty]
    public bool doppioScattoPerformed;

    // ? --- La differenza tra questo e' "doppioScattoPerformed" e'
    // ? --- che non dipende dai check fatti col ground.
    // ? --- Questa variabile gestisce solo i calcoli applicati al movimento,
    // ? --- una volta raggiunta la "DOPPIOSCATTO_MAX_DISTANCE",
    // ? --- la variabile si resetta.
    [SerializeField, ReadOnly, HideProperty]
    public bool doppioScattoPerforming;

    // ? --- Usato per triggherare i particellare nei punti giusti
    [SerializeField, ReadOnly, HideProperty] 
    public Vector2 doppioScattoDirection;



    /// <summary>
    /// Il motivo per cui queste varibili esistono e' dovuto al fatto che
    /// il retrieve dell'input avviene negli UPDATE, ma siccome al
    /// ricevimento dell'input io dovrei modificare la linearvelocity e
    /// questo puo' avvenire solo nell'fixedUpdate ne'
    /// consegue che input retrival e azione avvengono in due momenti opposti.
    /// 
    /// Dunque queste variabili vengono settate nell'update 
    /// e lette nel fixed, dove modifichero' la linearVel.
    /// 
    /// Vengono resettate nell'EvaluatePhysics
    /// </summary>
    [SerializeField, ReadOnly, HideProperty] 
    bool canPerformJustPressedJumpInputEvent = false;
    
    [SerializeField, ReadOnly, HideProperty] 
    bool canPerformJustPressedReleaseEvent = false;


    // -------------------------------------------
    // ! Events
    // -------------------------------------------
    #region Events
    [NonSerialized]
    public UnityEvent onFirstJumpPerformed = new();
    [NonSerialized]
    public UnityEvent onLand = new();
    [NonSerialized]
    public UnityEvent onDoppioScattoStart = new();
    [NonSerialized]
    public UnityEvent onDoppioScattoEnd = new();
    #endregion


    // ? --- Potrei anche fare dei metodi che si richiamano fino a che
    // ? --- non si termina uno dei due salti
    void InputRetrieve()
    {


        if(!plr.plrInp.inputIsActive)
            return;

        if(plr.jumpInput.WasPerformedThisFrame())
        {
            canPerformJustPressedJumpInputEvent = true;
            canPerformJustPressedReleaseEvent = false;
        }
    
        if(plr.jumpInput.WasReleasedThisFrame())
        {
            
            canPerformJustPressedReleaseEvent = true;
        }
    } 

    void EvaluatePhysics()
    {
        if(!plr.plrInp.inputIsActive)
            return;

        if(canPerformJustPressedJumpInputEvent)
        {
            canPerformJustPressedJumpInputEvent = false;
            //Debug.Log("InputArrivato");
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
                && !doppioScattoPerforming 
                && doppioScattoCooldownTimer.HasEnded())
            {
                firstJumpPerforming = false;
                doppioScattoPerformed = true;
                doppioScattoPerforming = true;
                doppioScattoCooldownTimer.Restart();
                plr.DisableMovement();
                
                // ? --- Visto che possiamo saltare in tutte
                // ? --- le direzioni, resettiamo totalmente
                plr.rb.linearVelocity = Vector2.zero;
                plr.rb.gravityScale = 0f;

                // ! --- Al momento se non diamo nulla in input 
                // ! --- per il doppioScatto andiamo semplicemente
                // ! --- verso l'alto.
                Vector2 doppioScattoValue = moveInputValue;
                frameDur = 0 ;
                if(doppioScattoValue == Vector2.zero)
                    doppioScattoValue = new Vector2(0f, 1f);

                //plr.rb.AddForce(doppioScattoValue * DOPPIOSCATTO_FORCE, 
                //    ForceMode2D.Impulse);

                plr.rb.linearVelocity = 
                    doppioScattoValue * DOPPIOSCATTO_FORCE;

                plr.moveInput.Disable();


                doppioScattoDirection = doppioScattoValue;
                onDoppioScattoStart.Invoke();
            }
            
            
        }   
            
        if(canPerformJustPressedReleaseEvent &&
            // ? --- Solo se stiamo gia' volando     
            firstJumpPerforming
            )
        {
            firstJumpPerforming = false;
            // plr.rb.linearVelocity = new Vector2
            // (
            //     plr.rb.linearVelocity.x,
            //     0f
            // );

            Vector2 lerpedSpeedToZero = 
                Vector2.Lerp(plr.rb.linearVelocity, 
                new Vector2(plr.rb.linearVelocity.x, 0f), 0.7f);

            plr.rb.linearVelocity = lerpedSpeedToZero;

            if(plr.rb.linearVelocityX == 0f)
                canPerformJustPressedReleaseEvent = false;
            // canPerformJustPressedReleaseEvent = false;
        }
    }

    /// <summary>
    /// ? Controlla che tutte le condizioni siano vere
    /// ? in caso contrario setta le flag
    /// </summary>
    void FirstJumpUpdate()
    {
        if(plr.rb.linearVelocity.y < 0)
        {
            firstJumpPerforming = false;
        }
    }

    // ? --- Utilizzato solo quando si sta effettuando il DoppioScatto
    int frameDur = 0;
    void DoppioScattoLogic()
    {
        if(doppioScattoPerforming)
        {
            frameDur++;
            
            if(
                frameDur 
                >= 
                DOPPIOSCATTO_FRAME_DURATION 
                ||
                // ? --- Se si ferma contro un ostacolo 
                plr.rb.linearVelocity == Vector2.zero)
            {
                doppioScattoPerforming = false;

                // ? --- E' un erorre architetturale questo if,
                // ? --- in ogni caso, il controllo del movimento
                // ? --- e' prioritario del kick e charge state cosi' in 
                // ? --- caso si calciasse e saltasse non venisse attivato
                // ? --- il movimento a causa del salto MENTRE si calcia.
                //if(!plr.pbi.fsm.kickState.isActive &&
                //    !plr.pbi.fsm.chargingState.isActive)
                //{
                //    plr.EnableMovement();
                //}
                onDoppioScattoEnd.Invoke();
                plr.rb.linearVelocity = Vector2.zero;
                plr.rb.gravityScale = 1f;
                plr.moveInput.Enable();
                doppioScattoDirection = Vector2.zero;
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
        doppioScattoCooldownTimer = new 
            Timer(DOPPIOSCATTO_COOLDOWN_DURATION);
    }

    void Update()
    {
        doppioScattoCooldownTimer.UpdateTime();

        if(!plr.pbi.fsm.kickState.isActive &&
            !plr.pbi.fsm.chargingState.isActive)
        {
            InputRetrieve();
        }

        /// Ho aumentato il numero delle chiamate del FixedUpdate
        /// in modo che sia piu' preciso, questo toglie il problema 
        /// del tunneling. 
        /// 
        /// Mettendo questa funzione dentro il fixedUpdate, si bugga
        /// la chiamata degli eventi dei particellari per questioni di timing.
        /// Per ora, dunque, lo lascio qui, visto che alla fine,
        /// non causava tunneling
        EvaluatePhysics();
        //DoppioScattoLogic();
    }

    void FixedUpdate()
    {
        FirstJumpUpdate();
        DoppioScattoLogic();
    }
}
