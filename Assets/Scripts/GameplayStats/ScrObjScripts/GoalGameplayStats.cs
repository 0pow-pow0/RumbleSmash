using UnityEngine;

[CreateAssetMenu(fileName = "GoalGameplayStats", menuName = "GameplayStats/GoalGameplayStats")]
public class GoalGameplayStats : ScriptableObject
{
    public int START_SHIELD_HP;

    public float INVULNERABILITY_TIME;

    // -------------------------------------------
    // ! Threshold
    // -------------------------------------------
    // ? --- Sotto che soglia si rompe un pochino la barriera?
    public int THRESHOLD_HP_STAGE1_BREAK;
    public int THRESHOLD_HP_STAGE2_BREAK;
    public int THRESHOLD_HP_STAGE3_BREAK;
}
