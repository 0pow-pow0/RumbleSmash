using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIQuests : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI questText;

    [SerializeField]
    Image backgroundQuest;

    public void Awake()
    {
        questText.gameObject.SetActive(false);
        questText.text = "";
        backgroundQuest.gameObject.SetActive(false);
    } 

    public void StartQuest(string qstTex)
    {
        questText.gameObject.SetActive(true);
        questText.text = qstTex;
        backgroundQuest.gameObject.SetActive(true);
        
        questText.transform.localScale *= 5;
        backgroundQuest.transform.localScale *= 5;

        questText.transform.DOScale(0.5f, 0.8f)
        .OnComplete(
            () => questText.transform.DOScale(1f, 0.3f)
        );
        backgroundQuest.transform.DOScale(0.5f, 0.8f)
        .OnComplete(
            () => backgroundQuest.transform.DOScale(1f, 0.3f)
        );
    }

    public void UpdateQuestText(string newText)
    {
        questText.text = newText;
    }

    public void EndQuest()
    {
        questText.gameObject.SetActive(false);
        questText.text = "";
        backgroundQuest.gameObject.SetActive(false);
    }

}
