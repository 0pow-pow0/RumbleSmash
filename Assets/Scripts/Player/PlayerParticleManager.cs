using System;
using UnityEditor.Build;
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

    Player plr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plr = GetComponentInParent<Player>();
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
                        plr.sprite.bounds.center.x -
                        runStartSprite.bounds.extents.x, 

                        plr.sprite.bounds.center.y -
                        plr.sprite.bounds.extents.y +
                        runStartSprite.bounds.extents.y
                    );
                    
                }
                else
                {
                    runStartSprite.flipX = true;
                    runStartSprite.transform.position = 
                    new Vector2
                    (
                        plr.sprite.bounds.center.x +
                        plr.sprite.bounds.extents.x +
                        runStartSprite.bounds.extents.x,

                        plr.sprite.bounds.center.y - 
                        plr.sprite.bounds.extents.y +
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
                            plr.sprite.bounds.center.x +
                            plr.sprite.bounds.extents.x +
                            runEndSprite.sprite.bounds.extents.x, 

                            plr.sprite.bounds.center.y -
                            plr.sprite.bounds.extents.y +
                            runEndSprite.sprite.bounds.extents.y 
                        );                    
                }
                else
                {
                    runEndSprite.flipX = false;
                    runEndSprite.transform.position = 
                    new Vector2
                    (
                        plr.sprite.bounds.center.x -
                        plr.sprite.bounds.extents.x - 
                        runEndSprite.sprite.bounds.extents.x,
                        
                        plr.sprite.bounds.center.y - 
                        plr.sprite.bounds.extents.y +
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
                    plr.sprite.bounds.center.x,

                    plr.sprite.bounds.center.y -
                    plr.sprite.bounds.extents.y +
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
                    plr.sprite.bounds.center.x,

                    plr.sprite.bounds.center.y -
                    plr.sprite.bounds.extents.y +
                    jumpLandSprite.bounds.extents.y / 2
                );
                playerEffects.SetTrigger("JumpLandTrigger");
            }
        );

        plr.pj.onDoppioScattoStarted.AddListener(
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
    }


}
