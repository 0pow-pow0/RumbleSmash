using UnityEngine;
using DG.Tweening;

public class ShakeSprite : MonoBehaviour
{
    [SerializeField]
    float shakeDuration;
    [SerializeField]
    float shakeForce;

    private SpriteRenderer spr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();


        spr.gameObject.transform.DOShakePosition
        (
            shakeDuration,
            shakeForce
        ).SetLoops(-1, LoopType.Restart);
    }

}
