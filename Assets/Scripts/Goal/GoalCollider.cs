using UnityEngine;
using UtilityShit;

public class GoalCollider : MonoBehaviour
{
    [SerializeField]
    Goal goal;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PowUtility.Log("GOAL!!!!", Color.yellow);
        goal.Score(); 
    }
}
