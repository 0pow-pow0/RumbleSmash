using DG.Tweening;
using UnityEngine;

public class AnimationEndlessRotate : MonoBehaviour
{

    [SerializeField]
    float rotationSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.DORotate(new Vector3(0f, 0f, 360f), rotationSpeed, RotateMode.FastBeyond360)
        .SetLoops(-1, LoopType.Restart)
        .SetEase(Ease.Linear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        transform.DOKill(); 
    }
}
