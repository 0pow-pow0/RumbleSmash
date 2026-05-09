using UnityEngine;
using UnityEngine.InputSystem;

public class TestShit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if(Keyboard.current.aKey.wasPressedThisFrame)
        {
            rb.AddForce(new Vector2(0.5f, 0.5f) * 10, ForceMode.Impulse);
        }
    }
}
