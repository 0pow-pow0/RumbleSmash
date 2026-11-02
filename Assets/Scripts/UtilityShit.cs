// USARE SOLO COME USING NON E' UNO SCRIPT UNITY
using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace UtilityShit
{
    public class PowUtility
    {    
        /// <summary>
        /// Metodo semplificato per verificare le collisioni
        /// </summary>
        public static bool CheckBox(BoxCollider coll, int layerMask)
        {
            //Debug.Log("Center: " + coll.center);

            return 
                Physics.CheckBox(coll.transform.position,
                coll.bounds.size/2, 
                Quaternion.identity,
                layerMask);

        }

        public static Collider[] OverlapBox(BoxCollider coll, int layerMask)
        {
            Collider[] colls = Physics.OverlapBox(coll.transform.position, 
                coll.bounds.size / 2, Quaternion.identity, layerMask);
            return colls;
        }

        public static bool CheckSphere(SphereCollider coll, int layerMask)
        {
            return Physics.CheckSphere(coll.transform.position, coll.radius, layerMask);
        }

        ///<summary>
        /// Permette di colorare il testo O.O :O
        ///</summary>
        public static void Log(object message, Color clr)
        {
            if (clr == null)
            {
                Debug.Log(message);
            }
                Debug.Log("<color=" + "#" + ColorUtility.ToHtmlStringRGB(clr) + ">" + message + "</color>");
        }



        /// <summary>
        /// Per iterare un array di gameObjects e disattivarli
        /// </summary>
        public static void SetActiveObjs(GameObject[] objs, bool activeState)
        {
            for(int i = 0; i < objs.Length; i++)
            {
                objs[i].SetActive(activeState);
            }
        }

        /// <summary>
        /// Ritarda l'esecuzione di una funzione di un tot di secondi.
        /// Non e' protetto da sovrapposizioni della stessa chiamata.
        /// Indipendente da deltaTime
        /// </summary>
        //private static IEnumerator lastCall = null;

    }
    public class Timer
    {
        public float elapsedTime { get; protected set; }
        public float maxTime { get; protected set; }  // Quando il timer scattera'

        public Timer(float max)
        {
            maxTime = max;
        }
        public void UpdateTime()
        {
            // Previeni overflow
            if (elapsedTime < maxTime)
            {
                elapsedTime += Time.deltaTime;
            }
        }

        public bool HasEnded()
        {
            return elapsedTime >= maxTime;
        }

        public void Restart()
        {
            elapsedTime = 0;
        }

        /// <summary>
        /// In pratica lo fa finire.
        /// setta elapsedTime a maxTime, rendendo HasEnded true.
        /// </summary>
        public void End()
        {
            elapsedTime = maxTime;    
        }

        // Cambia il valore massimo utile per sapere se il timer e' scaduto
        public void ChangeMaxTime(float newMax)
        {
            maxTime = newMax;
        }
    }
}