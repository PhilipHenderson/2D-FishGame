using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour
{
    [Header("Fish Properties")]
    public int level = 1;
    public float speed = 0.1f;
    public int size = 0;
    public Sprite[] fishSprites;

    public bool isMovementAllowed = true;

    SpriteRenderer spriteRenderer;

    private FishSpawner fishSpawner;
    private bool rSide;
    private Vector3 center;
    private Vector3 rightDeathZone;
    private Vector3 leftDeathZone;

    private float lifespan = 15f;  // Lifespan of the fish in seconds
    private float elapsedTime = 0f;  // Elapsed time since the fish's creation



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Fishnitialization();
        fishSpawner = GameObject.Find("FishSpawner").GetComponent<FishSpawner>();
        center = new Vector3(0,0);
        rightDeathZone = center + new Vector3(15,0);
        leftDeathZone = center + new Vector3(-15,0);
    }

    void Update()
    {
        if (isMovementAllowed)
        {
            elapsedTime += Time.deltaTime;  // Increment the elapsed time
            if (rSide)
            {
                transform.position += new Vector3(-speed, 0);
                if (transform.position.x < leftDeathZone.x)
                {
                    fishSpawner.fishCount--;
                    Destroy(gameObject);
                }
                else if (transform.position.x < leftDeathZone.x + speed)
                {
                    // Adjust the position to prevent stuttering
                    transform.position = new Vector3(leftDeathZone.x + speed, transform.position.y, transform.position.z);
                }
            }
            else
            {
                transform.position += new Vector3(speed, 0);
                if (transform.position.x > rightDeathZone.x)
                {
                    fishSpawner.fishCount--;
                    Destroy(gameObject);
                }
                else if (transform.position.x > rightDeathZone.x - speed)
                {
                    // Adjust the position to prevent stuttering
                    transform.position = new Vector3(rightDeathZone.x - speed, transform.position.y, transform.position.z);
                }
            }

            // Check if the fish has exceeded its lifespan
            if (elapsedTime >= lifespan)
            {
                fishSpawner.fishCount--;
                Destroy(gameObject);
            }
        }
    }


    public void Fishnitialization()
    {
        // Sprite
        GetComponent<SpriteRenderer>().sprite = fishSprites[Random.Range(0,fishSprites.Length)];

        // Start Position
        if (transform.position.x < center.x)
        {
            rSide = false;
        }
        else
        {
            rSide = true;
            spriteRenderer.flipX = true;
        }

        // Level
        level = Random.Range(1, 9);

        // Size
        size += level;
        transform.localScale = new Vector3(size, size);

        if (level < 6)
        {
            speed = Random.Range(0.01f, 0.03f);
        }
        if (level > 6)
        {
            speed = 0.01f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fish"))
        {
            // Ignore collision between the two fish objects
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }
}
