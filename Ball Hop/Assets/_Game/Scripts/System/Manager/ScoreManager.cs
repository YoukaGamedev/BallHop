using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int Score { get; private set; }

    public static event Action<int> OnScoreAdded;

    private void OnEnable()
    {
        Platform.OnCollideWithPlayer += HandleOnCollideWithPlayer;
    }
    private void OnDisable()
    {
        Platform.OnCollideWithPlayer -= HandleOnCollideWithPlayer;
    }

    public void AddScore(int scoreValue)
    {
        Score += scoreValue;
        OnScoreAdded?.Invoke(Score);
    }

    public int GetBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        if (Score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", Score);
            return Score;
        }
        else
        {
            return bestScore;
        }
    }

    // event
    void HandleOnCollideWithPlayer()
    {
        AddScore(1);
    }
}
