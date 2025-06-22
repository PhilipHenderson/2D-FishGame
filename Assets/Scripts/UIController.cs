using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Fish Symbols
    public Image[] fishSymbols = new Image[5];  // Array of fish symbol images
    public Image levelUpIndicator;

    public TMP_Text levelTxt;
    public TMP_Text killsTxt;
    public TMP_Text scoreTxt;
    public TMP_Text stageTxt;

    public TMP_Text pausedScoreTxt;
    public TMP_Text winScoreTxt;
    public TMP_Text endScoreTxt;

    public int playerLevel;
    public int playerKills;
    public float playerScore;
    public float tempScore;
    public int tempKills;

    public TMP_Text gameOverStageTxt;
    public TMP_Text gameOverLevelTxt;
    public TMP_Text gameOverTimeTxt;
    public TMP_Text gameOverKillsTxt;
    public TMP_Text gameOverScoreTxt;

    public TMP_Text winStageTxt;
    public TMP_Text winLevelTxt;
    public TMP_Text winTimeTxt;
    public TMP_Text winKillsTxt;

    private ScoreManager scoreManager;
    public TimeSpan currentT;


    public Sprite eatenIcon;     // Sprite for the eaten icon
    public Sprite unEatenIcon;   // Sprite for the uneaten icon
    //public Sprite levelUpIndicator;

    public int foodEatenUI = 0;  // Counter for the amount of food eaten
    public TimeSpan currentTime;

    public TMP_Text timeTxt;
    private Coroutine timerCoroutine;

    PlayerController playerController;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        currentTime = TimeSpan.Zero; // Initialize currentTime as TimeSpan.Zero
        foodEatenUI = playerController.foodEaten;
        fishSymbols[0].sprite = unEatenIcon;
        fishSymbols[1].sprite = unEatenIcon;
        fishSymbols[2].sprite = unEatenIcon;
        timerCoroutine = StartCoroutine(UpdateTimer()); // Assign the timer coroutine
        scoreManager = ScoreManager.instance;
    }

    private void FixedUpdate()
    {
        UpdateScore();
    }

    private void Update()
    {
        UpdateFishSymbols();
        UpdateLevel();
        UpdateKills();
        UpdateStage();
    }

    public void UpdateFishSymbols()
    {
        for (int i = 0; i < fishSymbols.Length; i++)
        {
            if (i < foodEatenUI)
            {
                fishSymbols[i].sprite = eatenIcon;
            }
            else
            {
                fishSymbols[i].sprite = unEatenIcon;
            }
        }

        // Toggle visibility of the level up indicator based on the number of fish symbols eaten
        if (foodEatenUI >= 3)
        {
            levelUpIndicator.gameObject.SetActive(true);
        }
        else
        {
            levelUpIndicator.gameObject.SetActive(false);
        }
    }
    public void UpdateStage()
    {
        int currentStage = FindObjectOfType<GameManagerScript>().currentStage;
        stageTxt.text = "Stage: " + currentStage.ToString();
        gameOverStageTxt.text = currentStage.ToString();
        winStageTxt.text = currentStage.ToString(); // Update the win screen stage text
    }
    public void UpdateLevel()
    {
        playerLevel = playerController.level;
        levelTxt.text = "Level: " + playerLevel.ToString();
        gameOverLevelTxt.text = playerLevel.ToString();
        winLevelTxt.text = playerLevel.ToString(); // Update the win screen level text
    }
    public void UpdateKills()
    {
        playerKills = playerController.totalKills;
        killsTxt.text = "Kills: " + playerKills.ToString();
        gameOverKillsTxt.text = playerKills.ToString();
        winKillsTxt.text = playerKills.ToString(); // Update the win screen kills text
    }
    public void UpdateScore()
    {
        playerScore = playerController.score;
        int roundedScore = Mathf.RoundToInt(playerScore);
        scoreTxt.text = "Score: " + roundedScore.ToString();
        pausedScoreTxt.text = "Score: " + roundedScore.ToString();
        winScoreTxt.text = roundedScore.ToString(); // Update the win screen score text
        endScoreTxt.text = roundedScore.ToString();
    }
    public void UpdateWinTime(TimeSpan time)
    {
        winTimeTxt.text = time.ToString(@"hh\:mm\:ss");
    }
    IEnumerator UpdateTimer()
    {
        TimeSpan startTime = TimeSpan.FromSeconds(Time.time);

        while (true)
        {
            currentTime = TimeSpan.FromSeconds(Time.time) - startTime;
            currentT = currentTime;
            timeTxt.text = "Time: " + currentTime.ToString(@"hh\:mm\:ss");
            winTimeTxt.text = currentTime.ToString(@"hh\:mm\:ss"); // Update the win screen time text
            yield return new WaitForSeconds(1f);
        }
    }

    public void RestartTimer()
    {
        StopAllCoroutines();
        StartTimer();
    }
    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
    }
    public void StartTimer()
    {
        if (timerCoroutine != null)
        {
            StartCoroutine(UpdateTimer());
        }
    }
    public void ResetUI()
    {
        foodEatenUI = 0;
        // Reset fish symbols
        for (int i = 0; i < fishSymbols.Length; i++)
        {
            fishSymbols[i].sprite = unEatenIcon;
        }

        // Reset level indicator
        levelUpIndicator.gameObject.SetActive(false);

        // Reset level, kills, and score text
        levelTxt.text = "Level: 2";
        killsTxt.text = "Kills: 0";
        scoreTxt.text = "Score: 0";

        // Restart the timer
        RestartTimer();
    }
}
