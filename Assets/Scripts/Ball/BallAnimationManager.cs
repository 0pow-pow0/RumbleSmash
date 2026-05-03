using UnityEngine;

public class BallAnimationManager : MonoBehaviour
{
    Ball bl;
    [Header("References"), SerializeField]
    ParticleSystem scoreAnimation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.SetParent(GlobalParticleManagerPow.Get().transform);

        bl = GameManager.Get().ball;

        bl.onBallScore.AddListener(
            () =>
            {
                Debug.Log("Particle start"); 
                scoreAnimation.gameObject.transform.position =
                    bl.transform.position;
                scoreAnimation.Play();
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
