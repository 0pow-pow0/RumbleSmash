using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBallCollider : MonoBehaviour
{
    Player plr;
    public int lastPowerImpressed;
    public Vector2 lastDirectionImpressedNormalized;

    // ? --- Quanti frame passano prima che il collder si disattivi?
    // ? --- Il valore viene settato sulla base di quello in 
    // ? --- "PlayerBallInteractions"
    public int numberOfFramesBeforeDeactivation;

    /// <summary>
    /// Serve per dare un delay alla DISATTIVAZIONE del collider
    /// </summary>
    IEnumerator ColliderTimer()
    {
        //gameObject.SetActive(true);
        //Debug.Log("Activated");
        //Debug.Log("Frames:"  + numberOfFramesBeforeDeactivation);
        // ? --- Aspetta X frames
        for(int i = 0; i < numberOfFramesBeforeDeactivation; i++)
        {
            //Debug.Log("Waited");
            yield return null;
        }
        //Debug.Log("Deactivated");

        gameObject.SetActive(false);
    }

    public void Activate(int _powerImpressed, Vector2 _directionImpressed)
    {
        lastPowerImpressed = _powerImpressed;
        lastDirectionImpressedNormalized = _directionImpressed; 

        // ? --- Il gameobject deve essere attivo per startare coroutine
        gameObject.SetActive(true);
        StartCoroutine("ColliderTimer");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Pisello: " + other.gameObject.layer);
        if(other.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            Debug.Log("Ball hit!");
            Debug.Log("Power: " + lastPowerImpressed);
            Debug.Log("Direction: " + lastDirectionImpressedNormalized);

            Ball bl =  other.GetComponentInParent<Ball>();
  
            bl.AddForce(lastDirectionImpressedNormalized, lastPowerImpressed);
            
            
            plr.pbi.onBallHit.Invoke(new Vector2
            (
                bl.transform.position.x,
                bl.transform.position.y
            ));

            // ! --- Disattiva il collider dopo aver colliso con la palla
            StopCoroutine("ColliderTimer");
            gameObject.SetActive(false);
        }  
    }

    void Awake()
    {
        plr = GetComponentInParent<Player>();
        numberOfFramesBeforeDeactivation =
            GetComponentInParent<PlayerBallInteractions>().
                KICK_COLLIDER_FRAME_DURATION;
    }

}
