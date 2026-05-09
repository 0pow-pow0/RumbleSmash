using UnityEngine;


///<summary>
/// Auto instanzia due copie di se' stesso flippate se serve
///</summary>
public class InfiniteScrollingBackground : MonoBehaviour
{
    SpriteRenderer mySprite;
    SpriteRenderer rightCopy;
    SpriteRenderer topCopy;

    void Awake()
    {
        mySprite = GetComponent<SpriteRenderer>();

        GameObject gm = new GameObject();
        gm.AddComponent<SpriteRenderer>().sprite = mySprite.sprite;
        //rightCopy.flip
        //topCopy = GameObject.Instantiate(gameObject, gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
