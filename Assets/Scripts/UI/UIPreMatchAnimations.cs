using UnityEngine;

public class UIPreMatchAnimations : MonoBehaviour
{
    [Header("References"), SerializeField]
    GameObject father;

    void SetInitialState()
    {
        father.SetActive(false);
                
    }

    void Awake()
    {
        SetInitialState();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   

        MatchManager.Get().onPreMatchShowRivals.AddListener(
            (float dur) =>
            {
                father.SetActive(true);


                PowUtilityU.Get().DelayAction(
                    () =>
                    {
                        SetInitialState();
                    },
                    dur);
            }
            );
    }

}
