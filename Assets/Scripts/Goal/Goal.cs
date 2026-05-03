using System;
using UnityEngine;
using UnityEngine.Events;

public class Goal : MonoBehaviour
{
    [Header("References"), SerializeField]
    SpriteRenderer sprite;

    [SerializeField]
    BoxCollider2D coll;
    public void SetScoreCollider(bool isActive)
    {
        coll.enabled = isActive;
    }


    /// <summary>
    /// A chi appartiene la porta?
    /// Se appartiene al player1 dara' il punto all'avversario
    /// </summary>
    [SerializeField]
    public PlayerNumber playerNumber = new();
    
    public void Score()
    {
        if(playerNumber == PlayerNumber.PLAYER_1)
        {
            MatchManager.Get().ScorePlayer2(1);
            GameManager.Get().ball.onBallScore.Invoke();
        }
        else if (playerNumber == PlayerNumber.PLAYER_2)
        {
            MatchManager.Get().ScorePlayer1(1);
            GameManager.Get().ball.onBallScore.Invoke();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

}
