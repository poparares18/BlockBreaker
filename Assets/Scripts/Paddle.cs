using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    // configuration parameters
    [SerializeField] float minX = 1f;
    [SerializeField] float maxX = 15f;
    [SerializeField] float screenWidthInUnits = 16f;

    // cached references
    GameSession theGameSession;
    Ball theBall;

    //new
    public static int coinCount;
    public static bool firstLevelLoaded = true;
    public AudioClip coinSound;
    public int minCoinValue = 10;
    public int maxCoinValue = 20;

    // Start is called before the first frame update
    void Start()
    {
        theGameSession = FindObjectOfType<GameSession>();
        theBall = FindObjectOfType<Ball>();
        if (firstLevelLoaded)
        {
            coinCount = 0;
            firstLevelLoaded = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 paddlePos = new Vector2(transform.position.x, transform.position.y);
        paddlePos.x = Mathf.Clamp(GetXPos(), minX, maxX);
        transform.position = paddlePos;
    }

    private float GetXPos()
    {
        if (theGameSession.IsAutoPlayEnabled())
        {
            return theBall.transform.position.x;
        }
        else
        {
            return Input.mousePosition.x / Screen.width * screenWidthInUnits;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectibles")
        {
            int coinValue = Random.Range(minCoinValue, maxCoinValue);
            coinCount += coinValue;
            GameplayController.instance.CountCoins(coinCount);
            AudioSource.PlayClipAtPoint(coinSound, transform.position, 1f);
        }

        Destroy(collision.gameObject);
    }

}
