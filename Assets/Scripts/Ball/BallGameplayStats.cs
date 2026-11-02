using UnityEngine;

[CreateAssetMenu(fileName = "BallGameplayStats", menuName = "GameplayStats/BallGameplayStats")]
public class BallGameplayStats : ScriptableObject
{    
    [field: SerializeField]
    public Color impactFrameColor { get; private set; }


    #region STAGE_1
    [field: Header("Stage 1 Stats"), SerializeField] 
    public PhysicsMaterial2D stage1Bounciness;

    [field: SerializeField]
    public Color stage1Color { get; private set; }

    [field: SerializeField]
    public int STAGE1_DAMAGE { get; private set; }

    [field: SerializeField]
    public float STAGE1_MIN_MAGNITUDE { get; private set; }
    [field: SerializeField]
    public float STAGE1_GRAVITY_SCALE { get; private set; }
    #endregion

    #region STAGE_2
    [field: Header("Stage 2 Stats"), SerializeField] 
    public PhysicsMaterial2D stage2Bounciness;

    [field: SerializeField]
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

    #region STAGE_3
    [field: Header("Stage 3 Stats"), SerializeField] 
    public PhysicsMaterial2D stage3Bounciness;

    [field: SerializeField]
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
    public PhysicsMaterial2D STAGE_3_MIN_TIME_ENDED_MATERIAL { get; private set; }
    
    [field: SerializeField]
    public int STAGE3_FIRST_COLL_IF { get; private set; }

    #endregion
}
