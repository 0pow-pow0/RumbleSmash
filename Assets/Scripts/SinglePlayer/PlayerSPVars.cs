using UnityEngine;

public class PlayerSPVars : MonoBehaviour
{
    [field: SerializeField, EditorAttributes.ReadOnly]
    public int scoredPoints { get; private set; }
    public void AddPoints(int toAdd)
    {
        if(toAdd < 0)
        {
            Debug.LogError("Variabile toAdd negativa!");
            return;
        }

        scoredPoints += toAdd;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
