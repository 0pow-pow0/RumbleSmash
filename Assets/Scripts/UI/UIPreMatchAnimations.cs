using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class UIPreMatchAnimations : MonoBehaviour
{
    [Header("References"), SerializeField]
    GameObject father;

    void SetInitialState()
    {
        father.SetActive(true);
                
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MatchManager.Get().onPreMatchShowRivals.AddListener(
            (float dur) =>
            {
                SetInitialState();


                PowUtilityU.Get().DelayAction(
                    () =>
                    {
                        father.SetActive(false);
                    },
                    dur);
            }
            );
    }

}
