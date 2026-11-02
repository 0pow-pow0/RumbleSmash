using UnityEngine;

public class BoxCollider2DDRawWireframe : MonoBehaviour
{
    [SerializeField]
    bool debugMode = false;
    BoxCollider2D boxCollider2D;

    void Start()
    {
        BoxCollider2D bx2D = GetComponent<BoxCollider2D>();

        if(bx2D == null)
        {
            if(debugMode)
                Debug.LogError("BoxCollider2D not found in the GameObject!");
            this.enabled = false;
        }
        else
        {
            boxCollider2D = bx2D;
        }

    }

    void Update()
    {
        
    }
}
