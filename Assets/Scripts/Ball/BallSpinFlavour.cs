using UnityEngine;

/// <summary>
/// Permette alla palla di ruotare attorno ad un asse per simulare
/// la rotazione di un pallone quando lo si colpisce.
/// </summary>
public class BallSpinFlavour : MonoBehaviour
{
    [Header("References"), SerializeField]
    GameObject mesh;

    [Header("Squash and Strech Variables"), SerializeField]
    AnimationCurve squashAndStrechBehaviour;

    Ball bl;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bl = GetComponentInParent<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
