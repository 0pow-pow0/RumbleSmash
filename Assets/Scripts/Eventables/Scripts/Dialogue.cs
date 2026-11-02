using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public struct Sentence
{
    public string content;
    public int framesBetweenChars;    
}

[CreateAssetMenu(
fileName = "Dialogue", 
menuName = "ScriptableObject/Eventables/Dialogue")]

public class Dialogue : Eventable
{
    [SerializeField]
    public string speakerName;
    [SerializeField]
    public Sprite speakerIcon;

    [field: SerializeField, EditorAttributes.ReadOnly]
    public int MAX_CHAR_COUNTER { get; private set; } = 160; 

    [SerializeField]
    private Sentence[] sentences;
    int sentenceIndex = 0;
    
    [SerializeField]
    GameObject dialoguePrefab;
    GameObject dialogueObject;
    TextMeshProUGUI contentText;
    TextMeshProUGUI speakerNameText;
    
    // ? --- Dobbiamo stopparla ogni volta che skippiamo dialogo
    // ? --- altrimenti se il player skippa mentre un dialogo deve ancora finire
    // ? --- si renderizzano dei caratteri di prima
    Coroutine showCharCoroutine;

    public override void OnStart()
    {
        Debug.Log("Starting dialog with: " + speakerName);
        dialogueObject = Instantiate(dialoguePrefab);

        dialogueObject.transform.position = Vector3.zero;


        speakerNameText = 
        dialogueObject.transform
        .Find("DialogueBox/DialogArea/SpeakerNameBox/TextSpeakerName")
        .GetComponent<TextMeshProUGUI>();

        speakerNameText.text = speakerName;    


        contentText = 
        dialogueObject.transform
        .Find("DialogueBox/DialogArea/TextContent").GetComponent<TextMeshProUGUI>();


        Image img = dialogueObject.transform.Find("SpeakerIconBox/SpeakerIcon").GetComponent<Image>();
        img.sprite = speakerIcon;
 
        
        
        dialogueObject.transform
        .SetParent(GameObject.Find("= Canvas/DialoguesContainer/DialogPivot").transform, false);
    

        // ? --- Error handling easy
        foreach(Sentence stn in sentences)
        {
            if(stn.content.Length > MAX_CHAR_COUNTER)
            {
                Debug.LogWarning("One of the sentences of this dialogue is" +
                " above the max character counter, overflow expected!");
            }
        }

        dialogueInputThresholdTimer = 0f;
        sentenceIndex = 0;
        ShowTextByChar();

        SingleplayerInputManager.Get().DeactivatePlayerMap();
    }

    public override void OnEnd()
    {
        SingleplayerInputManager.Get().ActivatePlayerMap();
        Destroy(dialogueObject.gameObject);
    }

    // ? --- Evita che si insta clicchi appena si apre
    float dialogueInputThreshold = 0.5f;
    float dialogueInputThresholdTimer = 0;
    public override void Update()
    {
        //Debug.Log("Sara' pi sempre si: " + dialogueInputThresholdTimer);
        if(dialogueInputThresholdTimer > dialogueInputThreshold && 
            InputSystem.actions.FindAction("Submit").WasCompletedThisFrame())
        {
            NextSentence();
            Debug.Log("Skipping to next dialogue");
        }
        else
        {
            dialogueInputThresholdTimer += Time.deltaTime;
        }
    }

    private void NextSentence()
    {
        Debug.Log("Prossima frase!");
        if(sentenceIndex + 1 >= sentences.Length)
        {
            state = EventableState.QUIT;
            Debug.Log("Sentenza finita!");
            return;
        }

        sentenceIndex++;
        contentText.text = "";
        speakerNameText.text = speakerName;
        ShowTextByChar();
    }

    private void ShowTextByChar()
    {
        if(showCharCoroutine != null)
            PowUtilityU.Get().StopCoroutine(showCharCoroutine);
    
        showCharCoroutine = PowUtilityU.Get().StartCoroutine(ShowTextCharByCharRoutine());
    }
    private IEnumerator ShowTextCharByCharRoutine()
    {
        int shownChars = 0;
        Debug.Log("To show: " + sentences[sentenceIndex].content);
        int waitedFrames = 0;
        while(shownChars < sentences[sentenceIndex].content.Length)
        {
            contentText.text += sentences[sentenceIndex].content[shownChars]; 
            shownChars++;
            while(waitedFrames < sentences[sentenceIndex].framesBetweenChars)
            {
                yield return null;
                waitedFrames++; 
            }
            waitedFrames = 0; 
        }
        Debug.Log("Exiting");
    }
}
