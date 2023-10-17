using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("Game Over");
    }*/

    public static int numLives;
    public static bool firstLevelLoaded = true;
    [SerializeField]
    private AudioClip ballLostSound;

    private void Start()
    {
        if (firstLevelLoaded)
        {
            numLives = 3;
            firstLevelLoaded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Collectibles" && collision.tag != "Extra Ball")
        {
            Ball.powerBallActive = false;
            numLives--;
            if (numLives < 0)
            {
                SceneManager.LoadScene("Game Over");
                firstLevelLoaded = true;
                Paddle.firstLevelLoaded = true;
                GameplayController.firstLevelLoaded = true;
            }
            else
            {
                GameplayController.instance.CountLives(numLives);
                Ball.instance.SetBallPosition();
            }
            AudioSource.PlayClipAtPoint(ballLostSound, transform.position, 1f);
        }

        if (collision.tag == "Collectibles" || collision.tag == "Extra Ball")
        {
            Destroy(collision.gameObject, 1f);
        }
    }
}
