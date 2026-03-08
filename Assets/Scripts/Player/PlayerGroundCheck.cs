using UnityEngine;

/// <summary>
/// Simply Sets Player.isOnGround to either true or false
/// </summary>
public class PlayerGroundCheck : MonoBehaviour
{   
    Player plr; 
    void Awake()
    {
        plr = GetComponentInParent<Player>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("LevelCollider")
            &&
            // ? --- Resetta solo non stiamo saltando
            plr.rb.linearVelocity.y <= 0)
        {
            plr.isOnGround = true;
            plr.GetComponent<PlayerJump>().ResetJumpConditions();
        }
        else
        {
            plr.isOnGround = false;
        }
    }

}
