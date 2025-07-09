using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerBehaviour _player;
    [SerializeField] PlatformSpawner _spawner;

    [Header("Ads Settings :")]
    [SerializeField] int _interstitialAdInterval = 3;
    public static int _gameplayCount;

    bool _isGameOver = false;
    bool _isRevive = false;

    ScoreManager _scoreManager;

    public static event Action OnStartGame;
    public static event Action<bool> OnEndGame;

    private void Awake()
    {
        _scoreManager = GetComponent<ScoreManager>();
        _player.OnFirstJump += StartGame;
    }

    private void OnEnable()
    {
        Admob.OnRewardedAdWatched += Revive;
    }
    private void OnDisable()
    {
        Admob.OnRewardedAdWatched -= Revive;
    }

    void StartGame()
    {
        OnStartGame?.Invoke();
    }

    public void EndGame()
    {
        if (_isGameOver) return;

        _isGameOver = true;
        _player.OnGameOver();
        _spawner.OnGameOver();

        OnEndGame?.Invoke(CanRevive());
        if (CanRevive()) _isRevive = true;

        ShowInterstitial();

        SoundController.Instance.PlayAudio(AudioType.GAMEOVER);
    }

    private bool CanRevive()
    {
        bool isRewardLoaded = Admob.Instance.IsRewardedAdLoaded;
        return _scoreManager.Score > 2 && !_isRevive && isRewardLoaded;
    }

    private void Revive()
    {
        _isGameOver = false;
        _player.Revive();
        _spawner.Revive();
    }

    private void ShowInterstitial()
    {
        _gameplayCount++;

        // show interstitial every 3 times (default)
        if (Mathf.Repeat(_gameplayCount, _interstitialAdInterval) == 0)
        {
            Admob.Instance.ShowInterstitialAd();
        }
    }
}