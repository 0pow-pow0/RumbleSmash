using Mono.Cecil;
using UnityEngine;

public class TargetCollider : MonoBehaviour
{
    [SerializeField]
    Target tar; 

    [SerializeField]
    BoxCollider2D coll;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer 
            == LayerMask.NameToLayer("Ball"))
        {
            Debug.Log("Collided with ball");
            coll.enabled = false;
            tar.hasBeenTakenByPlayer = true;
        }
    }
}
