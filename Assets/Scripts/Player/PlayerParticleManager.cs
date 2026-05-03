using UnityEngine;

public class PlayerParticleManager : MonoBehaviour
{
    [Header("References"), SerializeField]
    ParticleSystem ballHit;
    [SerializeField]
    Animator runAnimation;
    [SerializeField]
    SpriteRenderer runStartSprite;
    [SerializeField]
    SpriteRenderer runEndSprite;
    Player plr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plr = GetComponentInParent<Player>();
        transform.SetParent(GlobalParticleManagerPow.Get().transform);

        plr.pbi.onBallHit.AddListener(
            (Vector2 collPos) =>
            {
                ballHit.gameObject.transform.position = collPos;
                ballHit.Play();
            }
        );

        plr.onPlayerMoveStart.AddListener
        (
            () =>
            {

                if(!plr.isOnGround)
                    return;
                
                runAnimation.SetTrigger("RunStartTrigger");

                if(plr.isFacingRight)
                {
                    runStartSprite.flipX = false;

                    Debug.Log("palle");
                    Debug.Log(runStartSprite.sprite);
                    runStartSprite  .transform.position = 
                        new Vector2
                        (
                            plr.sprite.bounds.center.x -
                            plr.sprite.bounds.extents.x -
                            runStartSprite.sprite.bounds.extents.x, 

                            plr.sprite.bounds.center.y -
                                plr.sprite.bounds.extents.y +
                            runStartSprite.sprite.bounds.extents.y 
                        );
                    
                }
                else
                {
                    runStartSprite.flipX = true;
                    runAnimation.transform.position = 
                    new Vector2
                    (
                        plr.sprite.bounds.center.x +
                        runStartSprite.bounds.extents.x,
                        
                        plr.sprite.bounds.center.y - 
                        plr.sprite.bounds.extents.y +
                        runStartSprite.bounds.extents.y
                    );
                
                }

            }
        );


        plr.onPlayerMoveEnd.AddListener(
            () =>
            {
                if(!plr.isOnGround)
                    return;

                
                runAnimation.SetTrigger("RunEndTrigger");

                if(plr.isFacingRight)
                {
                    runEndSprite.flipX = true;

                    Debug.Log("palle");
                    Debug.Log(runEndSprite.sprite);
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
                        plr.sprite.bounds.center.x +
                        runEndSprite.bounds.extents.x,
                        
                        plr.sprite.bounds.center.y - 
                        plr.sprite.bounds.extents.y +
                        runEndSprite.bounds.extents.y
                    );
                
                }
            }
        );
    }


}
