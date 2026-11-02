using Unity.VisualScripting;
using UnityEngine;
using UtilityShit;

public class GoalCollider : MonoBehaviour
{
    [SerializeField]
    Goal goal;

    [SerializeField]
    private BoxCollider2D coll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Debug.Log("GoalCollider.cs: Init completed");
        coll = GetComponent<BoxCollider2D>();
        Debug.Log("GoalCollider.cs: " + GetComponent<BoxCollider2D>());
        Debug.Log("GoalCollider.cs: " + coll);
    }

    
    public void SetCollider(bool isActive)
    {
        Debug.Log("GoalCollider.cs: " + coll);
        coll.enabled = isActive;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Cringe xD
        if(collision.gameObject.name != "GoalCollider")
            return;

        PowUtility.Log("GOAL!!!!", Color.yellow);

        goal.Score(); 
    }
}
