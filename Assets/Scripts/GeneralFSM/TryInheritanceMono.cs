using UnityEngine;

public class TryInheritanceMono : MonoBehaviour
{
    TryInheritance try1 = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        try1.Update();
    }
}
