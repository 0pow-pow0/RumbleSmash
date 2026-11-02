using UnityEngine;

public class TryComposition : MonoBehaviour
{
    FSM<TryComposition> fsm = new();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(fsm);    
        fsm.Update();
    }
}
