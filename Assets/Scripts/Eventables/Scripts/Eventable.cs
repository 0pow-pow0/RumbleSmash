using System.ComponentModel;
using UnityEngine;

public enum EventableState
{
    PENDING, // ? --- In coda per essere iniziata
    START, // ? --- Appena iniziato
    UPDATE, // ? --- In corso
    QUIT, // ? --- Sta venendo ultimata
    ENDED // ? --- Finita
}
[CreateAssetMenu(fileName = "Eventable", menuName = "ScriptableObject/Eventable")]

public abstract class Eventable : ScriptableObject
{
    [EditorAttributes.ReadOnly]
    public EventableState state;

    public abstract void OnStart();
    public abstract void OnEnd();
    public abstract void Update(); 
}