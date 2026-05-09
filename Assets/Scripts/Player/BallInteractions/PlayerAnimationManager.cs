using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    [SerializeField]
    Player plr;


    // ? --- Preso dalla mesh stesa
    [SerializeField]
    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        plr.onPlayerMoveStart.AddListener(
            () =>
            {
                anim.ResetTrigger("OnLand");
                anim.SetBool("IsRunning", true);
            }
        );
        plr.onPlayerMoveEnd.AddListener(
            () =>
            {
                anim.ResetTrigger("OnLand");
                anim.SetBool("IsRunning", false);
            }
        );

        // -------------------------------------------
        // ! In Air Animations
        // -------------------------------------------
        plr.pj.onFirstJumpPerformed.AddListener(
            () =>
            {
                anim.ResetTrigger("OnLand");
                anim.SetBool("IsJumping", true);
            }
        );

        plr.pj.onLand.AddListener(
            () =>
            {
                anim.SetBool("IsJumping", false);
                anim.SetBool("IsFalling", false);
                anim.SetTrigger("OnLand");
            }
        ); 
        
        plr.pj.onDoppioScattoStart.AddListener(
            () =>
            {
                anim.SetBool("IsJumping", false);
                anim.SetBool("IsFalling", false);
                anim.SetBool("IsDashing", true);
            }
        );

        plr.pj.onDoppioScattoEnd.AddListener(
            () =>
            {
                anim.SetBool("IsDashing", false);
            }
        );

        plr.pj.onFallStart.AddListener(
            () =>
            {
                // ? --- Necessario perche' potrebbe rimanere
                // ? --- attivo
                anim.ResetTrigger("OnLand");
                anim.SetBool("IsFalling", true);
                anim.SetBool("IsJumping", false);
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
    }
}
