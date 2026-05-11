 using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.ProBuilder;
using UnityEngine.Events;

public class TargetManager : MonoBehaviour
{
    [Header("References"), SerializeField]
    GameObject targetPrefab;


    /// <summary>
    /// Spawnero' tutti i target sul perimetro di questo
    /// box collider
    /// </summary>
    [SerializeField]
    BoxCollider2D perimeterSpawn;


    public (Vector2 randPoint, Vector2 normalToCenter) GetRandomBoxPerimeterPoint(bool noBottom = true)
    {
        Vector2 result;
        Vector2 normalTowardsCenterScreen;

        // ? --- Scegli su quale lato comparire del perimetro
        float randSide;
        
        if(noBottom)
            randSide = Random.Range(0f, 0.75f);
        else
            randSide = Random.value;
        

        // ? --- Lato sinistro
        if(randSide <= 0.25f)
        {
            result = new Vector2
            (
                perimeterSpawn.bounds.center.x - 
                perimeterSpawn.bounds.extents.x,
                Random.Range(
                    perimeterSpawn.bounds.center.y - 
                    perimeterSpawn.bounds.extents.y,
                    perimeterSpawn.bounds.center.y +
                    perimeterSpawn.bounds.extents.y)
            );

            normalTowardsCenterScreen = 
                new Vector2(1f, 0);
        }
        // ? --- Lato superiore
        else if(randSide <= 0.5f)
        {
            result = new Vector2
            (
                Random.Range
                (
                    perimeterSpawn.bounds.center.x -
                    perimeterSpawn.bounds.extents.x,
                    
                    perimeterSpawn.bounds.center.x +
                    perimeterSpawn.bounds.extents.x

                ),
                perimeterSpawn.bounds.center.y +
                perimeterSpawn.bounds.extents.y
            );

            normalTowardsCenterScreen = 
                new Vector2(0, -1f);
        }
        // ? --- Lato Destro
        else if(randSide <= 0.75f)
        {
            result = new Vector2
            (
                perimeterSpawn.bounds.center.x + 
                perimeterSpawn.bounds.extents.x,
                Random.Range(
                    perimeterSpawn.bounds.center.y - 
                    perimeterSpawn.bounds.extents.y,
                    perimeterSpawn.bounds.center.y +
                    perimeterSpawn.bounds.extents.y)
            );
            normalTowardsCenterScreen = 
                new Vector2(-1f, 0);
        }
        // ? --- Lato inferiore
        else if(randSide <= 1f)
        {
            result = new Vector2
            (
                Random.Range
                (
                    perimeterSpawn.bounds.center.x -
                    perimeterSpawn.bounds.extents.x,
                    
                    perimeterSpawn.bounds.center.x +
                    perimeterSpawn.bounds.extents.x

                ),
                perimeterSpawn.bounds.center.y -
                perimeterSpawn.bounds.extents.y
            );

            normalTowardsCenterScreen = 
                new Vector2(0f, 1f);
        }
        else
        {
            result = Vector2.zero;
            normalTowardsCenterScreen = Vector2.zero;
            Debug.Log("Error while calculating point" +
                 " on box perimeter");
        }
        
        Debug.Log("Rand: " + result);
        return (result, normalTowardsCenterScreen);
    }

    #region Gameplay Stats
    [Header("Gameplay Stats")]
    List<Target> spawnedTargets = new();
    float passedSpawnTime;
    float chosenSpawnTime;

    public void SetSpawnTime()
    {
        chosenSpawnTime = 
            Random.Range
            (
                MIN_TIME_TO_SPAWN_NEW_TARGET,
                MAX_TIME_TO_SPAWN_NEW_TARGET
            );
    }

    [SerializeField]
    float MIN_TIME_TO_SPAWN_NEW_TARGET;
    [SerializeField]
    float MAX_TIME_TO_SPAWN_NEW_TARGET;

    [SerializeField]
    bool waitDestroyBeforeSpawn;

    void CheckSpawn()
    {
        if(waitDestroyBeforeSpawn &&
            spawnedTargets.Count != 0)
        {
            return;
        }

        if(passedSpawnTime >= chosenSpawnTime)
        {
            SetSpawnTime();

            passedSpawnTime = 0; 
            CreateTarget(); 
        }
    }

    void CheckIfDestroyed()
    {
        List<Target> toRemove = new();
        foreach(Target tar in spawnedTargets)
        {
            if(tar == null)
            {
                toRemove.Add(tar);
            }
            if(tar.hasBeenTakenByPlayer)
            {
                toRemove.Add(tar);
            }
        }

        foreach(Target tarToRem in toRemove)
        {
            spawnedTargets.Remove(tarToRem);
            Destroy(tarToRem.gameObject);       
        }
    }

    float checkDestroyTimer = 0f;
    void CheckIfDestroyedTimed()
    {
        checkDestroyTimer += Time.deltaTime;
        if(checkDestroyTimer <= 1f)
        {
            return;
        }
        else
        {
            checkDestroyTimer = 0f;
        }

        List<Target> toRemove = new();
        foreach(Target tar in spawnedTargets)
        {
            if(tar == null)
            {
                toRemove.Add(tar);
            }
            if(tar.hasBeenTakenByPlayer)
            {
                toRemove.Add(tar);
            }
        }

        foreach(Target tarToRem in toRemove)
        {
            destroyedTargets++;
            GameManager.Get().player1
                .GetComponent<PlayerSPVars>()
                .AddPoints(tarToRem.pointsValue);
            
            onTargetDestroy.Invoke(tarToRem);
            spawnedTargets.Remove(tarToRem);
            if(tarToRem != null)
            Destroy(tarToRem.gameObject, 0.5f);       
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="onePoint">Si muovera' fra dei punti?</param>
    public Target CreateTarget(bool onePoint = true)
    {
        
        if(onePoint)
        {
            var randomizedResult = GetRandomBoxPerimeterPoint();
            
            Target tar = 
                Instantiate(targetPrefab).GetComponent<Target>();

            tar.transform.position = randomizedResult.randPoint;

            float angle = Vector2.SignedAngle
            (
                Vector2.right,
                randomizedResult.normalToCenter
            );

            tar.transform.localRotation = Quaternion.Euler
            (
                0f,
                0f,
                -angle
            );
    
            spawnedTargets.Add(tar);
            return tar;
        }
        else
        {
            Debug.Log("Not made");
        }

        return default;
    }


    int destroyedTargets = 0;
    #endregion

    #region Events

    public UnityEvent<Target> onTargetDestroy = new();

    #endregion

    
    void Awake()
    {
        InitSingleton();
    }


    void Start()
    {
        SetSpawnTime();
        //StartCoroutine(CheckIfDestroyedRoutine());
    }

    void Update()
    {
        passedSpawnTime += Time.deltaTime;
        CheckSpawn();
        CheckIfDestroyedTimed();
    }


    // -------------------------------------------
    // ! Singleton shit
    // -------------------------------------------
    private static TargetManager inst;
 
    public static TargetManager Get()
    {
        if(inst == null)
        {
            Debug.LogError("TargetManager non instanziato!");    
            return null;
        }

        return inst;
    }

    void InitSingleton()
    {
        if(inst != null)
        {
            Debug.LogError("TargetManager gia' instanziato");
        }
        else
        {
            inst = this;
        }
    }
}
