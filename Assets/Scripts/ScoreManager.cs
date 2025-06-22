using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private List<int> levelScores = new List<int>();
    [SerializeField]
    private int totalScore = 0;
    private List<int> highScores = new List<int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddLevelScore(int score)
    {
        levelScores.Add(score);
        totalScore += score;
    }

    public void AddToHighScores()
    {
        highScores.Add(totalScore);
        highScores.Sort((a, b) => b.CompareTo(a));
        if (highScores.Count > 10)
        {
            highScores.RemoveAt(highScores.Count - 1);
        }
    }

    public List<int> GetHighScores()
    {
        return highScores;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }
}
