using System.ComponentModel;
using EditorAttributes;
using UnityEngine;
using UtilityShit;

// ? --- Praticamente una fsm, ma gli stati vengono messi dall'inspector
// ? --- Inoltre, piuttosto che cambiare casualmente fra di loro, vengono
// ? --- eseguiti come una Queue.
public class Sequencer : MonoBehaviour
{
    // ? --- Verra' eseguito un eventable alla volta
    [SerializeField]    
    Eventable[] events; 

    [field: SerializeField, EditorAttributes.ReadOnly]
    public Eventable activeEvent { get; private set; } 

    [field: SerializeField, EditorAttributes.ReadOnly]
    public int activeEventIndex { get; private set; } 


    void Start()
    {
        SingleplayerInputManager.Get().onPlayer1Joined.AddListener
        (
            () =>
            {
                if(events.Length > 0)
                {
                    activeEvent = events[0];
                    activeEvent.OnStart();
                    activeEvent.state = EventableState.START;
                }    

                
            }
        );
    }

    // Update is called once per frame
    void Update()
    {
        if(activeEvent == null)
            return;

        if(activeEvent.state == EventableState.QUIT)
        {
            PowUtility.Log("Next eventable", Color.magenta);
            NextEventable();
            return;
        }
        activeEvent.Update();
    }

    void NextEventable()
    {
        activeEvent.OnEnd();
        // viene triggherato dallo state stesso
        //activeEvent.state = EventableState.QUIT;

        
        // ? --- Per bypassare un eventuale reset della variabile activeState
        // ? --- PRIMA che venga eseguita la DelayAction
        Eventable oldEvnt = activeEvent;
        // ? --- Ritarda di un frame cosi' se qualcuno sta checkando 
        // ? --- lo state ha il tempo di reagire.
        PowUtilityU.Get()
        .DelayAction(() => oldEvnt.state = EventableState.ENDED, 0f);

        if(activeEventIndex + 1 >= events.Length)
        {
            activeEvent = null;
            return;
        }

        activeEventIndex++;
        activeEvent = events[activeEventIndex];
        activeEvent.state = EventableState.START;
        activeEvent.OnStart();

        // ? --- Per bypassare un eventuale reset della variabile activeState
        // ? --- PRIMA che venga eseguita la DelayAction
        Eventable newEvnt = activeEvent;
        
        // ? --- Stesso ragionamento di sopra
        PowUtilityU.Get()
        .DelayAction(() => newEvnt.state = EventableState.UPDATE, 0f);
    }
}
