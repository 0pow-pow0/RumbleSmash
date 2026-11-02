using UnityEngine;
using UnityEngine.InputSystem;

public class HotReloader : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.iKey.wasPressedThisFrame)
        {
            Ball bl = GameManager.Get().ball;
            bl.transform.position = new Vector3(-0.899999976f,-0.219999999f,-0.0430799052f);
        }
        if(Keyboard.current.oKey.wasPressedThisFrame)
        {
            Ball bl = GameManager.Get().ball;
            bl.rb.linearVelocity = Vector3.zero;
        }
    }
}
