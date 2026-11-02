using UnityEngine;

[CreateAssetMenu(fileName = "PlayerGameplayStats", menuName = "GameplayStats/PlayerGameplayStats")]
public class PlayerGameplayStats : ScriptableObject
{
    [Header("Movement Related"), SerializeField]
    public bool canPassGhostPlatforms;
    [SerializeField] public float SPEED;
    [SerializeField] public float FALL_SPEED;
    [SerializeField] public float FALL_SPEED_ON_INPUT;
    // ? --- Velocita' con cui si ferma quando viene interrotto
    // ? --- il movimento per cause esterne.
    // ? --- Sara' la t di un lerp.
    [SerializeField] public float STOP_SPEED;



    //--------------------------------------------
    [field: Header("Jump Related"), SerializeField]
    public float JUMP_FORCE { get; private set; }

    [field: SerializeField] 
    public float DOPPIOSCATTO_FORCE { get; private set; }    

    // ? --- Il doppioScatto si interrompera' dopo X frame
    [field: SerializeField] 
    public int DOPPIOSCATTO_FRAME_DURATION { get; private set; }
        
    [field: SerializeField] 
    public float DOPPIOSCATTO_COOLDOWN_DURATION { get; private set; }



    //--------------------------------------------
    [field: Header("Kick Related"), SerializeField]
    public int KICK_FORCE { get; private set; }
    
    // ? --- La forza del calcio quando viene chargato al massimo.
    // ? --- Il calcolo della forza finale che verra' applicato 
    // ? --- alla palla:
    // ? --- KICK_FORCE + ((tempo di prezzione[da 0 a 1]) * KICK_FORCE_CHARGED_MAX)
    [field:SerializeField] public int KICK_FORCE_CHARGED_MAX { get; private set; }


    // ? --- Spinta derivata dal calcio
    [field:SerializeField] 
    public int KICK_FORCE_PUSH { get; private set; }

    [field:SerializeField, Space(10)]
    public float TIME_TO_REACH_MAX_CHARGE { get; private set; }

    // ? --- Tempo di pressione necessaria prima 
    // ? --- di CARICA che il giocatore entri in stato di CARICA
    [field:SerializeField] 
    public float CHARGE_WINDOW { get; private set; }

    [field:SerializeField] 
    public int KICK_COLLIDER_FRAME_DURATION { get; private set; }

}
