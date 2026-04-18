    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// Finite state machine per gestire gli stati di qualunque entita'
    /// E' necessario solo dare una reference del GameObject da cui si vogliono
    /// ricevere informazioni utili per processare azioni nei codici dei singoli stati.
    /// 
    /// Modificata ad hoc per questo scenario
    /// </summary>
    public class BallInteractionsFSM
    {
        public BaseState _currentState { get; private set; }
        public PlayerBallInteractions pbi;
        
        #region STATI CONCRETI
        const int NUMBER_OF_STATES = 0;
        // ! Inserire qui gli stati figli di BaseStates
        //RunState runState;
        public KickState kickState { get; private set; }
        public ChargingState chargingState { get; private set; }
        public AwaitState awaitState { get; private set; }

        #endregion

        public BallInteractionsFSM()
        {

            // ! Inserire qui le inizializzazioni degli stati concreti
            kickState = new KickState();
            chargingState = new ChargingState();
            awaitState = new AwaitState();

            _currentState = awaitState;
            _currentState.StateEnter(this);
        }

        public void Update()
        {
            //if (GameManager.isGamePaused) { return; }



            //Debug.Log("state: " + _currentState);
            _currentState.StateUpdate(this);
        }

        /// <summary>
        /// Lo switch state sara' possibile solo ed esclusivamente se il nuovo stato ritorna
        /// un valore true durante l'esecuzione dello StateEnter, se e' false semplicemente
        /// non si triggera ne' lo StateExit del vecchio stato ed e' come se non avessimo fatto alcuno switch.
        /// </summary>
        public bool SwitchState(BaseState newState)
        {
            if(newState.CanEnterState(this))
            {
                _currentState.StateExit(this);
                _currentState.isActive = false;

                _currentState = newState;
                _currentState.StateEnter(this); 
                _currentState.isActive = true;
                //GameManager.animState = _currentState._d_stateName;
            }

            return false;
        }


        void ActivateColliders(float powerToImpress)
        {
            
        }
    }
