    using UnityEngine;
using UnityEngine.InputSystem;

public class Ball : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D rb;

    [Header("Deubg Input")]
    [SerializeField] InputAction resetBallSpeed;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(resetBallSpeed.WasPressedThisFrame())
        {
            Debug.Log("Velocity Reset!");
        }
    }
}
