using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayController : MonoBehaviour
{
    public static GameplayController instance;

    [SerializeField]
    private TextMeshProUGUI lifeCount, coinCount;

    public static int coinsForLevelUp;
    public static int coinInitialValue;

    [SerializeField]
    private int coinIntervalForLevelUp = 500;

    public static bool firstLevelLoaded = true;

    GameSession theSession;

    private void Awake()
    {
        MakeInstance();
    }

    void Start()
    {
        theSession = FindObjectOfType<GameSession>();
        if (firstLevelLoaded)
        {
            coinsForLevelUp = coinIntervalForLevelUp;
            coinInitialValue = coinIntervalForLevelUp;
            print("Coins needed for level up is " + coinsForLevelUp);
            firstLevelLoaded = false;
        }

        CountLives(LoseCollider.numLives);
        CountCoins(Paddle.coinCount);
    }

    // Update is called once per frame
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
            print("making instance");
        }
    }

    public void CountLives(int numLives)
    {
        Debug.Log("Number of lives is " + numLives);
        lifeCount.text = "Lives: " + numLives;
    }

    public void CountCoins(int numCoins)
    {
        if (numCoins >= coinsForLevelUp)
        {
            LoseCollider.numLives++;
            theSession.PlayLevelUpSound();
            Debug.Log("Played");
            CountLives(LoseCollider.numLives);
            coinsForLevelUp += coinInitialValue;
        }

        coinCount.text = "Coins: " + numCoins;
    }
}
