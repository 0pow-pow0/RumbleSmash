using UnityEngine;
using EditorAttributes;
using System;

/// <summary>
/// Contiene tanti gameObject di una scena insieme.
/// In questo modo posso manipolare tanti gameObject 
/// da un singolo posto, in questo modo posso transitare
/// da uno STATO del GIOCO in cui solo una PORZIONE
/// di una scena deve avere GameObject attiva 
/// ed un'altra porzione in cui sono disattivati.
/// </summary>
[Serializable]
public class ScenePortion : MonoBehaviour 
{

    [field: SerializeField]
    public string portionName { get; private set; }
    
    /// <summary>
    /// Flag per decidere se disattivare i gameObject
    /// o solo la logica.
    /// 
    /// Utile se vogliamo mostrare oggetti a schermo,
    /// ma privarli della logica.
    /// </summary>
    [field: SerializeField]
    public bool mustDeactivateObjects { get; private set; }

    /// <summary>
    /// Oggetti contenuti nella porzione.
    /// </summary>
    [SerializeField]
    GameObject[] objs;

    [Header("Debug"), 
    SerializeField, EditorAttributes.ReadOnly]
    bool isActive;


    public virtual void OnPortionStart()
    {
        if(mustDeactivateObjects)
        {
            
        }
    }

    public virtual void UpdatePortion()
    {
        
    }

    public virtual void OnPortionEnd()
    {
        
    }
}