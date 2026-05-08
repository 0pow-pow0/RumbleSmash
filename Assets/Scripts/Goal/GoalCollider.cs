using UnityEngine;
using UtilityShit;

public class GoalCollider : MonoBehaviour
{
    [SerializeField]
    Goal goal;

    public BoxCollider2D coll { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    
    public void SetCollider(bool isActive)
    {
        coll.enabled = isActive;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        PowUtility.Log("GOAL!!!!", Color.yellow);
        goal.Score(); 
    }
}
