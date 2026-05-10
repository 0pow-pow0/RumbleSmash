using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("References"), SerializeField]
    GameObject[] leftRaySpawnPivot;
    [SerializeField]
    GameObject[] rightRaySpawnPivot;
    [SerializeField]
    GameObject[] topRaySpawnPivot;

    [SerializeField]
    GameObject[] weaponSpawnPivot;


    #region Gameplay Stats
    [Header("Ray Stats"), SerializeField]
    #region Ray Logic
    public float raySpawnSpeed;
    public float rayTravelSpeed;
    #endregion

    #region WeaponLogic
    [Header("Weapon Stats")]
    public float weaponSpawnSpeed;
    public float projectilesSpeed;
    public float fireRate; 
    #endregion
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
