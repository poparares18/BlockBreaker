using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // config params
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject blockSparklesVFX;
    [SerializeField] Sprite[] hitSprites;

    // cached reference
    Level level;

    // state variables
    [SerializeField] int timesHit; // TODO only serialized for debug purposes

    //new
    [SerializeField]
    private GameObject coin, extraBall;
    private bool isBreakable, isSpecial;

    private void Start()
    {
        isBreakable = (this.tag == "Breakable");
        isSpecial = (this.tag == "Special");
        CountBreakableBlocks();
    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (isBreakable)
        {
            level.CountBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBreakable)
        {
            HandleHit(collision);
            return;
        }
        if (isSpecial)
        {
            HandleSpecial();
        }
        if (Ball.powerBallActive && collision.gameObject.tag != "Extra Ball")
        {
            HandleHit(collision);
        }
    }

    private void HandleHit(Collision2D collision)
    {
        if (Ball.powerBallActive && collision.gameObject.tag != "Extra Ball")
        {
            timesHit += 3;
        }
        else
        {
            timesHit++;
        }
        int maxHits = hitSprites.Length + 1;
        if (timesHit >= maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void HandleSpecial()
    {
        int brickEffect = UnityEngine.Random.Range(0, 2);

        GameObject newCoin = Instantiate(coin, transform.position, Quaternion.identity) as GameObject;
        Destroy(gameObject);

        if (brickEffect == 0)
        {
            int numNewBalls = UnityEngine.Random.Range(2, 5);

            for (int i = 0; i < numNewBalls; i++)
            {
                GameObject newBall = Instantiate(extraBall, transform.position, Quaternion.identity) as GameObject;
                newBall.GetComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(7f, 12f));
            }
            PlayBlockDestroySFX();
        }
        else if (brickEffect == 1)
        {
            Ball.instance.ChangeBallType(brickEffect);
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("Block sprite is missing from array!" + gameObject.name);
        }
    }

    private void DestroyBlock()
    {
        PlayBlockDestroySFX();
        GameObject newCoin = Instantiate(coin, transform.position, Quaternion.identity) as GameObject;
        Destroy(gameObject);
        if (isBreakable)
        {
            level.BlockDestroyed();
        }
        TriggerSparklesVFX();
    }

    private void PlayBlockDestroySFX()
    {
        FindObjectOfType<GameSession>().AddToScore();
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
    }

    private void TriggerSparklesVFX()
    {
        GameObject sparkles = Instantiate(blockSparklesVFX, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
}
