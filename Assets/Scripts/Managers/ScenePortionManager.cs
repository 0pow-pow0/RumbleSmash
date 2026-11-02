using UnityEngine;
using EditorAttributes;

/// <summary>
/// Gestisce la logica di WRAPPER di oggetti, in modo che
/// possa gestire piu' logiche di grandi sezione di codice.
/// 
/// Ad esempio, questo script gestira' una sezione di 
/// in cui si assegnano i controller dei player, una volta
/// assegnati si passera' al gameplay vero e proprio, nella
/// stessa scena.
/// </summary>
public class ScenePortionManager : MonoBehaviour
{
    /// <summary>
    /// Ordinato in base all'ordine di attivazione
    /// </summary>
    ScenePortion[] scenePortions;
    ScenePortion activePortion;

    void Awake()
    {
        if(scenePortions.Length == 0)
        {
            Debug.LogError("No ScenePortions added," +
            " deactivating myself");
            gameObject.SetActive(false);
            return;
        }

        activePortion = scenePortions[0];
        activePortion.OnPortionStart(); 
    }

    void Update()
    {
        activePortion.UpdatePortion();  
    }
    
    void SwitchPortion(ScenePortion newScene)
    {
        activePortion.OnPortionEnd();
        activePortion = newScene;
        activePortion.OnPortionStart();
    }
}

