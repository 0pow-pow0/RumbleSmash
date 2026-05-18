using Unity.VisualScripting;
using UnityEngine;
using System;
using DG.Tweening;


public class Target : MonoBehaviour
{
    public bool hasBeenTakenByPlayer;

    [SerializeField]
    SpriteRenderer sprite;
    [SerializeField]
    SpriteRenderer spriteOutline;

    [field: SerializeField]
    public int pointsValue { get; private set; }

    [field: NonSerialized]
    public Vector2[] movementPoints { get; private set; }
    int activePoint;


    [field: SerializeField]
    public float MAX_LIFE_TIME { get; private set; }
    [field: SerializeField]
    public float MIN_LIFE_TIME { get; private set; }

    public void SetActivePoints(Vector2[] newPoints)
    {
        movementPoints = new Vector2[newPoints.Length];
        activePoint = 0; 
    }

    void Start()
    {
        float chosenLifeTime;
        chosenLifeTime = 
            UnityEngine.
            Random.Range(MIN_LIFE_TIME, MAX_LIFE_TIME);

        sprite.DOFade(0f, chosenLifeTime);
        spriteOutline.DOFade(0f, chosenLifeTime);

        PowUtilityU.Get().DelayAction
        (
            () =>
            {
                Destroy(gameObject);
                Debug.Log("Eliminato");
            },
            chosenLifeTime
        );
    }

    void OnDestroy()
    {
        sprite.DOKill();
        spriteOutline.DOKill();     
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
