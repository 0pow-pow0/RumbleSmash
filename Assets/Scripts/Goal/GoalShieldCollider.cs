using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Gestisce unicamente la collisione, ll
/// </summary>
public class GoalShieldCollider : MonoBehaviour
{
    [Header("References"), SerializeField]
    Goal goal;

    [SerializeField]
    private BoxCollider2D coll;
    
    void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    public void SetCollider(bool isActive)
    {
        coll.enabled = isActive;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        Debug.Log(other.collider.name);
        if(other.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            goal.RemoveShieldHP(GameManager.Get().ball.damage);
        }
    }
}
