using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private UIController uiController;
    private FishSpawner spawner;
    private GameManagerScript gameManagerScript;

    private Vector2 screenBounds;

    [Header("Player Properties")]
    [SerializeField]
    private float speed;

    public int level;
    public int foodEaten;
    public float score;
    public int totalKills;

    public bool isMovementAllowed = true;
    public bool gameWon;
    public bool gameLost;

    private void Start()
    {
        // Get the reference to the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        uiController = FindObjectOfType<UIController>();
        spawner = FindObjectOfType<FishSpawner>();
        gameManagerScript = FindObjectOfType<GameManagerScript>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        transform.localScale = new Vector3(2,2);
    }

    void Update()
    {
        if (isMovementAllowed)
        {
            // Get input axes
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            // Wrap the player's position to the other side if it goes off-screen
            if (transform.position.x > screenBounds.x)
            {
                transform.position = new Vector3(-screenBounds.x, transform.position.y, transform.position.z);
            }
            else if (transform.position.x < -screenBounds.x)
            {
                transform.position = new Vector3(screenBounds.x, transform.position.y, transform.position.z);
            }

            if (transform.position.y > screenBounds.y)
            {
                transform.position = new Vector3(transform.position.x, -screenBounds.y, transform.position.z);
            }
            else if (transform.position.y < -screenBounds.y)
            {
                transform.position = new Vector3(transform.position.x, screenBounds.y, transform.position.z);
            }

            // Calculate movement vector
            Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f) * speed * Time.deltaTime;

            // Apply movement to the player's position
            transform.position += movement;

            // Flip the sprite horizontally if moving left
            if (moveHorizontal < 0)
            {
                spriteRenderer.flipX = true;
            }
            // Flip the sprite horizontally if moving right
            else if (moveHorizontal > 0)
            {
                spriteRenderer.flipX = false;
            }
            if (Time.time >= 1f)
            {
                score = totalKills * 2;

                if (gameWon || gameLost)
                {
                    score /= 2;
                }

                uiController.UpdateScore();
            }

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fish")
        {
            FishController enemy = collision.gameObject.GetComponent<FishController>();

            if (enemy != null)
            {
                if (enemy.level < level)
                {
                    // Destroy the enemy fish
                    Destroy(collision.gameObject);
                    spawner.fishCount--;

                    // kills = 0 and level player up to next level
                    if (foodEaten == 3)
                    {
                        // Level Up
                        LevelUp();
                        foodEaten = 0;
                        totalKills++;
                    }
                    // if less then the third kill, increase Kills and Decrease kills till next level
                    else
                    {
                        foodEaten++;
                        totalKills++;
                        uiController.UpdateFishSymbols();
                        uiController.foodEatenUI++;
                    }
                }
                else
                {
                    // Move the player off-screen
                    transform.position = new Vector3(25f, 25f);

                    // Stop the player movement
                    isMovementAllowed = false;

                    // Stop the movement of each fish individually
                    FishController[] fishControllers = FindObjectsOfType<FishController>();
                    foreach (FishController fishController in fishControllers)
                    {
                        fishController.isMovementAllowed = false;
                    }

                    // Pause the game
                    Time.timeScale = 0f;
                    // Show the pause screen
                    gameManagerScript.ShowEndScreen();
                }
            }
        }
        // If (collision.gameObject.tag == "PowerUp")
    }

    public void ResumeMovement()
    {
        isMovementAllowed = true;
    }

    public void LevelUp()
    {
        transform.localScale += new Vector3(1.0f, 1.0f, 1.0f);
        level += 1;
        speed += 1;

        // Reset foodEaten and update UI fish symbols
        foodEaten = 0;
        uiController.foodEatenUI = foodEaten;
        uiController.UpdateFishSymbols();

        // Update UI level text
        uiController.UpdateLevel();
    }

    public void ResetStats()
    {
        transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        level = 2;
        speed = 2;
        totalKills = 0;
        score = 0;
        foodEaten = 0;
        // Reset any other variables as needed
    }

}