using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.InputSystem;

public class InputTest : MonoBehaviour
{
    [SerializeField]
    PlayerInput plrInp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("TestInput: " + InputSystem.actions
            .FindAction("Move").ReadValue<Vector2>());

        Debug.Log(plrInp.actions["Move"].ReadValue<Vector2>());
    }
}
