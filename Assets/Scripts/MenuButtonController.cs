using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuButtonController : MonoBehaviour
{
    [Header("Menus")]
    public GameObject SettingsMenu;
    public GameObject highScoreMenu;

    [Header("Main Menu Titles")]
    public GameObject Image1;
    public GameObject Image2;

    [Header("Settings Titles")]
    public GameObject Image3;
    public GameObject Image4;

    [Header("HighScoreTitles")]
     public GameObject Image5 = null;
    public GameObject Image6 = null;

    [Header("Backgrounds")]
    public GameObject mainBG1;
    public GameObject mainBG2;
    public GameObject settingsHighscoreBG1;
    public GameObject settingsHighscoreBG2;

    [Header("Particals")]
    public GameObject mainMenuParticals1;
    public GameObject mainMenuParticals2;
    public GameObject settingsParticals1;
    public GameObject settingsParticals2;
    public GameObject highScoreParticals1 = null;
    public GameObject highScoreParticals2 = null;

    private void Start()
    {
        if (SettingsMenu != null)
        {
            SettingsMenu.SetActive(false);
            Image3.SetActive(false);
            Image4.SetActive(false);
            settingsHighscoreBG1.SetActive(false);
            settingsHighscoreBG2.SetActive(false);
            settingsParticals1.SetActive(false);
            settingsParticals2.SetActive(false);
        }
        if (highScoreMenu != null)
        {
            highScoreMenu.SetActive(false);
            Image5.SetActive(false);
            Image6.SetActive(false);
            highScoreParticals1.SetActive(false);
            highScoreParticals2.SetActive(false);
        }
    }

    public void PlayButton()
    {
        // Load the game scene
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenuButton()
    {
        // Load the credits scene
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1.0f;
    }


    public void QuitButton()
    {
        // Quit the application (works only in builds, not in the Unity editor)
        Application.Quit();
    }
}
