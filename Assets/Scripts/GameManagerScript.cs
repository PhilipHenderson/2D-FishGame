using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameManagerScript : MonoBehaviour
{
    public int targetPerStage = 10;
    public int currentStage;
    public GameObject pauseScreen;
    public GameObject winScreen;
    public GameObject endScreen;
    public GameObject player;

    private PlayerController playerController;
    private FishController fishController;
    private UIController uiController;
    private FishSpawner fishSpawner;
    private CoralPlacement coralPlacement;

    private bool isPaused = false;
    private bool levelEnded = false;
    private bool isGameOver = false;
    private bool retry = false;

    public List<Button> resumeButtons;
    public List<Button> restartButtons;
    public List<Button> nextButtons;
    public List<Button> exitButtons;

    private void Awake()
    {
        player = Instantiate(player);
        currentStage = 1;
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        fishController = FindObjectOfType<FishController>();
        fishSpawner = FindObjectOfType<FishSpawner>();
        uiController = FindObjectOfType<UIController>();
        coralPlacement = FindObjectOfType<CoralPlacement>();

        // Set the initial state of the screens
        pauseScreen.SetActive(false);
        winScreen.SetActive(false);
        endScreen.SetActive(false);

        AttachButtonListeners(resumeButtons, ResumeOnClick);
        AttachButtonListeners(restartButtons, RestartOnClick);
        AttachButtonListeners(nextButtons, NextOnClick);
        AttachButtonListeners(exitButtons, ExitOnClick);
    }

    private void AttachButtonListeners(List<Button> buttons, UnityEngine.Events.UnityAction clickAction)
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(clickAction);
        }
    }

    private void Update()
    {
        // Check for pause input
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        // Check if player reached target level
        if (!levelEnded && playerController.level >= targetPerStage)
        {
            ShowWinScreen();
        }

        // Check if player is destroyed
        if (playerController.IsDestroyed())
        {
            ShowEndScreen();
        }
    }

    public void ResumeOnClick()
    {
        ResumeGame();
    } // Done
    public void ResumeGame()
    {
        // Resume the game
        Time.timeScale = 1f;
        // Hide the score screen and other screens
        pauseScreen.SetActive(false);
        winScreen.SetActive(false);
        endScreen.SetActive(false);
        // Resume the movement of fish and player
        playerController.isMovementAllowed = true;
        // Stop the movement of each fish individually
        FishController[] fishControllers = FindObjectsOfType<FishController>();
        foreach (FishController fishController in fishControllers)
        {
            fishController.isMovementAllowed = true;
        }
        isPaused = false;
    } // Done
    public void PauseGame()
    {
        // Pause the game
        Time.timeScale = 0f;
        // Show the pause screen
        pauseScreen.SetActive(true);
        // Stop the movement of fish and player
        playerController.isMovementAllowed = false;

        // Stop the movement of each fish individually
        FishController[] fishControllers = FindObjectsOfType<FishController>();
        foreach (FishController fishController in fishControllers)
        {
            fishController.isMovementAllowed = false;
        }
    } // Done

    public void RestartOnClick()
    {
        // Reset player stats if the object is not null
        if (playerController != null)
        {
            if (isGameOver == false)
            {
                if (!retry)
                {
                    // Reset player stats
                    uiController.tempScore = uiController.playerScore;
                    uiController.tempKills = playerController.totalKills;
                    playerController.ResetStats();
                    // Reset any other necessary variables or game state
                    uiController.playerScore = uiController.tempScore;
                    playerController.totalKills = uiController.tempKills;
                    uiController.tempScore = 0.0f;
                    uiController.tempKills = 0;
                }
                else
                {
                    // Reset player stats
                    uiController.tempScore = uiController.playerScore;
                    playerController.ResetStats();
                    // Reset any other necessary variables or game state
                    uiController.playerScore = uiController.tempScore;
                    uiController.tempScore = 0.0f;
                    uiController.tempKills = 0;
                    retry = false;
                }
            }
            else
            {
                playerController.ResetStats();
            }
            Time.timeScale = 1.0f;
        }

        // Reset spawner
        fishSpawner.ResetSpawner();

        // Reset UI
        uiController.ResetUI();

        // Reset the timer
        uiController.RestartTimer();

        // Hide the game over screen
        endScreen.SetActive(false);
        winScreen.SetActive(false);
        pauseScreen.SetActive(false);

        // Resume player movement if the object is not null
        if (playerController != null)
        {
            playerController.ResumeMovement();
        }
        else
            Debug.Log("Player Controller not found");

        // Start the game
        isGameOver = false;

        playerController.transform.position = Vector3.zero;
        Time.timeScale = 1.0f;
        FishController[] fishControllers = FindObjectsOfType<FishController>();
        foreach (FishController fishController in fishControllers)
        {
            fishController.isMovementAllowed = true;
            Destroy(fishController.gameObject);
        }
        coralPlacement.RegenerateCoral();
    }
    public void NextOnClick()
    {
        // Increment the current stage
        currentStage++;
        coralPlacement.RegenerateCoral();
        // Reset the necessary stats in the player controller
        uiController.tempKills = playerController.totalKills;
        playerController.ResetStats();
        playerController.totalKills = uiController.tempKills;
        uiController.tempKills = 0;

        // Restart the timer in the UIController
        uiController.RestartTimer();

        // Load the next level or perform any necessary actions
        // SceneManager.LoadScene("NextLevel");
        winScreen.SetActive(false);


        // Restart the level
        RestartOnClick();
        Time.timeScale = 1.0f;
    } // Done
    public void ExitOnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    } // Done

    public void ShowWinScreen()
    {
        Time.timeScale = 0.0f;
        playerController.gameWon = true;
        retry = true;
        // Show the win screen
        winScreen.SetActive(true);
        // Pause the game or perform any necessary actions
    } // Done
    public void ShowEndScreen()
    {
        Time.timeScale = 0f;
        playerController.gameLost = true;
        isGameOver = true;
        // Show the end screen
        endScreen.SetActive(true);
        // Pause the game or perform any necessary actions

        // Get the UIController component
        UIController uiController = FindObjectOfType<UIController>();
        // Update the end screen timer text with the currentT value
        uiController.gameOverTimeTxt.text = uiController.currentT.ToString(@"hh\:mm\:ss");
    }
}
