using UnityEngine;

/// <summary>
/// Scrolla solo verso destra, per cambiare la direzione
/// occorre qualche piccola modifica alle reference e 
/// alla funziona Move()
/// </summary>
public class InfiniteScroll : MonoBehaviour
{
    public SpriteRenderer leftMost;
    public SpriteRenderer rightMost;

    public float speed = 2f;

    private float spriteWidth;
    private Vector2 centerPoint;
    private SpriteRenderer actualRightMostSprite;

    void Start()
    {
        spriteWidth = leftMost.GetComponent<SpriteRenderer>().bounds.size.x;

        centerPoint = leftMost.transform.position;
        actualRightMostSprite = rightMost;
    }

    void Update()
    {
        Move();

        Reposition();
    }

    void Move()
    {
        leftMost.transform.position +=  
            Vector3.left * speed * Time.deltaTime;
        
        rightMost.transform.position = new Vector3
        (
            leftMost.bounds.center.x + 
            spriteWidth,
            rightMost.transform.position.y,
            rightMost.transform.position.z
        );
    }

    void Reposition()
    {
        // ? --- Lato destro sprite
        if(actualRightMostSprite.bounds.center.x + 
            actualRightMostSprite.bounds.extents.x <
            // ? --- Bordo destro di centerPoint
            centerPoint.x + spriteWidth / 2
            )
        {
            leftMost.transform.position = 
                new Vector3
                (
                    // Sarebbe la semplificazione di cP.x + spriteWidth /2 +  sprite Width /2
                    centerPoint.x + spriteWidth,
                    leftMost.transform.position.y,
                    leftMost.transform.position.z
                );
            
            // ? --- La vecchia sprite di sinistra ora
            // ? --- diventa destra 
            SpriteRenderer oldLeftMost = leftMost;
            leftMost = rightMost;
            rightMost = oldLeftMost;

            actualRightMostSprite = rightMost;
        }
    }
}