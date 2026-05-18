using UnityEngine;
using DG.Tweening;

public class AnimationJumpEnter : MonoBehaviour
{
    [SerializeField]
    float startDelay = 0f;
    [SerializeField]
    float initSizeMultiplier = 5;
    [SerializeField]
    float smallBounceSizeMultiplier = 0.5f;

    [SerializeField]
    float dropSpeed = 0.8f;
    [SerializeField]
    float returnToNormalSizeSpeed = 0.3f;

    Vector2 startScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startScale = transform.localScale;

        // ? --- E' una semplice sprite
        if(startDelay == 0)
        {

            transform.localScale *= initSizeMultiplier;

            transform.DOScale(smallBounceSizeMultiplier, dropSpeed)
            .OnComplete(
                () => transform.DOScale(startScale, returnToNormalSizeSpeed));
        }

        else
        {
            transform.gameObject.SetActive(false);
            PowUtilityU.Get().DelayAction(
            () =>
            {
                transform.gameObject.SetActive(true);
                transform.localScale *= initSizeMultiplier;

                transform.DOScale(smallBounceSizeMultiplier, dropSpeed)
                .OnComplete(
                    () => transform.DOScale(startScale, returnToNormalSizeSpeed)
                );
            },
            startDelay);
        }
    }

    void OnDestroy()
    {
        transform.DOKill();
    }
}
