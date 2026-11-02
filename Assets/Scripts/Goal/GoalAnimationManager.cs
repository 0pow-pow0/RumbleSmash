using UnityEngine;

public class GoalAnimationManager : MonoBehaviour
{
    [SerializeField]
    Goal goal;

    [SerializeField]
    ParticleSystem onShieldDestroyEffect;

    void Start()
    {
        goal.onShieldDestroy.AddListener(
            () =>
            {
                onShieldDestroyEffect.Play();
            }
        );
    }

}
