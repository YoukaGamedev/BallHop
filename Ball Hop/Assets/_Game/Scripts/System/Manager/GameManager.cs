using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerBehaviour _player;
    [SerializeField] private PlatformSpawner _spawner;
    [SerializeField] private ScoreManagerGINVO _scoreManagerGINVO; // Untuk submit skor ke GINVO WebGL
    [SerializeField] private ScoreManager _scoreManager; // Menghitung skor gameplay

    public static int _gameplayCount;

    private bool _isGameOver = false;
    private bool _isRevive = false;

    public static event Action OnStartGame;
    public static event Action<bool> OnEndGame;

    private void Awake()
    {
        // Pastikan semua komponen terhubung
        if (_player == null) _player = FindFirstObjectByType<PlayerBehaviour>();
        if (_spawner == null) _spawner = FindFirstObjectByType<PlatformSpawner>();
        if (_scoreManager == null) _scoreManager = GetComponent<ScoreManager>();

        // Subscribe ke event PlayerBehaviour
        if (_player != null)
        {
            _player.OnFirstJump += StartGame;
        }
        else
        {
            Debug.LogError("PlayerBehaviour belum di-assign di GameManager!");
        }
    }

    private void StartGame()
    {
        OnStartGame?.Invoke();
    }

    public void EndGame()
    {
        if (_isGameOver) return;

        _isGameOver = true;

        // Matikan kontrol player & spawner
        if (_player != null) _player.OnGameOver();
        if (_spawner != null) _spawner.OnGameOver();

        // Kirim event game over
        OnEndGame?.Invoke(CanRevive());
        if (CanRevive()) _isRevive = true;

        // Mainkan suara game over
        if (SoundController.Instance != null)
            SoundController.Instance.PlayAudio(AudioType.GAMEOVER);

        // Submit skor ke leaderboard
        int playerScore = (_scoreManager != null) ? _scoreManager.Score : 0;

        if (_scoreManagerGINVO != null)
        {
            _scoreManagerGINVO.SubmitPlayerScore(playerScore);
        }
        else
        {
            Debug.LogWarning("ScoreManagerGINVO belum di-assign, skor tidak terkirim!");
        }
    }

    private bool CanRevive()
    {
        return (_scoreManager != null && _scoreManager.Score > 2 && !_isRevive);
    }

    private void Revive()
    {
        _isGameOver = false;
        if (_player != null) _player.Revive();
        if (_spawner != null) _spawner.Revive();
    }
}
