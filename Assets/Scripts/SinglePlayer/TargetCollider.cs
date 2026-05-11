using DG.Tweening;
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

            
            tar.transform.DOShakePosition(0.8f, 0.05f);
            tar.transform.DOScale(1.1f, 0.15f).
            OnComplete
            (
                () => 
                {
                    tar.transform.DOScale(1f, 0.15f);
                }
            );
        }
    }
}
