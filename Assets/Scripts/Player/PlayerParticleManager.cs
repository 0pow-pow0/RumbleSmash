using DG.Tweening;
using UnityEngine;

public class PlayerParticleManager : MonoBehaviour
{
    [Header("References"), SerializeField]
    ParticleSystem ballHit;
    [SerializeField]
    Animator playerEffects;

    [SerializeField, Space(15)]
    SpriteRenderer runStartSprite;
    [SerializeField]
    SpriteRenderer runEndSprite;

    [SerializeField, Space(15)]
    SpriteRenderer jumpStartSprite;
    [SerializeField]
    SpriteRenderer jumpLandSprite;

    [field: Space(15), SerializeField] 
    public GameObject dashTrailsFather { get; private set; }
    [field: SerializeField] 
    public TrailRenderer[] dashTrails { get; private set; }

    [field: Space(15), SerializeField] 
    public ParticleSystem oldchargeStart;
    [SerializeField]
    public GameObject chargeStart;
    [SerializeField]
    public SpriteRenderer chargeStartSprite;

    [field: SerializeField]
    public ParticleSystem chargeEnd { get; private set; }



    Player plr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plr = GetComponentInParent<Player>();

        // ? --- Praticamente e' per evitare il problema di quando voglio 
        // ? --- posizionare dei particellare in un punto,
        // ? --- pero' se fosser child del player si muoverebbero con lui
        // ? --- Non so se sia la roba migliore xDDDDDDDDDDD

        transform.SetParent(GlobalParticleManagerPow.Get().transform);
        transform.position = Vector3.zero;
        
        plr.pbi.onBallHit.AddListener(
            (Vector2 collPos) =>
            {
                ballHit.gameObject.transform.position = collPos;
                ballHit.Play();
            }
        );

        // -------------------------------------------
        // ! Movement
        // -------------------------------------------
        plr.onPlayerMoveStart.AddListener
        (
            () =>
            {
                if(!plr.isOnGround)
                    return;
                

                if(plr.isFacingRight)
                {
                    runStartSprite.flipX = false;

                    runStartSprite.transform.position = 
                    new Vector2
                    (
                        plr.bodyColl.bounds.center.x -
                        runStartSprite.bounds.extents.x, 

                        plr.bodyColl.bounds.center.y -
                        plr.bodyColl.bounds.extents.y +
                        runStartSprite.bounds.extents.y
                    );
                    
                }
                else
                {
                    runStartSprite.flipX = true;
                    runStartSprite.transform.position = 
                    new Vector2
                    (
                        plr.bodyColl.bounds.center.x +
                        plr.bodyColl.bounds.extents.x +
                        runStartSprite.bounds.extents.x,

                        plr.bodyColl.bounds.center.y - 
                        plr.bodyColl.bounds.extents.y +
                        runStartSprite.bounds.extents.y
                    );
                
                }

                playerEffects.SetTrigger("RunStartTrigger");
            }
        );


        plr.onPlayerMoveEnd.AddListener(
            () =>
            {
                if(!plr.isOnGround)
                    return;

                

                if(plr.isFacingRight)
                {
                    runEndSprite.flipX = true;

                    runEndSprite.transform.position = 
                        new Vector2
                        (
                            plr.bodyColl.bounds.center.x +
                            plr.bodyColl.bounds.extents.x +
                            runEndSprite.sprite.bounds.extents.x, 

                            plr.bodyColl.bounds.center.y -
                            plr.bodyColl.bounds.extents.y +
                            runEndSprite.sprite.bounds.extents.y 
                        );                    
                }
                else
                {
                    runEndSprite.flipX = false;
                    runEndSprite.transform.position = 
                    new Vector2
                    (
                        plr.bodyColl.bounds.center.x -
                        plr.bodyColl.bounds.extents.x - 
                        runEndSprite.sprite.bounds.extents.x,
                        
                        plr.bodyColl.bounds.center.y - 
                        plr.bodyColl.bounds.extents.y +
                        runEndSprite.sprite.bounds.extents.y
                    );
                
                }          
                playerEffects.SetTrigger("RunEndTrigger");
            }
        );

        plr.pj.onFirstJumpPerformed.AddListener(
            () =>
            {
                
                jumpStartSprite.transform.position = new Vector2(
                    plr.bodyColl.bounds.center.x,

                    plr.bodyColl.bounds.center.y -
                    plr.bodyColl.bounds.extents.y +
                    jumpStartSprite.bounds.extents.y
                    
                );
                

                playerEffects.SetTrigger("JumpStartTrigger");
            }
        );

        plr.pj.onLand.AddListener(
            () =>
            {
                jumpLandSprite.transform.position = new Vector2
                (
                    plr.bodyColl.bounds.center.x,

                    plr.bodyColl.bounds.center.y -
                    plr.bodyColl.bounds.extents.y +
                    jumpLandSprite.bounds.extents.y / 2
                );
                playerEffects.SetTrigger("JumpLandTrigger");
            }
        );

        plr.pj.onDoppioScattoStart.AddListener(
            () =>
            {
                dashTrailsFather.transform.position = 
                    plr.transform.position;
 
                float newAngle = 
                    Vector2.SignedAngle(
                        Vector2.right,
                        plr.pj.doppioScattoDirection
                    );
                dashTrailsFather.transform.localRotation =
                    Quaternion.Euler(0, 0, newAngle);
  
                // ? --- Altrimenti il cambio di posizione fatto prima gia'
                // ? --- creerebbe trails
                foreach(TrailRenderer tr in dashTrails)
                {
                    tr.Clear();
                }

                foreach(TrailRenderer tr in dashTrails)
                {
                    tr.emitting = true;
                }

                PowUtilityU.Get().RepeatActionForFrame(
                    () =>
                    {
                        dashTrailsFather.transform.position =
                            plr.transform.position;                       
                    },
                    plr.pj.DOPPIOSCATTO_FRAME_DURATION
                );
            }
        );

        plr.pj.onDoppioScattoEnd.AddListener(
            () =>
            {
                foreach(TrailRenderer tr in dashTrails)
                {
                    tr.emitting = false;
                    
                }
            }
        );
    
    
        plr.pbi.onChargeStart.AddListener(
            () =>
            {
                // oldchargeStart.gameObject.transform.position =
                //     new Vector3(
                //         plr.coll.bounds.center.x + 
                //         plr.coll.bounds.extents.x,
                        
                //         plr.coll.bounds.center.y +
                //         plr.coll.bounds.extents.y,
                //         0f
                //     );

                // oldchargeStart.Play();
                
                chargeStart.gameObject.SetActive(true);
                chargeStart.transform.localRotation = Quaternion.identity;
                chargeStartSprite.color = Color.black;

                // ? --- Un po' troppe coroutine

                chargeStart.transform.SetParent(plr.transform, false);

                chargeStart.gameObject.transform
                .DOScale(new Vector3(1.2f, 1.2f, 1f), 0.2f)
                .OnComplete(
                    () =>
                    {
                        chargeStart.gameObject.transform
                        .DOScale(
                            new Vector3(0.7f, 0.7f, 1f), 
                            0.1f
                        )
                        .OnComplete(
                            () =>
                            {
                                chargeStartSprite.color =
                                    new Color(
                                        chargeStartSprite.color.r,
                                        chargeStartSprite.color.g,
                                        chargeStartSprite.color.b,
                                        0.3f
                                    );

                                chargeStartSprite
                                .DOFade
                                (
                                    1f, 
                                    plr.pbi.TIME_TO_REACH_MAX_CHARGE - 0.2f
                                );
                            }
                        );  

                        // chargeStart.gameObject.transform
                        // .DORotate
                        // (
                        //     new Vector3(0f, 0f, 360f),
                        //     // ? --- 0.2 perche' lo scale provoca questo delay
                        //     plr.pbi.TIME_TO_REACH_MAX_CHARGE - 0.2f, 
                        //     RotateMode.Fast
                        // )
                        // .SetLoops(-1, LoopType.Incremental);
                        
                    });
                


            }
        );

        plr.pbi.onChargeEnd.AddListener(
            () =>
            {
                Debug.Log("Ended");
                chargeStart.transform.DOKill();
                
                // chargeStart.transform.
                // DOShakePosition
                // (
                //     0.3f,
                //     new Vector3(0.1f, 0.1f, 0f),
                //     1  
                // )
                // .SetLoops(-1, LoopType.Restart);

                chargeStart.gameObject.transform
                .DOScale
                (
                    new Vector3(1.4f, 1.4f, 1f),
                    0.1f
                ).OnComplete(
                    () =>
                    {
                        chargeStart.gameObject.transform
                        .DOScale
                        (
                            new Vector3(1.2f, 1.2f, 1f),
                            0.1f
                        ).SetLoops(-1, LoopType.Yoyo);
                    }
                );

            }
        );

        plr.pbi.onKickEnd.AddListener(
            () =>
            {
                // ? --- La funzione onKickEnd viene chiamata sia a fine
                // ? --- charge, se stiamo caricando,
                // ? --- sia a fine kick, anche se non stiamo chargando.
                // ? --- In ogni caso, dunque, io resetto le risorse del charge.
                chargeStart.transform.DOKill();
                chargeStartSprite.DOKill();
                chargeStart.SetActive(false); 
                chargeStart.transform.SetParent(playerEffects.transform, false);
            }
        );
    }


}
