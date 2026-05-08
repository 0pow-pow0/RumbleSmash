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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("LevelCollider"))
        {
            plr.isOnGround = true;  
            // ? --- Altrimenti se dovesse mai capitare di
            // ? --- sfiorare con il collider una piattaforma
            // ? --- mentre si cade si trigghererebbe,
            // ? --- anche se stiamo cadendo.
            if(plr.rb.linearVelocityY <= 0) 
            {
                Debug.Log("OnLand");
                plr.pj.onLand.Invoke();
            }
        }    
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("LevelCollider"))
        {
            plr.isOnGround = false; 
        }        
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
